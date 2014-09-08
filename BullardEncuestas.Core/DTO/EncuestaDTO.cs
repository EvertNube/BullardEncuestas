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
        public bool EstadoEncuesta { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }

        public decimal? PromedioGeneral { get; set; }
        public PeriodoDTO Periodo { get; set; }
        public GrupoTrabajoDTO GrupoEvaluado { get; set; }
        //Reportes
        public List<ReporteDTO> listaReporteDetalle { get; set; }
        public List<PersonaDTO> listaReportePersonas { get; set; }
        public List<PreguntaDTO> listaReportePreguntas { get; set; }
        //Matriz
        public ItemMatriz[,] matrizReporteDetalle { get; set; }
        //Matriz
        //Reportes
        public List<SeccionDTO> Secciones { get; set; }
        public List<int> GrupoEvaluador { get; set; }
        public string StrGrupoEvaluador { get; set; }
    }
}
