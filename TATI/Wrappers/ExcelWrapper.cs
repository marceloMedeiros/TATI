using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Wrappers
{
    public class ExcelWrapper : IDisposable
    {
        private IRow row;
        private ISheet sheet1;
        private HSSFWorkbook hssfworkbook;
        private int LinhaExcel = 0;
        public int TotalDeLinhas { get { return LinhaExcel; } }

        public ExcelWrapper(string companyName, string subject, string sheetName)
        {
            hssfworkbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = companyName;
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = subject;
            hssfworkbook.SummaryInformation = si;
            sheet1 = hssfworkbook.CreateSheet(sheetName);
            row = sheet1.CreateRow(LinhaExcel);
        }

        public ExcelWrapper(System.IO.Stream sIOs)
        {
            hssfworkbook = new HSSFWorkbook(sIOs);
            //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //dsi.Company = companyName;
            //hssfworkbook.DocumentSummaryInformation = dsi;
            //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //si.Subject = subject;
            //hssfworkbook.SummaryInformation = si;
            sheet1 = hssfworkbook.GetSheetAt(0);
            row = sheet1.GetRow(0);
        }

        public void IncrementaLinha()
        {
            row = sheet1.CreateRow(++LinhaExcel);
        }

        public void AdicionaConteudo(int conteudo, uint coluna)
        {
            ICell qntCell = row.CreateCell((int)coluna);
            qntCell.SetCellType(CellType.Numeric);
            qntCell.SetCellValue(conteudo);
        }

        public void AdicionaConteudo(double conteudo, uint coluna)
        {
            ICell qntCell = row.CreateCell((int)coluna);
            qntCell.SetCellType(CellType.Numeric);
            qntCell.SetCellValue(conteudo);
        }

        public void AdicionaConteudo(string conteudo, uint coluna)
        {
            ICell qntCell = row.CreateCell((int)coluna);
            qntCell.SetCellType(CellType.String);
            qntCell.SetCellValue(conteudo);
        }

        public void AdicionaFormula(string formula, uint coluna)
        {
            ICell qntCell = row.CreateCell((int)coluna);
            qntCell.SetCellType(CellType.Formula);
            qntCell.SetCellFormula(formula);
        }

        public void GeraHeader(bool activateHeader, params string[] headerRows)
        {
            uint cnt = 0;
            foreach(var headerName in headerRows)
            {
                AdicionaConteudo(headerName, cnt++);
            }
            IncrementaLinha();
        }

        public void AjustaHeader()
        {
            int numberOfColumns = sheet1.GetRow(0).PhysicalNumberOfCells;
            for (int i = 1; i <= numberOfColumns; i++)
            {
                sheet1.AutoSizeColumn(i);
                GC.Collect();
            }
        }

        public void GeraConteudoDoExcel(DataTable dt)
        {
            throw new NotImplementedException();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var column = dr[j] as DataColumn;
                    ICell qntCell = row.CreateCell(j);
                    qntCell.SetCellType(GetCellTypeFromColumn(column));
                    qntCell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
        }

        public void Dispose()
        {
        }

        public void WriteToMemory(MemoryStream stream)
        {
            this.hssfworkbook.Write(stream);
        }

        public bool ConverterParaDataTable(out DataTable dt) => ConverterParaDataTable(out dt, false);

        public bool ConverterParaDataTable(out DataTable dt, bool cabecalho)
        {
            return ConverterParaDataTable(out dt, cabecalho, false);
        }

        public bool ConverterParaDataTable(out DataTable dt, bool cabecalho, bool preservaLinhasVazias)
        {
            return ConverterParaDataTable(out dt, cabecalho, preservaLinhasVazias, sheet1);
        }

        public bool ConverterParaDataTable(out DataTable dt, bool cabecalho, bool preservaLinhasVazias, ISheet sh)
        {

            bool result = false;
            DataTable dtSheet = new DataTable();
            dtSheet.Rows.Clear();
            dtSheet.Columns.Clear();

            try
            {
                int i = 0;
                while (sh.GetRow(i) != null)
                {
                    // add neccessary columns
                    if (dtSheet.Columns.Count < sh.GetRow(i).Cells.Count)
                    {
                        for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
                        {
                            if (cabecalho)
                            {
                                try
                                {
                                    dtSheet.Columns.Add(sh.GetRow(0).GetCell(j).StringCellValue, typeof(string));
                                }
                                catch (Exception)
                                {
                                    dtSheet.Columns.Add("", typeof(string));
                                }
                            }
                            else
                                dtSheet.Columns.Add("", typeof(string));
                        }
                    }

                    // add row
                    dtSheet.Rows.Add();

                    // write row value
                    for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
                    {
                        var cell = sh.GetRow(i).GetCell(j);

                        if (cell != null)
                        {
                            // TODO: you can add more cell types capatibility, e. g. formula
                            switch (cell.CellType)
                            {
                                case NPOI.SS.UserModel.CellType.Numeric:
                                    dtSheet.Rows[i][j] = sh.GetRow(i).GetCell(j).NumericCellValue;
                                    //dataGridView1[j, i].Value = sh.GetRow(i).GetCell(j).NumericCellValue;

                                    break;
                                case NPOI.SS.UserModel.CellType.String:
                                    dtSheet.Rows[i][j] = sh.GetRow(i).GetCell(j).StringCellValue;

                                    break;
                            }
                        }
                    }

                    i++;
                }

                if (dtSheet.Rows.Count > 0)
                {
                    if (cabecalho)
                        dtSheet.Rows[0].Delete();
                }

                if (dtSheet.Rows.Count > 0)
                {
                    if (!preservaLinhasVazias)
                    {
                        var query = dtSheet.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string)));

                        if (query.Any())
                            dtSheet = query.CopyToDataTable();
                        else
                            dtSheet.Rows.Clear();
                    }
                }

                dt = dtSheet;
                result = dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                dt = null;
                result = false;
            }

            return result;
        }

        private CellType GetCellTypeFromColumn(DataColumn dc)
        {
            if (dc.DataType == Type.GetType("System.Boolean"))
                return CellType.Boolean;
            else if (dc.DataType == Type.GetType("System.String") ||
                     dc.DataType == Type.GetType("System.DateTime"))
                return CellType.String;
            else if (dc.DataType == Type.GetType("System.Int16") ||
                    dc.DataType == Type.GetType("System.Int32") ||
                    dc.DataType == Type.GetType("System.Int64") ||
                    dc.DataType == Type.GetType("System.Decimal"))
                return CellType.Numeric;
            else
                return CellType.Unknown;
        }
    }
}
