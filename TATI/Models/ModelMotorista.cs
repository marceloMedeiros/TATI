using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace TATI.Models
{
    [Table("Motorista")]
    public class Motorista
    {
        public Int32 MotoristaID { set; get; }
        public string MotoristaNome { set; get; }
        public string Celular { set; get; }
        public string Email { set; get; }
        public string CPFouCNPJ { set; get; }
        public DateTime DataHoraCriacao { set; get; }
        public ICollection<Documento> Documentos { get; set; }
    }
}
