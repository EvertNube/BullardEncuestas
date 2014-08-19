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
        protected BULLARDEncuestasEntities getContext()
        {
            return new BULLARDEncuestasEntities();
        }
        protected string getHost()
        {
            Uri uri = System.Web.HttpContext.Current.Request.Url;
            string host = uri.GetLeftPart(UriPartial.Authority);
            return host;
        }
    }
}
