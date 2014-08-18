using BullardEncuestas.Core.DTO;
using BullardEncuestas.Data;
using BullardEncuestas.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.BL
{
    public class EncuestaBL : Base
    {
        public IList<EncuestaDTO> getEncuestas()//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.SP_GetEncuestas().Select(r => new EncuestaDTO
                {
                    IdEncuesta = r.IdEncuesta,
                    NombreEncuesta = r.NombreEncuesta,
                    Periodo = new PeriodoDTO { Descripcion = r.NombrePeriodo }
                    //GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.NombreGrupo }
                }).ToList();
                return result;
            }
        }

        public EncuestaDTO getEncuesta(int id)//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.Encuesta.Where(x => x.IdEncuesta == id)
                    .Select(r => new EncuestaDTO
                    {
                        IdEncuesta = r.IdEncuesta,
                        NombreEncuesta = r.Nombre,
                        IdPeriodo = r.IdPeriodo,
                        IdGrupoTrabajo = r.IdGrupoTrabajo,
                        Estado = r.Estado,
                        Periodo = new PeriodoDTO { Descripcion = r.Periodo.Descripcion },
                        GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.Nombre },
                        Secciones = 
                    }).SingleOrDefault();
                return result;
            }
        }

    }
}
