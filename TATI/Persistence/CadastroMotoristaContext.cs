using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TATI.Models;

namespace TATI
{
    public class CadastroMotoristaContext : DbContext
    {
        public CadastroMotoristaContext()
            : base("CadastroMotorista")
        {

        }

        public CadastroMotoristaContext(string customStr)
                : base(customStr)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Protocolo> Protocolos { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
    }
}
