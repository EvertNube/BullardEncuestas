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
        public int IdTipoRespuesta { get; set; }
        public string Texto { get; set; }
        public string Descripcion { get; set; }
        public int OrdenPregunta { get; set; }
        public bool EstadoPregunta { get; set; }
        
        public int IdEncuesta { get; set; }
        //public List<string> Respuestas { get; set; }
        public TipoRespuestaDTO TipoRespuesta { get; set; }
        public List<RespuestasDTO> Respuestas { get; set; }
    }
}
