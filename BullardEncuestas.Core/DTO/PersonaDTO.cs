using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    [Serializable]
    public class PersonaDTO
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int? IdEmpresa { get; set; }
        //public int? IdGrupoTrabajo { get; set; }
        public bool Estado { get; set; }
        public string RutaImagen { get; set; }
        public decimal Promedio { get; set; }

        public EmpresaDTO Empresa { get; set; }
        public GrupoTrabajoDTO GrupoTrabajo { get; set; }
        public List<EncuestaEvaluadorDTO> listaEncuestaEvaluador { get; set; }
        public List<EncuestaEvaluadorDTO> listaEncuestaEvaluado { get; set; }
        public List<int> ListaGruposTrabajo { get; set; }
        public List<GrupoTrabajoDTO> GruposTrabajo { get; set; }
    }
}
