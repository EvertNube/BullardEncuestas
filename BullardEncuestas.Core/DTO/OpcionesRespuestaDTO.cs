using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class OpcionesRespuestaDTO
    {
        public int IdOpcion { get; set; }
        public int Valor { get; set; }
        public string Nombre { get; set; }
        public int IdTipoRespuesta { get; set; }
    }
}
