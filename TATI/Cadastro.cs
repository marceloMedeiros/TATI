using System;
using System.Linq;
using System.Windows.Forms;
using TATI.Models;

namespace TATI
{
    public partial class Cadastro : Form
    {
        public Cadastro()
        {
            InitializeComponent();
        }

        public string usuarioCadastrado = String.Empty;

        private void Login_Load(object sender, EventArgs e)
        {
            lblErro.Visible = false;
            txtSenha.PasswordChar = '*';
            txtNome.Focus();
        }

        private Usuario verificaUsuario(string login)
        {
            using (var context = new CadastroMotoristaContext())
            {
                var usuarios = context.Usuarios
                                     .Where(a => a.Login == login);
                if (usuarios.Count<Usuario>() > 0)
                {
                    return usuarios.Single<Usuario>();
                }
                else
                {
                    return null;
                }
            }
        }

        private void limpar()
        {
            txtNome.Text = String.Empty;
            txtUsuario.Text = String.Empty;
            txtSenha.Text = String.Empty;
            ckbAdministrador.Checked = false;
            lblErro.Visible = false;
            this.Visible = true;
            txtNome.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Cadastrar())
            {
                this.Close();
            }
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            txtUsuario.SelectAll();
        }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            txtSenha.SelectAll();
        }

        private void txtNome_Enter(object sender, EventArgs e)
        {
            txtNome.SelectAll();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Cadastrar())
                {
                    this.Close();
                }
            }
        }

        private bool Cadastrar()
        {
            lblErro.Visible = false;

            if (txtUsuario.Text.Trim() == String.Empty ||
                txtUsuario.Text.Trim() == String.Empty ||
                txtUsuario.Text.Trim() == String.Empty)
            {
                MessageBox.Show("É necessário informar todos os campos.", "Erro");
                return false;
            }


            Usuario usuario = verificaUsuario(txtUsuario.Text);

            if (usuario == null)
            {
                using (var context = new CadastroMotoristaContext())
                {
                    usuario = new Usuario
                    {
                        Nome = txtNome.Text,
                        Login = txtUsuario.Text,
                        Senha = txtSenha.Text,
                        Administrador = ckbAdministrador.Checked
                    };

                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                }
                usuarioCadastrado = txtUsuario.Text;
                return true;
            }
            else
            {
                MessageBox.Show("Usuário informado já está cadastrado no sistema.", "Erro");
                limpar();
                return true;

            }
        }

    }
}
