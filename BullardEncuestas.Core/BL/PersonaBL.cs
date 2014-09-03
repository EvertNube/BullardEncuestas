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
    public class PersonaBL : Base
    {
        public List<PersonaDTO> getPersonasPorGrupo(int? idGrupo = null)//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = new List<PersonaDTO>();
                if (idGrupo != null && idGrupo != 0)
                {
                    result = context.Persona.Where(x => x.IdGrupoTrabajo == idGrupo).Select(x => new PersonaDTO
                    {
                        IdPersona = x.IdPersona,
                        Nombre = x.Nombre,
                        Email = x.Email,
                        IdEmpresa = x.IdEmpresa,
                        IdGrupoTrabajo = x.IdGrupoTrabajo,
                        Estado = x.Estado,
                        Empresa = context.Empresa.Where(y => y.IdEmpresa == x.IdEmpresa).Select(w => new EmpresaDTO { IdEmpresa = (int)w.IdEmpresa, Nombre = w.Nombre, Estado = w.Estado }).FirstOrDefault(),
                        GrupoTrabajo = context.GrupoTrabajo.Where(y => y.IdGrupoTrabajo == x.IdGrupoTrabajo).Select(w => new GrupoTrabajoDTO { IdGrupoTrabajo = (int)x.IdGrupoTrabajo, Nombre = x.Nombre, Estado = x.Estado }).FirstOrDefault()
                    }).ToList();
                }
                else
                {
                    result = context.Persona.Select(x => new PersonaDTO
                    {
                        IdPersona = x.IdPersona,
                        Nombre = x.Nombre,
                        Email = x.Email,
                        IdEmpresa = x.IdEmpresa,
                        IdGrupoTrabajo = x.IdGrupoTrabajo,
                        Estado = x.Estado,
                        //Empresa = new EmpresaDTO { IdEmpresa = x.Empresa.IdEmpresa, Nombre = x.Empresa.Nombre, Estado = x.Empresa.Estado },
                        Empresa = context.Empresa.Where(y => y.IdEmpresa == x.IdEmpresa).Select(w => new EmpresaDTO { IdEmpresa = (int)w.IdEmpresa, Nombre = w.Nombre, Estado = w.Estado }).FirstOrDefault(),
                        //GrupoTrabajo = context.GrupoTrabajo.Where(y => y.IdGrupoTrabajo == x.IdGrupoTrabajo).Select(w => new GrupoTrabajoDTO { IdGrupoTrabajo = (int)w.IdGrupoTrabajo, Nombre = w.Nombre, Estado = w.Estado }).FirstOrDefault()
                        GrupoTrabajo = new GrupoTrabajoDTO { IdGrupoTrabajo = x.IdGrupoTrabajo ?? 0, Nombre = x.Nombre, Estado = x.Estado },
                    }).ToList();
                }
                return result;
            }
        }
        public PersonaDTO getPersona(int id)
        {
            using (var context = getContext())
            {
                var result = context.Persona.Where(x => x.IdPersona == id)
                    .Select(r => new PersonaDTO
                    {
                        IdPersona = r.IdPersona,
                        Nombre = r.Nombre,
                        Email = r.Email,
                        IdEmpresa = r.IdEmpresa,
                        IdGrupoTrabajo = r.IdGrupoTrabajo,
                        Estado = r.Estado,
                        Empresa = context.Empresa.Where(y => y.IdEmpresa == r.IdEmpresa).Select(w => new EmpresaDTO { IdEmpresa = (int)r.IdEmpresa, Nombre = r.Nombre, Estado = r.Estado }).FirstOrDefault(),
                        GrupoTrabajo = context.GrupoTrabajo.Where(y => y.IdGrupoTrabajo == r.IdGrupoTrabajo).Select(w => new GrupoTrabajoDTO { IdGrupoTrabajo = (int)r.IdGrupoTrabajo, Nombre = r.Nombre, Estado = r.Estado }).FirstOrDefault()
                    }).SingleOrDefault();
                return result;
            }
        }
        public PersonaDTO getPersonaEvaluador(int id)
        {
            using (var context = getContext())
            {
                var result = context.Persona.Where(x => x.IdPersona == id)
                    .Select(r => new PersonaDTO
                    {
                        IdPersona = r.IdPersona,
                        Nombre = r.Nombre,
                        Email = r.Email,
                        IdEmpresa = (int)r.IdEmpresa,
                        IdGrupoTrabajo = (int)r.IdGrupoTrabajo,
                        Estado = r.Estado,
                        listaEncuestaEvaluador = r.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == id).Select(x => new EncuestaEvaluadorDTO
                        {
                            IdEncuestaEvaluador = x.IdEncuestaEvaluador,
                            IdEncuesta = x.IdEncuesta,
                            IdEvaluador = x.IdEvaluador,
                            CodEvaluador = x.CodEvaluador,
                            EstadoEncuesta = x.EstadoEncuesta
                        }).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }
        public PersonaDTO getPersonaEvaluada(int id)
        {
            using (var context = getContext())
            {
                var result = context.Persona.Where(x => x.IdPersona == id)
                    .Select(r => new PersonaDTO
                    {
                        IdPersona = r.IdPersona,
                        Nombre = r.Nombre,
                        Email = r.Email,
                        IdEmpresa = (int)r.IdEmpresa,
                        IdGrupoTrabajo = (int)r.IdGrupoTrabajo,
                        Estado = r.Estado,
                        listaEncuestaEvaluado = r.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == id).Select(x => new EncuestaEvaluadorDTO
                        {
                            IdEncuestaEvaluador = x.IdEncuestaEvaluador,
                            IdEncuesta = x.IdEncuesta,
                            IdEvaluador = x.IdEvaluador,
                            CodEvaluador = x.CodEvaluador,
                            EstadoEncuesta = x.EstadoEncuesta
                        }).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(PersonaDTO persona)
        {
            using (var context = getContext())
            {
                try
                {
                    Persona nuevo = new Persona();
                    nuevo.Nombre = persona.Nombre;
                    nuevo.Email = persona.Email;
                    if (persona.IdEmpresa == 0)
                    { nuevo.IdEmpresa = null; }
                    else { nuevo.IdEmpresa = persona.IdEmpresa; }
                    if (persona.IdGrupoTrabajo == 0)
                    { nuevo.IdGrupoTrabajo = null; }
                    else { nuevo.IdGrupoTrabajo = persona.IdGrupoTrabajo; }
                    nuevo.Estado = persona.Estado;
                    context.Persona.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(PersonaDTO persona)
        {
            using (var context = getContext())
            {
                try
                {
                    var person = context.Persona.Where(x => x.IdPersona == persona.IdPersona).SingleOrDefault();
                    person.Nombre = persona.Nombre;
                    person.Email = persona.Email;
                    person.IdEmpresa = persona.IdEmpresa;
                    person.IdGrupoTrabajo = persona.IdGrupoTrabajo;
                    person.Estado = persona.Estado;
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

