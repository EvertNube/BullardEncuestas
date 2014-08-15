using BullardEncuestas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core
{
    public class Base
    {
        protected SISEncuestasEntities getContext()
        {
            return new SISEncuestasEntities();
        }
    }
}
