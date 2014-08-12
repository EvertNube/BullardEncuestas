using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    class PeriodoDTO
    {
        public int IdPeriodo { get; set; }
        public string Descripcion { get; set; }
        public List<EncuestaDTO> Encuestas { get; set; }

    }
}
