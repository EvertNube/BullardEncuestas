using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class EncuestaDTO
    {
        public int IdEncuesta { get; set; }
        public string NombreEncuesta { get; set; }
        public string Instrucciones { get; set; }
        public string Leyenda { get; set; }
        public int IdPeriodo { get; set; }
        public int? IdGrupoEvaluado { get; set; }
        public bool Estado { get; set; }
        public PeriodoDTO Periodo { get; set; }
        public GrupoTrabajoDTO GrupoEvaluado { get; set; }
        public List<SeccionDTO> Secciones { get; set; }
        public List<EncuestaEvaluadorDTO> listaEncuestaEvaluador { get; set; }
    }
}
