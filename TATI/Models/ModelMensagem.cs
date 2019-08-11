using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TATI.Models
{
    [Table("Mensagem")]
    public class Mensagem
    {
        public Int32 MensagemID { set; get; }
        public Int32 DestinoUsuarioID { set; get; }
        public string TextoMensagem { set; get; }
        public bool visualisado { set; get; }
        public Int32 UsuarioID { set; get; }
        public Usuario Usuario { get; set; }

    }
}
