using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    [Serializable]
    public class ReporteSeccionValorDTO
    {
        public int IdSeccion { get; set; }
        public int OrdenSeccion { get; set; }
        public int IdPregunta { get; set; }
        public int OrdenPregunta { get; set; }
        public int IdRespuesta { get; set; }
        public int IdTipoRespuesta { get; set; }
        public int IdOpcionRespuesta { get; set; }
        public int Valor { get; set; }
    }
}
