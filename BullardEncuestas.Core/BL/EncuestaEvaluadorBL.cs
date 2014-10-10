using BullardEncuestas.Core.DTO;
using BullardEncuestas.Data;
using BullardEncuestas.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullardEncuestas.Core.BL
{
    public class EncuestaEvaluadorBL : Base
    {
        public EncuestaEvaluadorDTO getEncuestaEvaluador(int idEncuesta, int idEvaluador)
        {
            using (var context = getContext())
            {
                var result = context.Encuesta.Where(r => r.IdEncuesta == idEncuesta).AsEnumerable()
                    .Select(r => new EncuestaEvaluadorDTO
                    {
                        IdEncuestaEvaluador = r.EncuestaEvaluador.Where(x => x.IdEvaluador == idEvaluador).Select(x => x.IdEncuestaEvaluador).FirstOrDefault(),
                        IdEncuesta = r.IdEncuesta,
                        IdEvaluador = idEvaluador,
                        EstaCompleto = r.EncuestaEvaluador.Where(x => x.IdEvaluador == idEvaluador).Select(x => x.EstaCompleto).FirstOrDefault(),
                        Encuesta = new EncuestaDTO
                        {
                            IdGrupoEvaluado = r.IdGrupoEvaluado,
                            IdPeriodo = r.IdPeriodo,
                            NombreEncuesta = r.Nombre,
                            Instrucciones = r.Instrucciones,
                            Leyenda = r.Leyenda,
                            FechaInicio = r.FechaInicio,
                            FechaCierre = r.FechaCierre,
                            EstadoEncuesta = r.Estado,
                            EstadoProceso = r.EstadoProceso,
                            Periodo = new PeriodoDTO { Descripcion = r.Periodo.Descripcion },
                            GrupoEvaluado = new GrupoTrabajoDTO { Nombre = r.Nombre },
                            Secciones = r.Seccion.Where(x => x.IdSeccionPadre == null && x.Estado == true).Select(x => new SeccionDTO
                            {
                                IdSeccion = x.IdSeccion,
                                Nombre = x.Nombre,
                                Orden = x.Orden,
                                EsSocio = x.EsSocio,
                                Estado = x.Estado,
                                Preguntas = x.Pregunta.Where(y => y.Estado == true).Select(y => new PreguntaDTO
                                {
                                    IdPregunta = y.IdPregunta,
                                    Texto = y.Texto,
                                    Descripcion = y.Descripcion,
                                    OrdenPregunta = y.Orden,
                                    EstadoPregunta = y.Estado,
                                    IdTipoRespuesta = y.IdTipoRespuesta,
                                    TipoRespuesta = new TipoRespuestaDTO
                                    {
                                        Nombre = y.TipoRespuesta.Nombre,
                                        OpcionesRespuesta = y.TipoRespuesta.OpcionesRespuesta.Select(o => new OpcionesRespuestaDTO
                                        {
                                            IdOpcion = o.IdOpcion,
                                            IdTipoRespuesta = o.IdTipoRespuesta,
                                            Valor = o.Valor,
                                            Nombre = o.Nombre
                                        }).ToList()
                                    },
                                    Respuestas = y.Respuestas.Where(a => a.EncuestaEvaluador.IdEvaluador == idEvaluador).Select(a => new RespuestasDTO { IdRespuestas = a.IdRespuestas, IdEvaluado = a.IdEvaluado, Valor = a.Valor }).ToList()
                                }).OrderBy(y => y.OrdenPregunta).ToList(),
                                SubSecciones = r.Seccion.Where(y => y.IdSeccionPadre == x.IdSeccion && y.Estado == true).Select(y => new SeccionDTO
                                {
                                    IdSeccion = y.IdSeccion,
                                    Nombre = y.Nombre,
                                    Orden = y.Orden,
                                    EsSocio = y.EsSocio,
                                    Estado = y.Estado,
                                    Preguntas = y.Pregunta.Where(z => z.Estado == true).Select(z => new PreguntaDTO
                                    {
                                        IdPregunta = z.IdPregunta,
                                        Texto = z.Texto,
                                        Descripcion = z.Descripcion,
                                        OrdenPregunta = z.Orden,
                                        EstadoPregunta = z.Estado,
                                        IdTipoRespuesta = z.IdTipoRespuesta,
                                        TipoRespuesta = new TipoRespuestaDTO
                                        {
                                            Nombre = z.TipoRespuesta.Nombre,
                                            OpcionesRespuesta = z.TipoRespuesta.OpcionesRespuesta.Select(o => new OpcionesRespuestaDTO
                                            {
                                                IdOpcion = o.IdOpcion,
                                                IdTipoRespuesta = o.IdTipoRespuesta,
                                                Valor = o.Valor,
                                                Nombre = o.Nombre
                                            }).ToList()
                                        },
                                        Respuestas = z.Respuestas.Where(a => a.EncuestaEvaluador.IdEvaluador == idEvaluador).Select(a => new RespuestasDTO { IdRespuestas = a.IdRespuestas, IdEvaluado = a.IdEvaluado, Valor = a.Valor }).ToList()
                                    }).OrderBy(z => z.OrdenPregunta).ToList()
                                }).OrderBy(y => y.Orden).ToList()
                            }).OrderBy(x => x.Orden).ToList()
                        }
                    }).SingleOrDefault();
                return result;
            }
        }

        public int add(EncuestaEvaluadorDTO encuestaEvaluadorDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    //Ordenando las respuestas en encuestaEvaluadorDTO.Respuestas
                    encuestaEvaluadorDTO.Respuestas = new List<RespuestasDTO>();
                    for (int i = 0; i < encuestaEvaluadorDTO.listaRespuestas.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(encuestaEvaluadorDTO.listaRespuestas[i].Trim()) && encuestaEvaluadorDTO.listaRespuestas[i] != "0")
                        {
                            RespuestasDTO respuesta = new RespuestasDTO();
                            respuesta.IdRespuestas = encuestaEvaluadorDTO.listaIdRespuestas[i];
                            respuesta.IdPregunta = encuestaEvaluadorDTO.listaPreguntas[i];
                            respuesta.IdEvaluado = encuestaEvaluadorDTO.listaEvaluados[i];
                            respuesta.Valor = encuestaEvaluadorDTO.listaRespuestas[i];
                            encuestaEvaluadorDTO.Respuestas.Add(respuesta);
                        }
                    }
                    //// convert source data to DataTable
                    DataTable table = encuestaEvaluadorDTO.Respuestas.ToDataTable();
                    //// create parameters
                    var IdEncuesta = new SqlParameter("@IdEncuesta", SqlDbType.Int);
                    IdEncuesta.Value = encuestaEvaluadorDTO.IdEncuesta;
                    var IdEvaluador = new SqlParameter("@IdEvaluador", SqlDbType.Int);
                    IdEvaluador.Value = encuestaEvaluadorDTO.IdEvaluador;
                    var CodEvaluador = new SqlParameter("@CodEvaluador", SqlDbType.NVarChar);
                    CodEvaluador.Value = "";
                    var EstaCompleto = new SqlParameter("@EstaCompleto", SqlDbType.Bit);
                    EstaCompleto.Value = encuestaEvaluadorDTO.EstaCompleto;
                    var tvpRespuestas = new SqlParameter("@tvpRespuestas", table);
                    tvpRespuestas.SqlDbType = SqlDbType.Structured;
                    tvpRespuestas.TypeName = "TVP_Respuestas";
                    var output = new SqlParameter("@IdEncuestaEvaluador", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    //// save using procedure with table value parameter
                    var totalCountParam = new SqlParameter("TotalCount", 0) { Direction = ParameterDirection.Output };
                    context.ExecuteTableValueProcedure("SP_AddRespuestas", "@IdEncuesta,@IdEvaluador,@CodEvaluador,@EstaCompleto,@tvpRespuestas,@IdEncuestaEvaluador OUTPUT", new object[] { IdEncuesta, IdEvaluador, CodEvaluador, EstaCompleto, tvpRespuestas, output });
                    return (int)output.Value;
                    ////Agrego la respuesta
                    //EncuestaEvaluador encuestaEvaluador = new EncuestaEvaluador();
                    //encuestaEvaluador.IdEncuesta = encuestaEvaluadorDTO.IdEncuesta;
                    //encuestaEvaluador.IdEvaluador = encuestaEvaluadorDTO.IdEvaluador;
                    //encuestaEvaluador.CodEvaluador = "";
                    //encuestaEvaluador.EstaCompleto = encuestaEvaluadorDTO.EstaCompleto;
                    //context.EncuestaEvaluador.Add(encuestaEvaluador);
                    //foreach (var item in encuestaEvaluadorDTO.Respuestas)
                    //{
                    //    Respuestas respuesta = new Respuestas();
                    //    respuesta.IdEncuestaEvaluador = encuestaEvaluador.IdEncuestaEvaluador;
                    //    respuesta.IdPregunta = item.IdPregunta;
                    //    respuesta.IdEvaluado = item.IdEvaluado;
                    //    respuesta.Valor = item.Valor;
                    //    encuestaEvaluador.Respuestas.Add(respuesta);
                    //}
                    //context.SaveChanges();
                    //return encuestaEvaluador.IdEncuestaEvaluador;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public bool update(EncuestaEvaluadorDTO encuestaEvaluadorDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    encuestaEvaluadorDTO.Respuestas = new List<RespuestasDTO>();
                    for (int i = 0; i < encuestaEvaluadorDTO.listaRespuestas.Count; i++)
                    {
                        RespuestasDTO respuesta = new RespuestasDTO();
                        respuesta.IdRespuestas = encuestaEvaluadorDTO.listaIdRespuestas[i];
                        respuesta.IdPregunta = encuestaEvaluadorDTO.listaPreguntas[i];
                        respuesta.IdEvaluado = encuestaEvaluadorDTO.listaEvaluados[i];
                        respuesta.Valor = encuestaEvaluadorDTO.listaRespuestas[i];
                        encuestaEvaluadorDTO.Respuestas.Add(respuesta);
                    }
                    //// convert source data to DataTable
                    DataTable table = encuestaEvaluadorDTO.Respuestas.ToDataTable();
                    //// create parameters
                    var Accion = new SqlParameter("@Accion", SqlDbType.Int);
                    Accion.Value = encuestaEvaluadorDTO.Accion;
                    var IdEncuestaEvaluador = new SqlParameter("@IdEncuestaEvaluador", SqlDbType.Int);
                    IdEncuestaEvaluador.Value = encuestaEvaluadorDTO.IdEncuestaEvaluador;
                    var tvpRespuestas = new SqlParameter("@tvpRespuestas", table);
                    tvpRespuestas.SqlDbType = SqlDbType.Structured;
                    tvpRespuestas.TypeName = "TVP_Respuestas";
                    var intOutput = new SqlParameter("@intOutput", SqlDbType.Bit); //var totalCountParam = new SqlParameter("TotalCount", 0) { Direction = ParameterDirection.Output };
                    intOutput.Value = false;
                    intOutput.Direction = ParameterDirection.Output;
                    //// save using procedure with table value parameter
                    context.ExecuteTableValueProcedure("SP_UpdateRespuestas", "@Accion,@IdEncuestaEvaluador,@tvpRespuestas,@intOutput OUTPUT", new object[] { Accion, IdEncuestaEvaluador, tvpRespuestas, intOutput });
                    return (bool)intOutput.Value;
                    //for (int i = 0; i < encuestaEvaluadorDTO.listaRespuestas.Count; i++)
                    //{
                    //    var respuesta = context.Respuestas.AsEnumerable().Where(x => x.IdEncuestaEvaluador == encuestaEvaluadorDTO.IdEncuestaEvaluador && x.IdPregunta == encuestaEvaluadorDTO.listaPreguntas[i] && x.IdEvaluado == encuestaEvaluadorDTO.listaEvaluados[i]).SingleOrDefault();
                    //    if (respuesta != null)
                    //    {
                    //        if (encuestaEvaluadorDTO.listaRespuestas[i] == "0" || encuestaEvaluadorDTO.listaRespuestas[i].Trim() == "")
                    //            context.Respuestas.Remove(respuesta);
                    //        else if (encuestaEvaluadorDTO.listaRespuestas[i] != respuesta.Valor)
                    //            respuesta.Valor = encuestaEvaluadorDTO.listaRespuestas[i];
                    //    }
                    //    else
                    //    {
                    //        if (encuestaEvaluadorDTO.listaRespuestas[i] != "0" && encuestaEvaluadorDTO.listaRespuestas[i].Trim() != "")
                    //        {
                    //            Respuestas respuestaNew = new Respuestas();
                    //            respuestaNew.IdEncuestaEvaluador = encuestaEvaluadorDTO.IdEncuestaEvaluador;
                    //            respuestaNew.IdPregunta = encuestaEvaluadorDTO.listaPreguntas[i];
                    //            respuestaNew.IdEvaluado = encuestaEvaluadorDTO.listaEvaluados[i];
                    //            respuestaNew.Valor = encuestaEvaluadorDTO.listaRespuestas[i];
                    //            context.Respuestas.Add(respuestaNew);
                    //        }
                    //    }
                    //}
                    //if (encuestaEvaluadorDTO.Accion == 2) //Se ejecuta si se presiona "Enviar y Salir"
                    //{
                    //    var encuestaEvaluador = context.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == encuestaEvaluadorDTO.IdEncuestaEvaluador).SingleOrDefault();
                    //    encuestaEvaluador.EstaCompleto = true; //Encuesta terminada
                    //}
                    //context.SaveChanges();
                    //return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool updateEstadoProcesoEncuesta(int idEncuesta)
        {
            using (var context = getContext())
            {
                try
                {
                    if (context.SP_GetEncuestaCompleta(idEncuesta).SingleOrDefault() == 1)
                    {
                        var encuesta = context.Encuesta.Where(x => x.IdEncuesta == idEncuesta).SingleOrDefault();
                        encuesta.EstadoProceso = "Completo"; //Todas las encuestas han sido completadas.
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

    }
}
