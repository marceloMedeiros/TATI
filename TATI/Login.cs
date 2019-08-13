using System;
using System.Linq;
using System.Windows.Forms;
using TATI.Models;

namespace TATI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            lblErro.Visible = false;
            txtSenha.PasswordChar = '*';
            txtUsuario.Focus();

            using (CadastroMotoristaContext dbContext = new CadastroMotoristaContext())
            {
                if (!dbContext.Database.Exists())
                {
                    MessageBox.Show("Não foi possível conectar ao banco de dados. Verifique se o arquivo TATI.exe.config presente na pasta de execução deste programa contém a string de conexão correta para o banco de dados SQL Server.", "Erro");
                    this.Close();
                }
            }

        }

        private Usuario verificaCredenciais(string login, string senha)
        {
            using (var context = new CadastroMotoristaContext())
            {
                var usuarios = context.Usuarios
                                     .Where(a => a.Login == login & a.Senha == senha);
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
            txtUsuario.Text = String.Empty;
            txtSenha.Text = String.Empty;
            lblErro.Visible = false;
            this.Visible = true;
            txtUsuario.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Logar();
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            txtUsuario.SelectAll();
        }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            txtSenha.SelectAll();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Logar();
            }
        }

        private void Logar()
        {
            lblErro.Visible = false;
            Usuario usuario = verificaCredenciais(txtUsuario.Text, txtSenha.Text);

            if (usuario == null)
            {
                lblErro.Visible = true;
                txtUsuario.Focus();
                txtUsuario.SelectAll();
            }
            else
            {
                lblErro.Visible = false;
                Menu form = new Menu(usuario);
                form.StartPosition = FormStartPosition.CenterScreen;
                this.Visible = false;
                form.ShowDialog();
                limpar();
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            Cadastro form = new Cadastro();
            form.ShowDialog();
            limpar();
            txtUsuario.Text = form.usuarioCadastrado;
        }

    }
}
