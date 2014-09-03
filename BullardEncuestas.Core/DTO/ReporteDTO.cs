using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    [Serializable]
    public class ReporteDTO
    {
        public int IdEncuesta { get; set; }
        public string NombreEncuesta { get; set; }
        public bool Estado { get; set; }
        public int IdPeriodo { get; set; }
        public string NombrePeriodo { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }

        public decimal? PromedioGeneral { get; set; }
        public List<DetalleEvaluadoDTO> ListaEvaluados { get; set; }
        public List<ReporteSeccionValorDTO> ListaSeccionProm { get; set; }
        public ReporteDTO reporteAnterior { get; set; }
        //s
    }
}
