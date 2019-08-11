using System;
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
            if (usuario.Administrador)
            {
                Extensions.MyExtensions.EnableTab(tabAprovacaoDocumentos, true);
            }
            else
            {
                Extensions.MyExtensions.EnableTab(tabAprovacaoDocumentos, false);
            }
        }

    }
}
