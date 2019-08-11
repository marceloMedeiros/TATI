using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace TATI.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        public Int32 UsuarioID { set; get; }
        public string Login { set; get; }
        public string Nome { set; get; }
        public string Senha { set; get; }
        public bool Administrador { set; get; }
        public ICollection<Documento> Documentos { get; set; }
        public ICollection<Mensagem> Mensagens { get; set; }
    }
}
