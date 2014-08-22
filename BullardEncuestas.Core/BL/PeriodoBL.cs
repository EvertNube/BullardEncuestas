using BullardEncuestas.Core.DTO;
using BullardEncuestas.Data;
using BullardEncuestas.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.BL
{
    public class PeriodoBL : Base
    {
        public IList<PeriodoDTO> getPeriodos(bool activeOnly = false)
        {
            using (var context = getContext())
            {
                var result = !activeOnly ? context.Periodo.Select(r => new PeriodoDTO { IdPeriodo = r.IdPeriodo, Descripcion = r.Descripcion, Estado = r.Estado }) : 
                    context.Periodo.Where(r => r.Estado == true).Select(r => new PeriodoDTO { IdPeriodo = r.IdPeriodo, Descripcion = r.Descripcion, Estado = r.Estado });
                return result.ToList();
            }
        }

        //public bool add(SeccionDTO seccionDTO)
        //{
        //    using (var context = getContext())
        //    {
        //        try
        //        {
        //            Seccion seccion = new Seccion();
        //            context.Seccion.Add(seccion);
        //            context.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //            //return false;
        //        }
        //    }
        //}

        //public bool update(SeccionDTO seccionDTO)
        //{
        //    using (var context = getContext())
        //    {
        //        try
        //        {
        //            var seccion = context.Seccion.Where(x => x.IdSeccion == seccionDTO.IdSeccion).SingleOrDefault();
        //            seccion.Nombre = seccionDTO.Nombre;
        //            seccion.Orden = seccionDTO.Orden;
        //            seccion.EsSocio = seccionDTO.EsSocio;
        //            seccion.Estado = seccionDTO.Estado;
        //            context.SaveChanges();
        //            seccionDTO.IdEncuesta = seccion.IdEncuesta;
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            //throw e;
        //            return false;
        //        }
        //    }
        //}

    }
}
