using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class EncuestaEvaluadorDTO
    {
        public int IdEncuestaEvaluador { get; set; }
        public int IdEncuesta { get; set; }
        public int IdEvaluador { get; set; }
        public string CodEvaluador { get; set; }
        public bool EstaCompleto { get; set; }
        //public int IdEvaluado { get; set; }
        public List<RespuestasDTO> Respuestas { get; set; }
        public EncuestaDTO Encuesta { get; set; }
        public List<string> listaRespuestas { get; set; }
        public List<int> listaPreguntas { get; set; }
        public List<int> listaEvaluados { get; set; }
        public List<int> listaIdRespuestas { get; set; }
        public int Accion { get; set; }
        public int IdGrupoEvaluado { get; set; }
        //public string EstadoEncuesta { get; set; }
    }
}
