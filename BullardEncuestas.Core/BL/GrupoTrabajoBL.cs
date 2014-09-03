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
    public class GrupoTrabajoBL : Base
    {
        public IList<GrupoTrabajoDTO> getGruposEvaluados(bool activeOnly = false)
        {
            using (var context = getContext())
            {
                var result = !activeOnly ? context.GrupoTrabajo.Select(r => new GrupoTrabajoDTO { IdGrupoTrabajo = r.IdGrupoTrabajo, Nombre = r.Nombre, Estado = r.Estado }) :
                    context.GrupoTrabajo.Where(r => r.Estado == true).Select(r => new GrupoTrabajoDTO { IdGrupoTrabajo = r.IdGrupoTrabajo, Nombre = r.Nombre, Estado = r.Estado });
                return result.ToList();
            }
        }

        public GrupoTrabajoDTO getGrupoTrabajo(int id)
        {
            using (var context = getContext())
            {
                var result = context.GrupoTrabajo.Where(r => r.IdGrupoTrabajo == id).Select(r => new GrupoTrabajoDTO
                             {
                                 IdGrupoTrabajo = r.IdGrupoTrabajo,
                                 Nombre = r.Nombre,
                                 Estado = r.Estado,
                                 listaPersona = r.Persona.Select(x => new PersonaDTO { Nombre = x.Nombre }).ToList()
                             });
                return result.SingleOrDefault();
            }
        }
        public bool add(GrupoTrabajoDTO grupoTrabajo)
        {
            using (var context = getContext())
            {
                try
                {
                    GrupoTrabajo nuevo = new GrupoTrabajo();
                    nuevo.Nombre = grupoTrabajo.Nombre;
                    nuevo.Estado = grupoTrabajo.Estado;
                    context.GrupoTrabajo.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool update(GrupoTrabajoDTO grupoTrabajo)
        {
            using (var context = getContext())
            {
                try
                {
                    var grupo = context.GrupoTrabajo.Where(x => x.IdGrupoTrabajo == grupoTrabajo.IdGrupoTrabajo).SingleOrDefault();
                    grupo.Nombre = grupoTrabajo.Nombre;
                    grupo.Estado = grupoTrabajo.Estado;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
