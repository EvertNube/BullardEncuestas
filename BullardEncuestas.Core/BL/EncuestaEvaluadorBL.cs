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
    public class EncuestaEvaluadorBL : Base
    {
        public bool add(EncuestaEvaluadorDTO encuestaEvaluadorDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    EncuestaEvaluador encuestaEvaluador = new EncuestaEvaluador();
                    encuestaEvaluador.IdEncuesta = encuestaEvaluadorDTO.IdEncuesta;
                    encuestaEvaluador.IdEvaluador = encuestaEvaluadorDTO.IdEvaluador;
                    encuestaEvaluador.CodEvaluador = "";
                    encuestaEvaluador.EstadoEncuesta = true;
                    context.EncuestaEvaluador.Add(encuestaEvaluador);
                    foreach (var item in encuestaEvaluadorDTO.Respuestas)
                    {
                        Respuestas respuesta = new Respuestas();
                        respuesta.IdEncuestaEvaluador = encuestaEvaluador.IdEncuestaEvaluador;
                        respuesta.IdPregunta = item.IdPregunta;
                        respuesta.IdEvaluado = item.IdEvaluado;
                        respuesta.Valor = item.Valor;
                        encuestaEvaluador.Respuestas.Add(respuesta);
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    //throw e;
                    return false;
                }
            }
        }

        public bool update(EncuestaEvaluadorDTO encuestaEvaluadorDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    var encuestaEvaluador = context.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == encuestaEvaluadorDTO.IdEncuestaEvaluador).SingleOrDefault();
                    context.SaveChanges();
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
