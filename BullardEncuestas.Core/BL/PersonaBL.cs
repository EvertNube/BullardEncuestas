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
                    foreach (var group in personaDTO.ListaGruposTrabajo)
                    {
                        var grupo = context.GrupoTrabajo.Where(x => x.IdGrupoTrabajo == group).SingleOrDefault();
                        persona.GrupoTrabajo.Add(grupo);
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    //throw e;
                }
            }
        }
        public bool update(PersonaDTO personaDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    var persona = context.Persona.Where(x => x.IdPersona == personaDTO.IdPersona).SingleOrDefault();
                    persona.Nombre = personaDTO.Nombre;
                    persona.Email = personaDTO.Email;
                    persona.IdEmpresa = personaDTO.IdEmpresa != 0 ? personaDTO.IdEmpresa : null;
                    persona.Estado = personaDTO.Estado;
                    var oldGrupo = persona.GrupoTrabajo.Select(x => x.IdGrupoTrabajo).ToList();
                    var newGrupo = personaDTO.ListaGruposTrabajo.Select(x => x).ToList();
                    var gruposToRemove = oldGrupo.Except(newGrupo).ToList();
                    var gruposToAdd = newGrupo.Except(oldGrupo).ToList();
                    foreach (var group in gruposToRemove)
                    {
                        var grupo = context.GrupoTrabajo.Where(x => x.IdGrupoTrabajo == group).SingleOrDefault();
                        persona.GrupoTrabajo.Remove(grupo);
                    }
                    foreach (var group in gruposToAdd)
                    {
                        var grupo = context.GrupoTrabajo.Where(x => x.IdGrupoTrabajo == group).SingleOrDefault();
                        persona.GrupoTrabajo.Add(grupo);
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    //throw e;
                }
            }
        }
        public bool esSocio(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    var conta = context.SP_EsSocio(id);
                    return conta == 1 ? true : false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}

