using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TATI.Models
{
    [Table("Protocolo")]
    public class Protocolo
    {
        public Int32 ProtocoloID { set; get; }
        public byte[] dados { set; get; }
        public Int32 DocumentoID { set; get; }
        public Documento Documento { get; set; }
    }
}
