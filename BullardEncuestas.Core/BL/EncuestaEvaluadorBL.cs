﻿using BullardEncuestas.Core.DTO;
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
        public EncuestaEvaluadorDTO getEncuestaEvaluador(int idEncuesta, int idEvaluador)
        {
            using (var context = getContext())
            {
                var result = context.Encuesta.Where(r => r.IdEncuesta == idEncuesta).AsEnumerable()
                    .Select(r => new EncuestaEvaluadorDTO
                    {
                        IdEncuestaEvaluador = r.EncuestaEvaluador.Select(x => x.IdEvaluador).FirstOrDefault(),
                        IdEncuesta = r.IdEncuesta,
                        IdEvaluador = idEvaluador,
                        Encuesta = new EncuestaDTO
                        {
                            NombreEncuesta = r.Nombre,
                            Instrucciones = r.Instrucciones,
                            Leyenda = r.Leyenda,
                            IdPeriodo = r.IdPeriodo,
                            IdGrupoEvaluado = r.IdGrupoEvaluado,
                            EstadoEncuesta = r.Estado,
                            Periodo = new PeriodoDTO { Descripcion = r.Periodo.Descripcion },
                            GrupoEvaluado = new GrupoTrabajoDTO { Nombre = r.Nombre },
                            Secciones = r.Seccion.Where(x => x.IdSeccionPadre == null).Select(x => new SeccionDTO
                            {
                                IdSeccion = x.IdSeccion,
                                Nombre = x.Nombre,
                                Orden = x.Orden,
                                EsSocio = x.EsSocio,
                                Estado = x.Estado,
                                Preguntas = x.Pregunta.Select(y => new PreguntaDTO
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
                                SubSecciones = r.Seccion.Where(y => y.IdSeccionPadre == x.IdSeccion).Select(y => new SeccionDTO
                                {
                                    IdSeccion = y.IdSeccion,
                                    Nombre = y.Nombre,
                                    Orden = y.Orden,
                                    EsSocio = y.EsSocio,
                                    Estado = y.Estado,
                                    Preguntas = y.Pregunta.Select(z => new PreguntaDTO
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
                                }).OrderBy(y => y.Orden).ToList(),
                            }).OrderBy(x => x.Orden).ToList(),
                        }
                    }).SingleOrDefault();
                return result;
            }
        }

        public bool add(EncuestaEvaluadorDTO encuestaEvaluadorDTO)
        {
            using (var context = getContext())
            {
                try
                {
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
                    EncuestaEvaluador encuestaEvaluador = new EncuestaEvaluador();
                    encuestaEvaluador.IdEncuesta = encuestaEvaluadorDTO.IdEncuesta;
                    encuestaEvaluador.IdEvaluador = encuestaEvaluadorDTO.IdEvaluador;
                    encuestaEvaluador.CodEvaluador = "";
                    encuestaEvaluador.EstadoEncuesta = false;
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
                    //var oldRespuestas = context.Respuestas.Where(x => x.IdEncuestaEvaluador == encuestaEvaluadorDTO.IdEncuestaEvaluador).ToList();
                    for (int i = 0; i < encuestaEvaluadorDTO.listaRespuestas.Count; i++)
                    {
                        if (encuestaEvaluadorDTO.listaIdRespuestas[i] != 0)
                        {
                            var respuesta = context.Respuestas.AsEnumerable().Where(x => x.IdRespuestas == encuestaEvaluadorDTO.listaIdRespuestas[i]).SingleOrDefault();
                            if (respuesta != null)
                            {
                                if (encuestaEvaluadorDTO.listaRespuestas[i] == "0")
                                    context.Respuestas.Remove(respuesta);
                                else if (encuestaEvaluadorDTO.listaRespuestas[i] != respuesta.Valor)
                                    respuesta.Valor = encuestaEvaluadorDTO.listaRespuestas[i];
                            }
                        }
                        else
                        {
                            if (encuestaEvaluadorDTO.listaRespuestas[i] != "0")
                            {
                                Respuestas respuesta = new Respuestas();
                                respuesta.IdEncuestaEvaluador = encuestaEvaluadorDTO.IdEncuestaEvaluador;
                                respuesta.IdPregunta = encuestaEvaluadorDTO.listaPreguntas[i];
                                respuesta.IdEvaluado = encuestaEvaluadorDTO.listaEvaluados[i];
                                respuesta.Valor = encuestaEvaluadorDTO.listaRespuestas[i];
                                context.Respuestas.Add(respuesta);
                            }
                        }
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

        public bool updateEstadoEncuesta(EncuestaEvaluadorDTO encuestaEvaluadorDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    var encuestaEvaluador = context.EncuestaEvaluador.Where(x => x.IdEncuestaEvaluador == encuestaEvaluadorDTO.IdEncuestaEvaluador).SingleOrDefault();
                    encuestaEvaluador.EstadoEncuesta = true; //Encuesta termiada.
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