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
        public List<PersonaDTO> getPersonasPorGrupo(int idGrupo)//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.SP_GetPersonasPorGrupo(idGrupo).Select(x => new PersonaDTO
                {
                    IdPersona = x.IdPersona,
                    Nombre = x.Nombre,
                    Email = x.Email,
                    Estado = x.Estado,
                    Empresa = new EmpresaDTO { Nombre = x.NombreEmpresa }
                }).ToList();
                return result;
            }
        }
        public List<PersonaDTO> getPersonas()
        {
            using (var context = getContext())
            {
                var result = context.Persona.Select(x => new PersonaDTO
                    {
                        IdPersona = x.IdPersona,
                        Nombre = x.Nombre,
                        Email = x.Email,
                        Estado = x.Estado,
                        Empresa = new EmpresaDTO { Nombre = x.Empresa.Nombre }
                    }).ToList();
                return result;
            }
        }
        public PersonaDTO getPersona(int id)
        {
            using (var context = getContext())
            {
                var result = context.Persona.Where(r => r.IdPersona == id)
                    .Select(r => new PersonaDTO
                    {
                        IdPersona = r.IdPersona,
                        Nombre = r.Nombre,
                        Email = r.Email,
                        IdEmpresa = r.IdEmpresa,
                        Estado = r.Estado,
                        GruposTrabajo = r.GrupoTrabajo.Select(w => new GrupoTrabajoDTO { IdGrupoTrabajo = w.IdGrupoTrabajo, Nombre = w.Nombre }).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }
        //public PersonaDTO getPersonaEvaluador(int id)
        //{
        //    using (var context = getContext())
        //    {
        //        var result = context.Persona.Where(x => x.IdPersona == id)
        //            .Select(r => new PersonaDTO
        //            {
        //                IdPersona = r.IdPersona,
        //                Nombre = r.Nombre,
        //                Email = r.Email,
        //                IdEmpresa = (int)r.IdEmpresa,
        //                IdGrupoTrabajo = (int)r.IdGrupoTrabajo,
        //                Estado = r.Estado,
        //                listaEncuestaEvaluador = r.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == id).Select(x => new EncuestaEvaluadorDTO
        //                {
        //                    IdEncuestaEvaluador = x.IdEncuestaEvaluador,
        //                    IdEncuesta = x.IdEncuesta,
        //                    IdEvaluador = x.IdEvaluador,
        //                    CodEvaluador = x.CodEvaluador,
        //                    EstadoEncuesta = x.EstadoEncuesta
        //                }).ToList()
        //            }).SingleOrDefault();
        //        return result;
        //    }
        //}
        //public PersonaDTO getPersonaEvaluada(int id)
        //{
        //    using (var context = getContext())
        //    {
        //        var result = context.Persona.Where(x => x.IdPersona == id)
        //            .Select(r => new PersonaDTO
        //            {
        //                IdPersona = r.IdPersona,
        //                Nombre = r.Nombre,
        //                Email = r.Email,
        //                IdEmpresa = (int)r.IdEmpresa,
        //                IdGrupoTrabajo = (int)r.IdGrupoTrabajo,
        //                Estado = r.Estado,
        //                listaEncuestaEvaluado = r.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == id).Select(x => new EncuestaEvaluadorDTO
        //                {
        //                    IdEncuestaEvaluador = x.IdEncuestaEvaluador,
        //                    IdEncuesta = x.IdEncuesta,
        //                    IdEvaluador = x.IdEvaluador,
        //                    CodEvaluador = x.CodEvaluador,
        //                    EstadoEncuesta = x.EstadoEncuesta
        //                }).ToList()
        //            }).SingleOrDefault();
        //        return result;
        //    }
        //}
        public bool add(PersonaDTO personaDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    Persona persona = new Persona();
                    persona.Nombre = personaDTO.Nombre;
                    persona.Email = personaDTO.Email;
                    persona.Estado = personaDTO.Estado;
                    persona.IdEmpresa = personaDTO.IdEmpresa != 0 ? personaDTO.IdEmpresa : null;                    
                    context.Persona.Add(persona);
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
                    //person.Nombre = persona.Nombre;
                    //person.Email = persona.Email;
                    //person.IdEmpresa = persona.IdEmpresa;
                    //person.IdGrupoTrabajo = persona.IdGrupoTrabajo;
                    //person.Estado = persona.Estado;
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

