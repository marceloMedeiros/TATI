using Extensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using TATI.Models;

namespace TATI
{
    public partial class Menu : Form
    {

        Usuario usuario;

        public Menu(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(695, 600);


            if (usuario.Administrador)
            {
                Extensions.MyExtensions.EnableTab(tabAprovacaoDocumentos, true);
            }
            else
            {
                Extensions.MyExtensions.EnableTab(tabAprovacaoDocumentos, false);
            }
        }

        #region Motorista

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
                var motoristaID = txtMotoristaMotoristaID.Text.ConvertToInt();
                Motorista motorista = context.Motoristas
                                .Where(a => a.MotoristaID == motoristaID).FirstOrDefault<Motorista>();
                context.Entry(motorista).State = EntityState.Deleted;
                context.SaveChanges();
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

        private void LimparMotorista()
        {
            rdbCPF.Checked = true;
            btnMotoristaIncluir.Enabled = true;
            btnMotoristaAlterar.Enabled = false;
            btnMotoristaExcluir.Enabled = false;
            txtMotoristaMotoristaID.Text = String.Empty;
            txtMotoristaDataHoraCriacao.Text = String.Empty;
            txtMotoristaCelular.Text = String.Empty;
            txtMotoristaCPFouCNPJ.Text = String.Empty;
            txtMotoristaNome.Text = String.Empty;
            txtMotoristaEmail.Text = String.Empty;
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

        private void tabMotoristas_Enter(object sender, EventArgs e)
        {
            LimparMotorista();
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
                        rdbCNPJ.Checked = true;
                    else
                        rdbCPF.Checked = true;

                    txtMotoristaCPFouCNPJ.Text = motorista.CPFouCNPJ;

                    btnMotoristaIncluir.Enabled = false;
                    btnMotoristaAlterar.Enabled = true;
                    btnMotoristaExcluir.Enabled = true;
                }
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

    }
}
