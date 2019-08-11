using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TATI.Models
{
    [Table("Documento")]
    public class Documento
    {
        public Int32 DocumentoID { set; get; }
        public string nomeArquivo { set; get; }
        public string extensaoArquivo { set; get; }
        public byte[] dados { set; get; }
        public bool aprovado { set; get; }
        public bool reprovado { set; get; }
        public Motorista Motorista { get; set; }
        public Usuario Usuario { get; set; }

    }
}
