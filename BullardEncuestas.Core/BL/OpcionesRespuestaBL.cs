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
    public class OpcionesRespuestaBL : Base
    {
        public IList<TipoRespuestaDTO> getOpcionesRespuesta(int id, bool activeOnly = false)
        {
            using (var context = getContext())
            {
                var result = !activeOnly ? context.OpcionesRespuesta.Where(r => r.IdTipoRespuesta == id)
                    .Select(r => new OpcionesRespuestaDTO { IdOpcion = r.IdOpcion, IdTipoRespuesta = r.IdTipoRespuesta, Nombre = r.Nombre, Valor = r.Valor })
                    : context.OpcionesRespuesta.Where(r => r.IdTipoRespuesta == id && r.Estado == true)
                    .Select(r => new OpcionesRespuestaDTO { IdOpcion = r.IdOpcion, IdTipoRespuesta = r.IdTipoRespuesta, Nombre = r.Nombre, Valor = r.Valor });
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
