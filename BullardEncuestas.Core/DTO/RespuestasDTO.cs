﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class RespuestasDTO
    {
        public int IdRespuestas { get; set; }
        public int IdEncuestaEvaluador { get; set; }
        public int IdPregunta { get; set; }
        public int IdEvaluado { get; set; }
        public string Valor { get; set; }
    }
}
