using Extensions;
using System;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TATI.Models;

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
            this.Text += " - Usuário ativo: " + usuarioLogin.Nome;

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

        #endregion

        #region Documento

        private void LimparDocumento()
        {
            btnDocumentoSelecionar.Enabled = true;
            btnDocumentoDownload.Enabled = false;
            btnDocumentoIncluir.Enabled = true;
            btnDocumentoExcluir.Enabled = false;

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
                pdf.addParagraph(string.Format("Documento nº{0} recebido com sucesso e anexado ao cadastro do(a) Motorista {1}.", documento.DocumentoID, documento.Motorista.Nome));
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
                Filter = "Documentos (*.txt;*.pdf;*.doc,*.docx,*.odt,*.jpg,*.jpeg,*.png)|*.txt;*.pdf;*.doc,*.docx,*.odt,*.jpg,*.jpeg,*.png|" + "Todos os Arquivos (*.*)|*.*",
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

        private void rdbDocumentoReprovado_EnabledChanged(object sender, EventArgs e)
        {
            rdbDocumentoReprovado.ForeColor = System.Drawing.Color.White;
        }

        private void rdbDocumentoReprovado_Paint(object sender, PaintEventArgs e)
        {
            dynamic radio = (RadioButton)sender;
            dynamic drawBrush = new SolidBrush(radio.ForeColor);
            dynamic sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString(((RadioButton)sender).Text, radio.Font, drawBrush, e.ClipRectangle, sf);
            drawBrush.Dispose();
            sf.Dispose();
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

        #endregion
    }
}
