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
    public class PreguntaBL : Base
    {
        public bool add(PreguntaDTO preguntaDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    Pregunta pregunta = new Pregunta();
                    context.Pregunta.Add(pregunta);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                    //return false;
                }
            }
        }

        public bool update(PreguntaDTO preguntaDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    var pregunta = context.Pregunta.Where(x => x.IdPregunta == preguntaDTO.IdPregunta).SingleOrDefault();
                    pregunta.Texto = preguntaDTO.Texto;
                    pregunta.Descripcion = preguntaDTO.Descripcion;
                    pregunta.Orden = preguntaDTO.OrdenPregunta;
                    pregunta.IdTipoRespuesta = preguntaDTO.IdTipoRespuesta;
                    pregunta.Estado = preguntaDTO.EstadoPregunta;
                    context.SaveChanges();
                    preguntaDTO.IdEncuesta = pregunta.Seccion.IdEncuesta;
                    return true;
                }
                catch (Exception e)
                {
                    //throw e;
                    return false;
                }
            }
        }

    }
}
