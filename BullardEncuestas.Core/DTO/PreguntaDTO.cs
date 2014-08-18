using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class PreguntaDTO
    {
        public int IdPregunta { get; set; }
        public int IdSeccion { get; set; }
        public string Texto { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }
        public List<RespuestasDTO> Respuestas { get; set; }
        public List<TipoRepuestaDTO> TipoRespuestas { get; set; }
    }
}
