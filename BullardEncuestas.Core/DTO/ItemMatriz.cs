using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.DTO
{
    public class ItemMatriz
    {
        public int IdItemColumn { get; set; }
        public int IdItemRow { get; set; }
        public string NombreItemColumn { get; set; }
        public string NombreItemRow { get; set; }
        public decimal ValorItem { get; set; }
    }
}
