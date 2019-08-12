using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATI.Models
{
    class ModelViewDocumento
    {
        public Int32 DocumentoID { set; get; }
        public string Arquivo { set; get; }
        public bool aprovado { set; get; }
        public bool reprovado { set; get; }
        public string MotoristaNome { get; set; }
        public string UsuarioNome { get; set; }

        private Documento _obj;

        public ModelViewDocumento(Documento obj)
        {
            _obj = obj;
        }
        public ModelViewDocumento()
        {
        }

        public Documento GetModel()
        {
            return _obj;
        }

    }
}
