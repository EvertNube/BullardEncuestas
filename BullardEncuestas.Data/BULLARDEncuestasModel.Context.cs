﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BullardEncuestas.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BULLARDEncuestasEntities : DbContext
    {
        public BULLARDEncuestasEntities()
            : base("name=BULLARDEncuestasEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Encuesta> Encuesta { get; set; }
        public virtual DbSet<EncuestaEvaluador> EncuestaEvaluador { get; set; }
        public virtual DbSet<GrupoTrabajo> GrupoTrabajo { get; set; }
        public virtual DbSet<OpcionesRespuesta> OpcionesRespuesta { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<Respuestas> Respuestas { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Seccion> Seccion { get; set; }
        public virtual DbSet<TipoRespuesta> TipoRespuesta { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    
        public virtual ObjectResult<Nullable<int>> SP_EsSocio(Nullable<int> idPersona)
        {
            var idPersonaParameter = idPersona.HasValue ?
                new ObjectParameter("IdPersona", idPersona) :
                new ObjectParameter("IdPersona", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_EsSocio", idPersonaParameter);
        }
    
        public virtual ObjectResult<SP_GetEncuestas_Result> SP_GetEncuestas()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetEncuestas_Result>("SP_GetEncuestas");
        }
    
        public virtual ObjectResult<SP_GetEncuestas2_Result> SP_GetEncuestas2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetEncuestas2_Result>("SP_GetEncuestas2");
        }
    
        public virtual ObjectResult<SP_GetEncuestasReporte_Result> SP_GetEncuestasReporte()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetEncuestasReporte_Result>("SP_GetEncuestasReporte");
        }
    
        public virtual ObjectResult<SP_GetEncuestasReporteDetalle2_Result> SP_GetEncuestasReporteDetalle2(Nullable<int> idEncuesta, Nullable<int> idPeriodo, Nullable<int> idGrupoEvaluado)
        {
            var idEncuestaParameter = idEncuesta.HasValue ?
                new ObjectParameter("IdEncuesta", idEncuesta) :
                new ObjectParameter("IdEncuesta", typeof(int));
    
            var idPeriodoParameter = idPeriodo.HasValue ?
                new ObjectParameter("IdPeriodo", idPeriodo) :
                new ObjectParameter("IdPeriodo", typeof(int));
    
            var idGrupoEvaluadoParameter = idGrupoEvaluado.HasValue ?
                new ObjectParameter("IdGrupoEvaluado", idGrupoEvaluado) :
                new ObjectParameter("IdGrupoEvaluado", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetEncuestasReporteDetalle2_Result>("SP_GetEncuestasReporteDetalle2", idEncuestaParameter, idPeriodoParameter, idGrupoEvaluadoParameter);
        }
    
        public virtual ObjectResult<SP_GetPersonasEnEncuesta_Result> SP_GetPersonasEnEncuesta(Nullable<int> idEncuesta, Nullable<int> idPeriodo, Nullable<int> idGrupoEvaluado)
        {
            var idEncuestaParameter = idEncuesta.HasValue ?
                new ObjectParameter("IdEncuesta", idEncuesta) :
                new ObjectParameter("IdEncuesta", typeof(int));
    
            var idPeriodoParameter = idPeriodo.HasValue ?
                new ObjectParameter("IdPeriodo", idPeriodo) :
                new ObjectParameter("IdPeriodo", typeof(int));
    
            var idGrupoEvaluadoParameter = idGrupoEvaluado.HasValue ?
                new ObjectParameter("IdGrupoEvaluado", idGrupoEvaluado) :
                new ObjectParameter("IdGrupoEvaluado", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetPersonasEnEncuesta_Result>("SP_GetPersonasEnEncuesta", idEncuestaParameter, idPeriodoParameter, idGrupoEvaluadoParameter);
        }
    
        public virtual ObjectResult<SP_GetPersonasPorGrupo_Result> SP_GetPersonasPorGrupo(Nullable<int> idGrupoTrabajo)
        {
            var idGrupoTrabajoParameter = idGrupoTrabajo.HasValue ?
                new ObjectParameter("IdGrupoTrabajo", idGrupoTrabajo) :
                new ObjectParameter("IdGrupoTrabajo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetPersonasPorGrupo_Result>("SP_GetPersonasPorGrupo", idGrupoTrabajoParameter);
        }
    
        public virtual ObjectResult<SP_GetPreguntasEnEncuesta_Result> SP_GetPreguntasEnEncuesta(Nullable<int> idEncuesta, Nullable<int> idPeriodo, Nullable<int> idGrupoEvaluado)
        {
            var idEncuestaParameter = idEncuesta.HasValue ?
                new ObjectParameter("IdEncuesta", idEncuesta) :
                new ObjectParameter("IdEncuesta", typeof(int));
    
            var idPeriodoParameter = idPeriodo.HasValue ?
                new ObjectParameter("IdPeriodo", idPeriodo) :
                new ObjectParameter("IdPeriodo", typeof(int));
    
            var idGrupoEvaluadoParameter = idGrupoEvaluado.HasValue ?
                new ObjectParameter("IdGrupoEvaluado", idGrupoEvaluado) :
                new ObjectParameter("IdGrupoEvaluado", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetPreguntasEnEncuesta_Result>("SP_GetPreguntasEnEncuesta", idEncuestaParameter, idPeriodoParameter, idGrupoEvaluadoParameter);
        }
    
        public virtual int SP_ReplicaEncuesta(Nullable<int> idPeriodo, Nullable<int> idGrupo)
        {
            var idPeriodoParameter = idPeriodo.HasValue ?
                new ObjectParameter("IdPeriodo", idPeriodo) :
                new ObjectParameter("IdPeriodo", typeof(int));
    
            var idGrupoParameter = idGrupo.HasValue ?
                new ObjectParameter("IdGrupo", idGrupo) :
                new ObjectParameter("IdGrupo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ReplicaEncuesta", idPeriodoParameter, idGrupoParameter);
        }
    }
}
