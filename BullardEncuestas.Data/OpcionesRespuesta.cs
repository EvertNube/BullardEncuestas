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
    
    public partial class OpcionesRespuesta
    {
        public int IdOpcion { get; set; }
        public Nullable<int> Valor { get; set; }
        public Nullable<int> IdTipoRespuesta { get; set; }
    
        public virtual TipoRespuesta TipoRespuesta { get; set; }
    }
}
