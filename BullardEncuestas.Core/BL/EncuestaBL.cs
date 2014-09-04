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
    public class EncuestaBL : Base
    {
        public IList<EncuestaDTO> getEncuestas()//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.SP_GetEncuestas().Select(r => new EncuestaDTO
                {
                    IdEncuesta = r.IdEncuesta,
                    NombreEncuesta = r.NombreEncuesta,
                    Periodo = new PeriodoDTO { Descripcion = r.NombrePeriodo },
                    EstadoEncuesta = r.Estado,
                    IdGrupoEvaluado = r.IdGrupoEvaluado,
                    StrGrupoEvaluador = r.StrGrupoEvaluador
                    //IdGrupoEvaluado = r.IdGrupoEvaluado
                    //GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.NombreGrupo }
                }).ToList();
                return result;
            }
        }

        public IList<EncuestaDTO> getEncuestas2()//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.SP_GetEncuestas2().Select(r => new EncuestaDTO
                {
                    IdEncuesta = r.IdEncuesta,
                    NombreEncuesta = r.NombreEncuesta,
                    Periodo = new PeriodoDTO { Descripcion = r.NombrePeriodo },
                    EstadoEncuesta = r.Estado,
                    IdGrupoEvaluado = r.IdGrupoEvaluado,
                    IdPeriodo = r.IdPeriodo
                    //GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.NombreGrupo }
                }).ToList();
                return result;
            }
        }

        public IList<EncuestaDTO> getEncuestasReporte()//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.SP_GetEncuestas2().Select(r => new EncuestaDTO
                {
                    IdEncuesta = r.IdEncuesta,
                    NombreEncuesta = r.NombreEncuesta,
                    Periodo = new PeriodoDTO { Descripcion = r.NombrePeriodo },
                    GrupoEvaluado = new GrupoTrabajoDTO { Nombre = r.NombreGrupo },
                    EstadoEncuesta = r.Estado,
                    IdGrupoEvaluado = r.IdGrupoEvaluado,
                    IdPeriodo = r.IdPeriodo,
                    PromedioGeneral = r.PromedioGeneral,
                    listaReporteDetalle = context.SP_GetEncuestasReporteDetalle2(r.IdEncuesta, r.IdPeriodo, r.IdGrupoEvaluado)
                    .Select(w => new ReporteDTO
                    {
                        IdPregunta = w.IdPregunta,
                        DescripPregunta = w.DescripPregunta,
                        IdEvaluado = w.IdEvaluado,
                        NombreEvaluado = w.NombreEvaluado,
                        PromedioPreguntaXEvaluado = w.PromedioPreguntaXEvaluado
                    }
                    ).ToList(),
                    listaReportePersonas = context.SP_GetPersonasEnEncuesta(r.IdEncuesta, r.IdPeriodo, r.IdGrupoEvaluado)
                    .Select(p => new PersonaDTO
                    {
                        IdPersona = p.IdPersona,
                        Nombre = p.NombreEvaluado,
                    }).ToList(),
                    listaReportePreguntas = context.SP_GetPreguntasEnEncuesta(r.IdEncuesta, r.IdPeriodo, r.IdGrupoEvaluado)
                    .Select(pre => new PreguntaDTO 
                    {
                        IdPregunta = pre.IdPregunta,
                        Descripcion = pre.PreguntaDescrip
                    }).ToList()
                    //GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.NombreGrupo }
                }).ToList();

                
                foreach(EncuestaDTO item in result)
                {
                    //item.matrizReporteDetalle
                    int numeroPreguntas = item.listaReportePreguntas.Count;
                    int numeroPersonas = item.listaReportePersonas.Count;

                    ItemMatriz [,] nuevaMatriz = new ItemMatriz[numeroPreguntas,numeroPersonas];
                    for(int i=0; i<numeroPreguntas; i++)
                    {
                        for(int j=0; j<numeroPersonas; j++)
                        {
                            ItemMatriz nuevo = new ItemMatriz();

                            nuevo.IdItemColumn = item.listaReportePersonas[j].IdPersona;
                            nuevo.NombreItemColumn = item.listaReportePersonas[j].Nombre;
                            nuevo.IdItemRow = item.listaReportePreguntas[i].IdPregunta;
                            nuevo.NombreItemRow = item.listaReportePreguntas[i].Descripcion;
                            nuevo.ValorItem = -1;

                            nuevaMatriz[i, j] = nuevo;
                        }
                    }

                    for (int i = 0; i < numeroPreguntas; i++)
                    {
                        for (int j = 0; j < numeroPersonas; j++)
                        {
                            foreach(ReporteDTO itemDetalle in item.listaReporteDetalle)
                            {
                                if(nuevaMatriz[i,j].IdItemColumn == itemDetalle.IdEvaluado && nuevaMatriz[i,j].IdItemRow == itemDetalle.IdPregunta)
                                {
                                    nuevaMatriz[i, j].ValorItem = itemDetalle.PromedioPreguntaXEvaluado;
                                }
                            }
                        }
                    }

                    item.matrizReporteDetalle = nuevaMatriz;
                }

                return result;
            }
        }

        public EncuestaDTO getEncuesta(int id)//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.Encuesta.Where(x => x.IdEncuesta == id).AsEnumerable()
                    .Select(r => new EncuestaDTO
                    {
                        IdEncuesta = r.IdEncuesta,
                        NombreEncuesta = r.Nombre,
                        Instrucciones = r.Instrucciones,
                        Leyenda = r.Leyenda,
                        IdPeriodo = r.IdPeriodo,
                        IdGrupoEvaluado = r.IdGrupoEvaluado,
                        EstadoEncuesta = r.Estado,
                        Periodo = new PeriodoDTO { Descripcion = r.Periodo.Descripcion },
                        GrupoEvaluado = new GrupoTrabajoDTO { Nombre = r.Nombre },
                        StrGrupoEvaluador = string.Join(", ", r.GrupoTrabajo1.Select(y => y.IdGrupoTrabajo)),
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
                                IdTipoRespuesta = y.IdTipoRespuesta
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
                                    IdTipoRespuesta = z.IdTipoRespuesta
                                }).OrderBy(z => z.OrdenPregunta).ToList()
                            }).OrderBy(y => y.Orden).ToList()
                        }).OrderBy(x => x.Orden).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }

        /*public void SendMailGrupo(int idGrupo, string nombreEncuesta, string periodo)
        {
            string to = string.Empty, copy = string.Empty, subject = string.Empty, body = string.Empty;
            PersonaBL oBL = new PersonaBL();
            var personas = oBL.getPersonasPorGrupo(idGrupo);
            string host = getHost();
            foreach (var item in personas)
            {
                var link = host + "/Admin/EncuestaEncuestador/" + item.IdPersona;
                subject = "Encuesta : " + nombreEncuesta;
                body = "Estimado " + item.Nombre + ",<br/>se ha abierto la encuesta " + nombreEncuesta + " para el Periodo " + periodo +
                    ", sirvase a contestar la encuesta a traves de este enlace:<br/>" + link + "Atentamente,<br/>La Administración.";
                to = item.Email;
                MailHandler.Send(to, copy, subject, body);
            }
        }*/
        public bool SendMailGrupo(string grupoEvaluadores, int idEncuesta, int idGrupoEvaluado, string nombreEncuesta, string periodo)
        {
            try
            {
                string to = string.Empty, copy = string.Empty, subject = string.Empty, body = string.Empty;
                PersonaBL oBL = new PersonaBL();
                List<int> GrupoEvaluadores = (grupoEvaluadores != null ? grupoEvaluadores.Split(',').Select(x => Convert.ToInt32(x)).ToList() : new List<int>());
                foreach (var grupo in GrupoEvaluadores)
                {
                    var personas = oBL.getPersonasPorGrupo(grupo);
                    string host = getHost();
                    foreach (var persona in personas)
                    {
                        var link = host + "/Admin/LlenarEncuesta?idEncuesta=" + idEncuesta + "&idGrupoEvaluado=" + idGrupoEvaluado + "&idEvaluador=" + persona.IdPersona;
                        subject = "Encuesta : " + nombreEncuesta;
                        body = "Estimado " + persona.Nombre + ",<br/>se ha abierto la encuesta " + nombreEncuesta + " para el Periodo " + periodo +
                        ", sirvase a contestar la encuesta a traves de este enlace:<br/>" + link + "<br/>Atentamente,<br/>La Administración.";
                        to = persona.Email;
                        MailHandler.Send(to, copy, subject, body);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool add(EncuestaDTO encuestaDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    context.SP_ReplicaEncuesta(encuestaDTO.IdPeriodo, encuestaDTO.IdGrupoEvaluado);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                    //return false;
                }
            }
        }

        public bool update(EncuestaDTO encuestaDTO)
        {
            using (var context = getContext())
            {
                try
                {
                    var encuesta = context.Encuesta.Where(x => x.IdEncuesta == encuestaDTO.IdEncuesta).SingleOrDefault();
                    encuesta.Nombre = encuestaDTO.NombreEncuesta;
                    encuesta.Instrucciones = encuestaDTO.Instrucciones;
                    encuesta.Leyenda = encuestaDTO.Leyenda;
                    encuesta.IdPeriodo = encuestaDTO.IdPeriodo;
                    encuesta.IdGrupoEvaluado = encuestaDTO.IdGrupoEvaluado;
                    encuesta.Estado = encuestaDTO.EstadoEncuesta;
                    var oldGrupos = encuesta.GrupoTrabajo1.Select(x => x.IdGrupoTrabajo).ToList();
                    var newGrupos = encuestaDTO.GrupoEvaluador.Select(x => x).ToList();
                    var gruposToRemove = oldGrupos.Except(newGrupos).ToList();
                    var gruposToAdd = newGrupos.Except(oldGrupos).ToList();
                    foreach (var group in gruposToRemove)
                    {
                        var grupo = context.GrupoTrabajo.Where(x => x.IdGrupoTrabajo == group).SingleOrDefault();
                        encuesta.GrupoTrabajo1.Remove(grupo);
                    }
                    foreach (var group in gruposToAdd)
                    {
                        var grupo = context.GrupoTrabajo.Where(x => x.IdGrupoTrabajo == group).SingleOrDefault();
                        encuesta.GrupoTrabajo1.Add(grupo);
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
