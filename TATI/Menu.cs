﻿using Extensions;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TATI.Models;
using Wrappers;

namespace TATI
{
    public partial class Menu : Form
    {
        Usuario usuarioLogin;

        public Menu(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogin = usuario;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(695, 600);
            this.Text += "   - Usuário ativo: " + usuarioLogin.Nome
                         + "   - Nível de Acesso: " + (usuarioLogin.Administrador ? "Administrador" : "Usuário");

            if (usuarioLogin.Administrador)
            {
                Extensions.MyExtensions.EnableTab(tabAprovacaoDocumentos, true);
            }
            else
            {
                Extensions.MyExtensions.EnableTab(tabAprovacaoDocumentos, false);
            }
        }

        public string MemoryStreamParaArquivo(MemoryStream ms) => MemoryStreamParaArquivo(ms, String.Empty, false);
        public string MemoryStreamParaArquivo(MemoryStream ms, string fileName, bool tempFile)
        {
            string nomeArquivo = string.Empty;
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            if (tempFile)
            {
                nomeArquivo = Path.GetTempFileName().Replace(".tmp", "") + fileName;
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = fileName,
                    OverwritePrompt = true,
                    RestoreDirectory = true
                };
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    nomeArquivo = sfd.FileName;
                }
                else
                {
                    return String.Empty;
                }
            }

            using (FileStream file = new FileStream(nomeArquivo, FileMode.Create, System.IO.FileAccess.Write))
            {
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, (int)ms.Length);
                file.Write(bytes, 0, bytes.Length);
                ms.Close();
            }
            return nomeArquivo;
        }

        public void ByteParaArquivo(byte[] bytes, string fileName)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                FileName = fileName,
                OverwritePrompt = true,
                RestoreDirectory = true
            };
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                using (FileStream file = new FileStream(sfd.FileName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    file.Write(bytes, 0, bytes.Length);
                }
            }
        }

        #region Motorista

        private void LimparMotorista()
        {
            btnMotoristaIncluir.Enabled = true;
            btnMotoristaAlterar.Enabled = false;
            btnMotoristaExcluir.Enabled = false;

            txtMotoristaMotoristaID.Text = String.Empty;
            txtMotoristaDataHoraCriacao.Text = String.Empty;
            txtMotoristaCelular.Text = String.Empty;
            txtMotoristaCPFouCNPJ.Text = String.Empty;
            txtMotoristaNome.Text = String.Empty;
            txtMotoristaEmail.Text = String.Empty;
            rdbCPF.Checked = true;

            CarregarGridMotorista();
            txtMotoristaNome.Focus();
        }

        private void CarregarGridMotorista()
        {

            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Motoristas.ToList();
                dgvMotoristas.DataSource = bi;
                dgvMotoristas.Refresh();
            }
            txtMotoristaNumeroRegistros.Text = dgvMotoristas.RowCount.ToString();
        }

        private void CarregarDadosMotorista(Int32 ID)
        {
            using (var context = new CadastroMotoristaContext())
            {
                var motorista = context.Motoristas
                                     .Where(a => a.MotoristaID == ID).FirstOrDefault<Motorista>();
                if (motorista != null)
                {
                    txtMotoristaMotoristaID.Text = motorista.MotoristaID.ToString();
                    txtMotoristaDataHoraCriacao.Text = motorista.DataHoraCriacao.ToString();
                    txtMotoristaCelular.Text = motorista.Celular;
                    txtMotoristaNome.Text = motorista.Nome;
                    txtMotoristaEmail.Text = motorista.Email;

                    //if (MyExtensions.IsCnpj(motorista.CPFouCNPJ))
                    if (motorista.CPFouCNPJ.ToString().Length > 14)
                    {
                        rdbCNPJ.Checked = true;
                    }
                    else
                    {
                        rdbCPF.Checked = true;
                    }

                    txtMotoristaCPFouCNPJ.Text = motorista.CPFouCNPJ;

                    btnMotoristaIncluir.Enabled = false;
                    btnMotoristaAlterar.Enabled = true;
                    btnMotoristaExcluir.Enabled = true;
                }
            }
        }

        private void tabMotoristas_Enter(object sender, EventArgs e)
        {
            LimparMotorista();
        }

        private void btnMotoristaIncluir_Click(object sender, EventArgs e)
        {
            Motorista motorista = new Motorista
            {
                Celular = txtMotoristaCelular.Text,
                CPFouCNPJ = txtMotoristaCPFouCNPJ.Text,
                Nome = txtMotoristaNome.Text,
                DataHoraCriacao = DateTime.Now,
                Email = txtMotoristaEmail.Text
            };

            using (var context = new CadastroMotoristaContext())
            {
                var result = context.Motoristas.Add(motorista);
                context.SaveChanges();
            }
            LimparMotorista();

        }

        private void btnMotoristaAlterar_Click(object sender, EventArgs e)
        {

            using (var context = new CadastroMotoristaContext())
            {
                var motoristaID = txtMotoristaMotoristaID.Text.ConvertToInt();
                Motorista motorista = context.Motoristas
                                .Where(a => a.MotoristaID == motoristaID).FirstOrDefault<Motorista>();

                motorista.Celular = txtMotoristaCelular.Text;
                motorista.CPFouCNPJ = txtMotoristaCPFouCNPJ.Text;
                motorista.Nome = txtMotoristaNome.Text;
                motorista.Email = txtMotoristaEmail.Text;
                context.SaveChanges();
            }
            LimparMotorista();
        }

        private void btnMotoristaExcluir_Click(object sender, EventArgs e)
        {
            using (var context = new CadastroMotoristaContext())
            {
                try
                {

                    var motoristaID = txtMotoristaMotoristaID.Text.ConvertToInt();
                    Motorista motorista = context.Motoristas
                                    .Where(a => a.MotoristaID == motoristaID).FirstOrDefault<Motorista>();
                    context.Entry(motorista).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is System.Data.Entity.Core.UpdateException)
                    {
                        if (ex.InnerException.InnerException != null && ex.InnerException.InnerException is System.Data.SqlClient.SqlException)
                        {
                            MessageBox.Show("Não é possível excluir esse registro, pois ele têm dependência com outro registro incluído no banco de dados.", "Erro");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            LimparMotorista();
        }

        private void btnMotoristaLimpar_Click(object sender, EventArgs e)
        {
            LimparMotorista();
        }

        private void rdbCPF_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCPF.Checked)
            {
                txtMotoristaCPFouCNPJ.Mask = "000,000,000-00";
            }
            else
            {
                txtMotoristaCPFouCNPJ.Mask = "00,000,000/0000-00";
            }
        }

        private void dgvMotoristas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMotoristas.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosMotorista(dgvMotoristas.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        private void dgvMotoristas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMotoristas.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosMotorista(dgvMotoristas.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        private void btnMotoristaRelatorio_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Action<ExcelWrapper> fncGeraHeaderExcel = (objExcel) =>
            {
                using (var context = new CadastroMotoristaContext())
                {
                    BindingSource bi = new BindingSource();
                    dt = context.Motoristas.ToList().ToDataTable();
                    objExcel.GeraHeader(false, dt.Columns.Cast<DataColumn>()
                                                     .Select(x => x.ColumnName)
                                                     .ToArray());
                }
            };

            using (ExcelWrapper ex = new ExcelWrapper("Controle de Motoristas", "Documentos", "documentos"))
            {
                fncGeraHeaderExcel(ex);
                foreach (DataRow row in dt.Rows)
                {
                    uint colcnt = 0;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        ex.AdicionaConteudo(row[dc].ToString(), colcnt++);
                    }
                    ex.IncrementaLinha();
                }
                ex.AjustaHeader();
                MemoryStream ms = new MemoryStream();
                ex.WriteToMemory(ms);
                string varq = MemoryStreamParaArquivo(ms, ".xlsx", true);
                if (varq != string.Empty)
                {
                    System.Diagnostics.Process.Start(varq);
                }
            }
        }

        #endregion

        #region Documento

        private void LimparDocumento()
        {
            btnDocumentoSelecionar.Enabled = true;
            btnDocumentoDownload.Enabled = false;
            btnDocumentoIncluir.Enabled = true;
            btnDocumentoExcluir.Enabled = false;
            cbxAprovacaoMotoristaID.Enabled = true;

            txtDocumentoDocumentoID.Text = String.Empty;
            txtDocumentoArquivo.Text = String.Empty;
            rdbDocumentoAprovado.Checked = false;
            rdbDocumentoReprovado.Checked = false;
            cbxDocumentoUsuarioID.SelectedIndex = -1;
            cbxDocumentoMotoristaID.SelectedIndex = -1;

            CarregarGridDocumento();

            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Usuarios.ToList();
                cbxDocumentoUsuarioID.DataSource = bi;
                cbxDocumentoUsuarioID.ValueMember = "UsuarioID";
                cbxDocumentoUsuarioID.DisplayMember = "Nome";
                cbxDocumentoUsuarioID.Refresh();

                BindingSource bi2 = new BindingSource();
                bi2.DataSource = context.Motoristas.ToList();
                cbxDocumentoMotoristaID.DataSource = bi2;
                cbxDocumentoMotoristaID.ValueMember = "MotoristaID";
                cbxDocumentoMotoristaID.DisplayMember = "Nome";
                cbxDocumentoMotoristaID.Refresh();
            }
            cbxDocumentoUsuarioID.SelectedValue = usuarioLogin.UsuarioID;
            cbxDocumentoUsuarioID.Refresh();

            cbxDocumentoMotoristaID.Focus();
        }

        private void CarregarGridDocumento()
        {
            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Documentos.Select(o => new ModelViewDocumento()
                {
                    DocumentoID = o.DocumentoID,
                    Arquivo = o.nomeArquivo + o.extensaoArquivo,
                    aprovado = o.aprovado,
                    reprovado = o.reprovado,
                    MotoristaNome = o.Motorista.Nome,
                    UsuarioNome = o.Usuario.Nome
                }).ToList();
                dgvDocumentos.DataSource = bi;
                dgvDocumentos.Refresh();
            }
            txtDocumentoNumeroRegistros.Text = dgvDocumentos.RowCount.ToString();
        }

        private void CarregarDadosDocumento(Int32 ID)
        {
            using (var context = new CadastroMotoristaContext())
            {
                var documento = context.Documentos
                                     .Where(a => a.DocumentoID == ID)
                                     .Include(a => a.Motorista)
                                     .Include(a => a.Usuario)
                                     .FirstOrDefault<Documento>();
                if (documento != null)
                {
                    txtDocumentoDocumentoID.Text = documento.DocumentoID.ToString();
                    rdbDocumentoAprovado.Checked = documento.aprovado;
                    rdbDocumentoReprovado.Checked = documento.reprovado;
                    if (documento.Usuario != null)
                    {
                        cbxDocumentoUsuarioID.SelectedValue = documento.Usuario.UsuarioID;
                    }

                    if (documento.Motorista != null)
                    {
                        cbxDocumentoMotoristaID.SelectedValue = documento.Motorista.MotoristaID;
                    }

                    txtDocumentoArquivo.Text = String.Format("{0}{1}", documento.nomeArquivo, documento.extensaoArquivo);

                    cbxAprovacaoMotoristaID.Enabled = false;
                    btnDocumentoIncluir.Enabled = false;
                    btnDocumentoExcluir.Enabled = true;
                    btnDocumentoSelecionar.Enabled = false;
                    btnDocumentoDownload.Enabled = true;
                }
            }
        }

        private void tabDocumentos_Enter(object sender, EventArgs e)
        {
            LimparDocumento();
        }

        private void BtnDocumentoIncluir_Click(object sender, EventArgs e)
        {

            Int32 numeroProtocolo;

            using (var context = new CadastroMotoristaContext())
            {

                Int32 usuarioID = cbxDocumentoUsuarioID.SelectedValue.ConvertToInt();
                Int32 motoristaID = cbxDocumentoMotoristaID.SelectedValue.ConvertToInt();

              

                var usuario = context.Usuarios
                                  .Where(a => a.UsuarioID == usuarioID).FirstOrDefault<Usuario>();

                var motorista = context.Motoristas
                                  .Where(a => a.MotoristaID == motoristaID).FirstOrDefault<Motorista>();

                if (motorista == null)
                {
                    MessageBox.Show("Nenhum motorista foi selecionado.", "Erro");
                    return;
                }

                byte[] file;
                if (File.Exists(txtDocumentoArquivo.Text))
                {
                    using (var stream = new FileStream(txtDocumentoArquivo.Text, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }
                }
                else
                {
                    //file = new byte[0];
                    MessageBox.Show("Nenhum arquivo foi selecionado.", "Erro");
                    return;
                }

                Documento documento = new Documento
                {
                    aprovado = rdbDocumentoAprovado.Checked,
                    reprovado = rdbDocumentoReprovado.Checked,
                    Usuario = usuario,
                    Motorista = motorista,
                    dados = file,
                    nomeArquivo = Path.GetFileNameWithoutExtension(txtDocumentoArquivo.Text),
                    extensaoArquivo = Path.GetExtension(txtDocumentoArquivo.Text)
                };


                Protocolo protocolo = new Protocolo()
                {
                    Documento = documento,
                    dados = new byte[0]
                };

                context.Documentos.Add(documento);
                context.Protocolos.Add(protocolo);
                context.SaveChanges();
                numeroProtocolo = protocolo.ProtocoloID;

                var protUpdate = context.Protocolos
                                .Where(a => a.ProtocoloID == numeroProtocolo).FirstOrDefault<Protocolo>();

                Wrappers.PdfWrapper pdf = new Wrappers.PdfWrapper();
                pdf.addParagraph("Cadastro de Motoristas");
                pdf.addParagraph(string.Format("Documento nº{0} recebido com sucesso e anexado ao cadastro do(a) motorista {1}.", documento.DocumentoID, documento.Motorista.Nome));
                pdf.addParagraph(string.Format("Nº do protocolo: {0}.", numeroProtocolo));
                protUpdate.dados = pdf.gerarPDF();

                context.SaveChanges();

                if (MessageBox.Show(string.Format("Documento recebido com sucesso. Foi gerado o protocolo nº {0}, deseja visualiza-lo?", numeroProtocolo), "Sucesso", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Int32 documentoID = txtDocumentoDocumentoID.Text.ConvertToInt();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(protUpdate.dados, 0, protUpdate.dados.Length);
                        string varq = MemoryStreamParaArquivo(memoryStream, ".pdf", true);
                        if (varq != string.Empty)
                        {
                            System.Diagnostics.Process.Start(varq);
                        }
                    }
                }
            }
            LimparDocumento();
        }

        private void btnDocumentoExcluir_Click(object sender, EventArgs e)
        {
            using (var context = new CadastroMotoristaContext())
            {
                try
                {
                    var documentoID = txtDocumentoDocumentoID.Text.ConvertToInt();
                    Documento documento = context.Documentos
                                    .Where(a => a.DocumentoID == documentoID).FirstOrDefault<Documento>();
                    context.Entry(documento).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is System.Data.Entity.Core.UpdateException)
                    {
                        if (ex.InnerException.InnerException != null && ex.InnerException.InnerException is System.Data.SqlClient.SqlException)
                        {
                            MessageBox.Show("Não é possível excluir esse registro, pois ele têm dependência com outro registro incluído no banco de dados.", "Erro");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            LimparDocumento();
        }

        private void btnDocumentoLimpar_Click(object sender, EventArgs e)
        {
            LimparDocumento();
        }

        private void btnDocumentoSelecionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Documentos (*.txt;*.pdf;*.doc;*.docx;*.odt;*.jpg;*.jpeg;*.png)|*.txt;*.pdf;*.doc;*.docx;*.odt;*.jpg;*.jpeg;*.png|" + "Todos os Arquivos (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false,
                ShowReadOnly = true,
                ReadOnlyChecked = true,
                CheckFileExists = true,
                CheckPathExists = true
            };
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtDocumentoArquivo.Text = openFileDialog1.FileName;
            }
        }

        private void dgvDocumentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDocumentos.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosDocumento(dgvDocumentos.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        private void dgvDocumentos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDocumentos.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosDocumento(dgvDocumentos.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        private void btnDocumentoDownload_Click(object sender, EventArgs e)
        {
            Int32 documentoID = txtDocumentoDocumentoID.Text.ConvertToInt();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var context = new CadastroMotoristaContext())
                {
                    var documento = context.Documentos
                                         .Where(a => a.DocumentoID == documentoID)
                                         .FirstOrDefault<Documento>();
                    if (documento != null)
                    {
                        memoryStream.Write(documento.dados, 0, documento.dados.Length);
                        MemoryStreamParaArquivo(memoryStream, documento.nomeArquivo + documento.extensaoArquivo, false);
                    }
                }
            }
        }

        private void btnDocumentoAbrir_Click(object sender, EventArgs e)
        {
            Int32 documentoID = txtDocumentoDocumentoID.Text.ConvertToInt();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var context = new CadastroMotoristaContext())
                {
                    var documento = context.Documentos
                                         .Where(a => a.DocumentoID == documentoID)
                                         .FirstOrDefault<Documento>();
                    if (documento != null)
                    {
                        memoryStream.Write(documento.dados, 0, documento.dados.Length);
                        string varq = MemoryStreamParaArquivo(memoryStream, documento.nomeArquivo + documento.extensaoArquivo, true);
                        if (varq != string.Empty)
                        {
                            System.Diagnostics.Process.Start(varq);
                        }
                    }
                }
            }
        }

        private void btnDocumentosAbrirProtocolo_Click(object sender, EventArgs e)
        {
            Int32 documentoID = txtDocumentoDocumentoID.Text.ConvertToInt();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var context = new CadastroMotoristaContext())
                {
                    var protocolo = context.Protocolos
                                         .Where(a => a.Documento.DocumentoID == documentoID)
                                         .FirstOrDefault<Protocolo>();
                    if (protocolo != null)
                    {
                        memoryStream.Write(protocolo.dados, 0, protocolo.dados.Length);
                        string varq = MemoryStreamParaArquivo(memoryStream, ".pdf", true);
                        if (varq != string.Empty)
                        {
                            System.Diagnostics.Process.Start(varq);
                        }
                    }
                }
            }
        }

        private void btnDocumentoRelatorio_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Action<ExcelWrapper> fncGeraHeaderExcel = (objExcel) =>
            {
                using (var context = new CadastroMotoristaContext())
                {
                    BindingSource bi = new BindingSource();
                    dt = context.Documentos.ToList().ToDataTable();
                    objExcel.GeraHeader(false, dt.Columns.Cast<DataColumn>()
                                                     .Select(x => x.ColumnName)
                                                     .ToArray());
                }
            };

            using (ExcelWrapper ex = new ExcelWrapper("Controle de Motoristas", "Documentos", "documentos"))
            {
                fncGeraHeaderExcel(ex);
                foreach (DataRow row in dt.Rows)
                {
                    uint colcnt = 0;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        ex.AdicionaConteudo(row[dc].ToString(), colcnt++);
                    }
                    ex.IncrementaLinha();
                }
                ex.AjustaHeader();
                MemoryStream ms = new MemoryStream();
                ex.WriteToMemory(ms);
                string varq = MemoryStreamParaArquivo(ms, ".xlsx", true);
                if (varq != string.Empty)
                {
                    System.Diagnostics.Process.Start(varq);
                }
            }
        }


        #endregion

        #region Aprovacao

        private void LimparAprovacao()
        {
            btnAprovacaoAprovar.Enabled = false;
            btnAprovacaoReprovar.Enabled = false;
            btnAprovacaoAbrir.Enabled = false;
            txtAprovacaoMensagem.Enabled = false;

            rdbAprovacaoAprovado.Enabled = false;
            rdbAprovacaoReprovado.Enabled = false;

            txtAprovacaoDocumentoID.Enabled = false;
            cbxAprovacaoMotoristaID.Enabled = false;
            cbxAprovacaoUsuarioID.Enabled = false;

            txtAprovacaoArquivo.Text = String.Empty;
            txtAprovacaoMensagem.Text = String.Empty;
            rdbAprovacaoAprovado.Checked = false;
            rdbAprovacaoReprovado.Checked = false;
            cbxAprovacaoMotoristaID.SelectedIndex = -1;
            cbxAprovacaoUsuarioID.SelectedIndex = -1;

            CarregarGridAprovacao();

            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Usuarios.ToList();
                cbxAprovacaoUsuarioID.DataSource = bi;
                cbxAprovacaoUsuarioID.ValueMember = "UsuarioID";
                cbxAprovacaoUsuarioID.DisplayMember = "Nome";
                cbxAprovacaoUsuarioID.Refresh();

                BindingSource bi2 = new BindingSource();
                bi2.DataSource = context.Motoristas.ToList();
                cbxAprovacaoMotoristaID.DataSource = bi2;
                cbxAprovacaoMotoristaID.ValueMember = "MotoristaID";
                cbxAprovacaoMotoristaID.DisplayMember = "Nome";
                cbxAprovacaoMotoristaID.Refresh();
            }

            dgvAprovacao.Focus();
        }

        private void CarregarGridAprovacao()
        {
            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Documentos.Select(o => new ModelViewDocumento()
                {
                    DocumentoID = o.DocumentoID,
                    Arquivo = o.nomeArquivo + o.extensaoArquivo,
                    aprovado = o.aprovado,
                    reprovado = o.reprovado,
                    MotoristaNome = o.Motorista.Nome,
                    UsuarioNome = o.Usuario.Nome
                })
                                                            .Where(a => a.aprovado == false & a.reprovado == false)
                                                            .ToList();
                dgvAprovacao.DataSource = bi;
                dgvAprovacao.Refresh();
            }
            txtAprovacaoNumeroRegistros.Text = dgvAprovacao.RowCount.ToString();
        }

        private void CarregarDadosAprovacao(Int32 ID)
        {
            using (var context = new CadastroMotoristaContext())
            {
                var documento = context.Documentos
                                     .Where(a => a.DocumentoID == ID)
                                     .Include(a => a.Motorista)
                                     .Include(a => a.Usuario)
                                     .FirstOrDefault<Documento>();
                if (documento != null)
                {
                    txtAprovacaoDocumentoID.Text = documento.DocumentoID.ToString();
                    rdbDocumentoAprovado.Checked = documento.aprovado;
                    rdbDocumentoReprovado.Checked = documento.reprovado;
                    if (documento.Usuario != null)
                    {
                        cbxAprovacaoUsuarioID.SelectedValue = documento.Usuario.UsuarioID;
                    }

                    if (documento.Motorista != null)
                    {
                        cbxAprovacaoMotoristaID.SelectedValue = documento.Motorista.MotoristaID;
                    }

                    txtAprovacaoArquivo.Text = String.Format("{0}{1}", documento.nomeArquivo, documento.extensaoArquivo);

                    btnAprovacaoAbrir.Enabled = true;
                    btnAprovacaoAprovar.Enabled = true;
                    btnAprovacaoReprovar.Enabled = true;
                    txtAprovacaoMensagem.Enabled = true;
                    txtAprovacaoMensagem.Focus();
                }
            }
        }

        private void tabAprovacaoDocumentos_Enter(object sender, EventArgs e)
        {
            LimparAprovacao();

        }

        private void btnAprovacaoAbrir_Click(object sender, EventArgs e)
        {
            Int32 documentoID = txtAprovacaoDocumentoID.Text.ConvertToInt();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var context = new CadastroMotoristaContext())
                {
                    var documento = context.Documentos
                                         .Where(a => a.DocumentoID == documentoID)
                                         .FirstOrDefault<Documento>();
                    if (documento != null)
                    {
                        memoryStream.Write(documento.dados, 0, documento.dados.Length);
                        string varq = MemoryStreamParaArquivo(memoryStream, documento.nomeArquivo + documento.extensaoArquivo, true);
                        if (varq != string.Empty)
                        {
                            System.Diagnostics.Process.Start(varq);
                        }
                    }
                }
            }
        }

        private void btnAprovacaoAprovar_Click(object sender, EventArgs e)
        {
            using (var context = new CadastroMotoristaContext())
            {

                Int32 usuarioID = cbxDocumentoUsuarioID.SelectedValue.ConvertToInt();
                Int32 documentoID = txtAprovacaoDocumentoID.Text.ConvertToInt();

                Documento documento = context.Documentos
                               .Include(a => a.Usuario)
                               .Where(a => a.DocumentoID == documentoID).FirstOrDefault<Documento>();

                documento.aprovado = true;
                documento.reprovado = false;

                if (txtAprovacaoMensagem.Text.Trim() != String.Empty)
                {

                    Mensagem mensagem = new Mensagem
                    {
                        Usuario = documento.Usuario,
                        DestinoUsuarioID = usuarioID,
                        TextoMensagem = txtAprovacaoMensagem.Text
                    };
                    context.Mensagens.Add(mensagem);
                }

                context.SaveChanges();
            }
            CarregarGridAprovacao();
        }

        private void btnAprovacaoReprovar_Click(object sender, EventArgs e)
        {
            using (var context = new CadastroMotoristaContext())
            {

                Int32 usuarioID = usuarioLogin.UsuarioID;
                Int32 documentoID = txtAprovacaoDocumentoID.Text.ConvertToInt();

                Documento documento = context.Documentos
                               .Include(a => a.Usuario)
                               .Where(a => a.DocumentoID == documentoID).FirstOrDefault<Documento>();

                Usuario usuario = context.Usuarios
                                 .Where(a => a.UsuarioID == usuarioID).FirstOrDefault<Usuario>();


                documento.aprovado = false;
                documento.reprovado = true;

                if (txtAprovacaoMensagem.Text.Trim() != String.Empty)
                {
                    Mensagem mensagem = new Mensagem
                    {
                        Usuario = usuario,
                        DestinoUsuarioID = documento.Usuario.UsuarioID,
                        TextoMensagem = txtAprovacaoMensagem.Text
                    };
                    context.Mensagens.Add(mensagem);
                }

                context.SaveChanges();
            }
            CarregarGridAprovacao();
        }

        private void dgvAprovacao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAprovacao.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosAprovacao(dgvAprovacao.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        private void dgvAprovacao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAprovacao.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosAprovacao(dgvAprovacao.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        #endregion

        #region Mensagem

        private void LimparMensagem()
        {
            btnMensagemExcluir.Enabled = false;
            cbxMensagemRemetenteID.Enabled = false;
            cbxMensagemDestinatarioID.Enabled = false;

            txtAprovacaoDocumentoID.Enabled = false;
            txtMensagemTextoMensagem.Enabled = false;

            txtMensagemMensagemID.Text = String.Empty;
            txtMensagemTextoMensagem.Text = String.Empty;
            cbxMensagemRemetenteID.SelectedIndex = -1;
            cbxMensagemDestinatarioID.SelectedIndex = -1;

            CarregarGridMensagem();

            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Usuarios.ToList();
                cbxMensagemRemetenteID.DataSource = bi;
                cbxMensagemRemetenteID.ValueMember = "UsuarioID";
                cbxMensagemRemetenteID.DisplayMember = "Nome";
                cbxMensagemRemetenteID.Refresh();

                BindingSource bi2 = new BindingSource();
                bi2.DataSource = context.Usuarios.ToList();
                cbxMensagemDestinatarioID.DataSource = bi2;
                cbxMensagemDestinatarioID.ValueMember = "UsuarioID";
                cbxMensagemDestinatarioID.DisplayMember = "Nome";
                cbxMensagemDestinatarioID.Refresh();
            }

            dgvAprovacao.Focus();
        }

        private void CarregarGridMensagem()
        {
            using (var context = new CadastroMotoristaContext())
            {
                BindingSource bi = new BindingSource();
                bi.DataSource = context.Mensagens
                                        .Where(a => a.Usuario.UsuarioID == usuarioLogin.UsuarioID | a.DestinoUsuarioID == usuarioLogin.UsuarioID)
                                        .OrderByDescending(a => a.MensagemID)
                                        .ToList();
                
                //.ThenByDescending(a => a.visualisado)

                dgvMensagens.DataSource = bi;
                dgvMensagens.Refresh();
            }
            txtMensagensNumeroRegistros.Text = dgvMensagens.RowCount.ToString();
        }

        private void CarregarDadosMensagem(Int32 ID)
        {
            using (var context = new CadastroMotoristaContext())
            {
                var mensagem = context.Mensagens
                                     .Where(a => a.MensagemID == ID)
                                     .Include(a => a.Usuario)
                                     .FirstOrDefault<Mensagem>();
                if (mensagem != null)
                {
                    txtMensagemMensagemID.Text = mensagem.MensagemID.ToString();
                    if (mensagem.Usuario != null)
                    {
                        cbxMensagemRemetenteID.SelectedValue = mensagem.Usuario.UsuarioID;
                    }

                    if (mensagem.DestinoUsuarioID != 0)
                    {
                        cbxMensagemDestinatarioID.SelectedValue = mensagem.DestinoUsuarioID;
                    }

                    txtMensagemTextoMensagem.Text = mensagem.TextoMensagem;

                    mensagem.visualisado = true;
                    context.SaveChanges();

                    btnMensagemExcluir.Enabled = true;
                }
            }
            CarregarGridMensagem();
        }

        private void tabMensagens_Enter(object sender, EventArgs e)
        {
            LimparMensagem();

        }

        private void btnMensagemExcluir_Click(object sender, EventArgs e)
        {
            using (var context = new CadastroMotoristaContext())
            {
                try
                {
                    var mensagemID = txtMensagemMensagemID.Text.ConvertToInt();
                    Mensagem mensagem = context.Mensagens
                                    .Where(a => a.MensagemID == mensagemID).FirstOrDefault<Mensagem>();
                    context.Entry(mensagem).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is System.Data.Entity.Core.UpdateException)
                    {
                        if (ex.InnerException.InnerException != null && ex.InnerException.InnerException is System.Data.SqlClient.SqlException)
                        {
                            MessageBox.Show("Não é possível excluir esse registro, pois ele têm dependência com outro registro incluído no banco de dados.", "Erro");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            LimparMensagem();
        }

        private void dgvMensagens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMensagens.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosMensagem(dgvMensagens.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        private void dgvMensagens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMensagens.RowCount > 0 && e.RowIndex > -1)
            {
                CarregarDadosMensagem(dgvMensagens.CurrentRow.Cells[0].Value.ConvertToInt());
            }
        }

        #endregion

    }
}
