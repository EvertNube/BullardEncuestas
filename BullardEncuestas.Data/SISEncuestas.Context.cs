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
    
    public partial class SISEncuestasEntities : DbContext
    {
        public SISEncuestasEntities()
            : base("name=SISEncuestasEntities")
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
        public virtual DbSet<Seccion> Seccion { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TipoRespuesta> TipoRespuesta { get; set; }
    }
}
