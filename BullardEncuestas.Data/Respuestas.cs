//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Respuestas
    {
        public int IdRespuestas { get; set; }
        public int IdEncuestaEvaluador { get; set; }
        public int IdPregunta { get; set; }
        public int IdEvaluado { get; set; }
        public string Valor { get; set; }
    
        public virtual EncuestaEvaluador EncuestaEvaluador { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}
