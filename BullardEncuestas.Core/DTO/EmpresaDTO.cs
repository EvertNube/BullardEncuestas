﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    [Serializable]
    class EmpresaDTO
    {
        public int IdEmpresa { get; set; }
        public string Nombre { get; set; }

        public List<PersonaDTO> listaPersona { get; set; }
    }
}
