using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    [Serializable]
    public class DetalleEvaluadoDTO
    {
        public int IdEvaluado { get; set; }
        public string NombreEvaluado { get; set; }
        public int IdSeccion { get; set; }
        public int SeccionOrden { get; set; }
        public int IdPregunta { get; set; }
        public int PreguntaOrden { get; set; }
        public string Pregunta { get; set; }
        public int Valor { get; set; }
    }
}
