﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class EncuestaDTO
    {
        public int IdEncuesta { get; set; }
        public string NombreEncuesta { get; set; }
        public int IdPeriodo { get; set; }
        public int? IdGrupoTrabajo { get; set; }
        public bool Estado { get; set; }
        public PeriodoDTO Periodo { get; set; }
        public GrupoTrabajoDTO GrupoTrabajo { get; set; }
        public List<SeccionDTO> Secciones { get; set; }
        public List<EncuestaEvaluadorDTO> listaEncuestaEvaluador { get; set; }
        
    }
}
