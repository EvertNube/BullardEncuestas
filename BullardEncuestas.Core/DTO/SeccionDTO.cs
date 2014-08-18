using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class SeccionDTO
    {
        public int IdSeccion { get; set; }
        public string Nombre { get; set; }
        public int IdEncuesta { get; set; }
        public int IdSeccionPadre { get; set; }
        public int Orden { get; set; }
        public List<PreguntaDTO> Preguntas { get; set; }
        public List<SeccionDTO> SubSecciones { get; set; }
    }
}
