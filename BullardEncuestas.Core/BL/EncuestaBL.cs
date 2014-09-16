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
                    EstadoProceso = r.EstadoProceso,
                    IdGrupoEvaluado = r.IdGrupoEvaluado,
                    GrupoEvaluado = new GrupoTrabajoDTO { Nombre = r.NombreGrupo },
                    StrGrupoEvaluador = r.StrGrupoEvaluador
                }).ToList();
                return result;
            }
        }

        public List<EncuestaDTO> getEncuestaEnPeriodo(int idPeriodo)
        {
            using (var context = getContext())
            {
                var result = context.SP_GetEncuestasPorIdPeriodo(idPeriodo)
                     .Select(r => new EncuestaDTO
                     {
                         IdEncuesta = r.IdEncuesta,
                         NombreEncuesta = r.NombreEncuesta,
                         Periodo = new PeriodoDTO { IdPeriodo = r.IdPeriodo, Descripcion = r.NombrePeriodo },
                         GrupoEvaluado = new GrupoTrabajoDTO { IdGrupoTrabajo = r.IdGrupoEvaluado, Nombre = r.NombreGrupo },
                         EstadoEncuesta = r.Estado
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
                        TextoPregunta = w.TextoPregunta,
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
                        Promedio = p.PromedioPersona
                    }).ToList(),
                    listaReportePreguntas = context.SP_GetPreguntasEnEncuesta(r.IdEncuesta, r.IdPeriodo, r.IdGrupoEvaluado)
                    .Select(pre => new PreguntaDTO
                    {
                        IdPregunta = pre.IdPregunta,
                        Texto = pre.TextoPregunta
                    }).ToList()
                    //GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.NombreGrupo }
                }).ToList();


                foreach (EncuestaDTO item in result)
                {
                    //item.matrizReporteDetalle
                    int numeroPreguntas = item.listaReportePreguntas.Count;
                    int numeroPersonas = item.listaReportePersonas.Count;

                    ItemMatriz[,] nuevaMatriz = new ItemMatriz[numeroPreguntas, numeroPersonas];
                    for (int i = 0; i < numeroPreguntas; i++)
                    {
                        for (int j = 0; j < numeroPersonas; j++)
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
                            foreach (ReporteDTO itemDetalle in item.listaReporteDetalle)
                            {
                                if (nuevaMatriz[i, j].IdItemColumn == itemDetalle.IdEvaluado && nuevaMatriz[i, j].IdItemRow == itemDetalle.IdPregunta)
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

        public EncuestaDTO getEncuestaReporteDetalle(int id)
        {
            using (var context = getContext())
            {
                //var result = context.Encuesta.Where(x => x.IdEncuesta == 2).AsEnumerable()
                var result = context.SP_GetEncuestasReportePorId(id).AsEnumerable()
                .Select(r => new EncuestaDTO
                {
                    IdEncuesta = r.IdEncuesta,
                    NombreEncuesta = r.NombreEncuesta,
                    Periodo = new PeriodoDTO { Descripcion = r.NombrePeriodo },
                    GrupoEvaluado = new GrupoTrabajoDTO { Nombre = r.NombreGrupo },
                    EstadoEncuesta = r.Estado,
                    IdGrupoEvaluado = r.IdGrupoTrabajo,
                    IdPeriodo = r.IdPeriodo,
                    PromedioGeneral = r.PromedioGeneral,
                    PromGeneralAnterior = r.PromedioGeneralAnterior,
                    listaReporteDetalle = context.SP_GetEncuestasReporteDetalle2(r.IdEncuesta, r.IdPeriodo, r.IdGrupoTrabajo)
                    .Select(w => new ReporteDTO
                    {
                        IdPregunta = w.IdPregunta,
                        TextoPregunta = w.TextoPregunta,
                        IdEvaluado = w.IdEvaluado,
                        NombreEvaluado = w.NombreEvaluado,
                        PromedioPreguntaXEvaluado = w.PromedioPreguntaXEvaluado
                    }
                    ).ToList(),
                    listaReportePersonas = context.SP_GetPersonasEnEncuesta(r.IdEncuesta, r.IdPeriodo, r.IdGrupoTrabajo)
                    .Select(p => new PersonaDTO
                    {
                        IdPersona = p.IdPersona,
                        Nombre = p.NombreEvaluado,
                        Promedio = p.PromedioPersona,
                        UrlImagen = p.RutaImagenPersona
                    }).ToList(),
                    listaReportePreguntas = context.SP_GetPreguntasEnEncuesta(r.IdEncuesta, r.IdPeriodo, r.IdGrupoTrabajo)
                    .Select(pre => new PreguntaDTO
                    {
                        IdPregunta = pre.IdPregunta,
                        Texto = pre.TextoPregunta
                    }).ToList()
                }).SingleOrDefault();

                int numeroPreguntas = result.listaReportePreguntas.Count;
                int numeroPersonas = result.listaReportePersonas.Count;

                ItemMatriz[,] nuevaMatriz = new ItemMatriz[numeroPreguntas, numeroPersonas];
                for (int i = 0; i < numeroPreguntas; i++)
                {
                    for (int j = 0; j < numeroPersonas; j++)
                    {
                        ItemMatriz nuevo = new ItemMatriz();

                        nuevo.IdItemColumn = result.listaReportePersonas[j].IdPersona;
                        nuevo.NombreItemColumn = result.listaReportePersonas[j].Nombre;
                        nuevo.IdItemRow = result.listaReportePreguntas[i].IdPregunta;
                        nuevo.NombreItemRow = result.listaReportePreguntas[i].Descripcion;
                        nuevo.ValorItem = -1;

                        nuevaMatriz[i, j] = nuevo;
                    }
                }

                for (int i = 0; i < numeroPreguntas; i++)
                {
                    for (int j = 0; j < numeroPersonas; j++)
                    {
                        foreach (ReporteDTO itemDetalle in result.listaReporteDetalle)
                        {
                            if (nuevaMatriz[i, j].IdItemColumn == itemDetalle.IdEvaluado && nuevaMatriz[i, j].IdItemRow == itemDetalle.IdPregunta)
                            {
                                nuevaMatriz[i, j].ValorItem = itemDetalle.PromedioPreguntaXEvaluado;
                            }
                        }
                    }
                }

                result.matrizReporteDetalle = nuevaMatriz;

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
                        EstadoProceso = r.EstadoProceso,
                        FechaInicio = r.FechaInicio,
                        FechaCierre = r.FechaCierre,
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
                        to = persona.Email;
                        subject = "Encuesta : " + nombreEncuesta;
                        var link = host + "/Admin/LlenarEncuesta?idEncuesta=" + idEncuesta + "&idGrupoEvaluado=" + idGrupoEvaluado + "&idEvaluador=" + persona.IdPersona;
                        //body = "Estimado " + persona.Nombre + ",<br/>se ha abierto la encuesta " + nombreEncuesta + " para el Periodo " + periodo +
                        //", sirvase a contestar la encuesta a traves de este enlace:<br/>" + link + "<br/>Atentamente,<br/>La Administración.";
                        body = cuerpoCorreo(persona.Nombre, nombreEncuesta, periodo, link);
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
                    //encuesta.IdPeriodo = encuestaDTO.IdPeriodo;
                    encuesta.IdGrupoEvaluado = encuestaDTO.IdGrupoEvaluado;
                    encuesta.EstadoProceso = "En proceso";//encuestaDTO.EstadoEncuesta;
                    encuesta.FechaInicio = encuestaDTO.FechaInicio;
                    encuesta.FechaCierre = encuestaDTO.FechaCierre;
                    var oldGrupos = encuesta.GrupoTrabajo1.Select(x => x.IdGrupoTrabajo).ToList();
                    var newGrupos = encuestaDTO.GrupoEvaluador != null ? encuestaDTO.GrupoEvaluador.Select(x => x).ToList() : new List<int>();
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

        public bool updateEstado(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    var encuesta = context.Encuesta.Where(x => x.IdEncuesta == id).SingleOrDefault();
                    if (encuesta.EstadoProceso == "Pendiente") encuesta.EstadoProceso = "En proceso";
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        private string cuerpoCorreo(string NombrePersona, string NombreEncuesta, string Periodo, string Link)
        {
            var body = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
"<head>" +
    "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
    "<meta content='telephone=no' name='format-detection' />" +
    "<title>Email Template</title>" +
    "<style type='text/css' media='screen'>" +
        "body { padding:0 !important; margin:0 !important; display:block !important; background:#ffffff; -webkit-text-size-adjust:none }" +
        "a { color:#00b8e4; text-decoration:underline }" +
        "h3 a { color:#1f1f1f; text-decoration:none }" +
        ".text2 a { color:#ea4261; text-decoration:none }" +
        "p { padding:0 !important; margin:0 !important } " +
    "</style>" +
"</head>" +

"<body class='body' style='padding:0 !important; margin:0 !important; display:block !important; background:#ffffff; -webkit-text-size-adjust:none'>" +

"<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#ffffff'>" +
    "<tr>" +
        "<td align='center' valign='top'>" +
            "<table width='800' border='0' cellspacing='0' cellpadding='0'>" +
                "<tr>" +
                    "<td align='center' bgcolor='#0082c9'>" +
                        "<table width='620' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                                "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left'><a href='www.bullardabogados.pe' target='_blank'><img src='http://54.186.226.155:75/Content/themes/admin/images/bullard.jpg' alt='' border='0' height='52' /></a></td>" +
                                "<td align='right'>" +
                                    "<table border='0' cellspacing='0' cellpadding='0'>" +
                                        "<tr>" +
                                            "<td class='top' style='color:#ea4261; font-family:Tahoma; font-size:12px; line-height:18px; text-align:left'><a class='link-top' style='color:#ffffff; text-decoration:underline' target='_blank' href='www.bullardabogados.pe'>bullardabogados.pe</a></td>" +
                                            "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left' width='10'></td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                                "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left' width='90'></td>" +
                                "<td>" +
                                    "<div style='font-size:0pt; line-height:0pt; height:30px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='30' style='height:30px' alt='' /></div>" +
                                    "<div>" +
                                        "<div class='h2' style='color:#1f1f1f; font-family:Tahoma; font-size:20px; line-height:24px; text-align:center; font-weight:bold'>" +
                                            "<div>BFE+ - Encuestas</div>" +
                                        "</div>" +
                                        "<div style='font-size:0pt; line-height:0pt; height:10px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='10' style='height:10px' alt='' /></div>" +
                                        "<div class='text-left' style='color:#868686; font-family:Tahoma; font-size:14px; line-height:18px; text-align:left'>" +
                                            "<div>Hola " + NombrePersona + ",<br />" +
                                            "Se ha abierto la encuesta " + NombreEncuesta + " para el período " + Periodo + ".<br />¡Nos encantaría conocer tu opinión! Por favor, contesta la encuesta a través de este enlace.</div>" +
                                        "</div>" +
                                        "<div style='font-size:0pt; line-height:0pt; height:35px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='35' style='height:35px' alt='' /></div>" +
                                    "</div>" +
                                    "<div>" +
                                        "<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                            "<tr>" +
                                                "<td valign='top' width='130'>" +
                                                "<div></div>" +
                                                    "<div class='img-center' style='font-size:0pt; line-height:0pt; text-align:center'><a href='" + Link + "' target='_blank'><img src='http://54.186.226.155:75/Content/images/img2.jpg' alt='' border='0' width='109' height='109' /></a></div>" +
                                                    "<div style='font-size:0pt; line-height:0pt; height:30px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='30' style='height:30px' alt='' /></div>" +
                                                    "<div class='h3' style='color:#1f1f1f; font-family:Tahoma; font-size:16px; line-height:20px; text-align:center; font-weight:normal'>" +
                                                        "<div><a href='" + Link + "' target='_blank'>" + NombreEncuesta + "</a></div>" +
                                                    "</div>" +
                                                    "<div style='font-size:0pt; line-height:0pt; height:15px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='15' style='height:15px' alt='' /></div>" +
                                                    "<div class='text2-center' style='color:#bebebe; font-family:Tahoma; font-size:12px; line-height:18px; text-align:center'>" +
                                                        "<div></div>" +
                                                    "</div>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                        "<div style='font-size:0pt; line-height:0pt; height:10px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='10' style='height:10px' alt='' /></div>" +
                                    "</div>" +
                                    "<div>" +
                                        "<div style='font-size:0pt; line-height:0pt; height:20px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='20' style='height:20px' alt='' /></div>" +
                                        "<div class='img' style='font-size:0pt; line-height:0pt; text-align:left'><a href='#' target='_blank'><img src='http://54.186.226.155:75/Content/images/separator.jpg' alt='' border='0' width='620' height='13' /></a></div>" +
                                    "</div>" +
                                    "<div>" +
                                        "<div style='font-size:0pt; line-height:0pt; height:35px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='35' style='height:35px' alt='' /></div>" +
                                    "</div>" +
                                "</td>" +
                                "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left' width='90'></td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<div class='img' style='font-size:0pt; line-height:0pt; text-align:left'><img src='http://54.186.226.155:75/Content/images/footer_top.jpg' alt='' border='0' width='800' height='10' /></div>" +
                        "<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#eeefec'>" +
                            "<tr>" +
                                "<td>" +
                                    "<div style='font-size:0pt; line-height:0pt; height:12px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='12' style='height:12px' alt='' /></div>" +
                                    "<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                        "<tr>" +
                                            "<td align='center'>" +
                                                "<table border='0' cellspacing='0' cellpadding='0'>" +
                                                    "<tr>" +
                                                        "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left'></td>" +
                                                        "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left' width='7'></td>" +
                                                        "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left'><a href='www.facebook.com/bullardfallaezcurra' target='_blank'><img src='http://54.186.226.155:75/Content/images/facebook-64.png' alt='' border='0' width='43' height='43' /></a></td>" +
                                                        "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left' width='7'></td>" +
                                                        "<td class='img' style='font-size:0pt; line-height:0pt; text-align:left'></td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                    "<div style='font-size:0pt; line-height:0pt; height:30px'><img src='http://54.186.226.155:75/Content/images/empty.gif' width='1' height='30' style='height:30px' alt='' /></div>" +
                                    "<div class='footer' style='color:#a9aaa9; font-family:Arial; font-size:11px; line-height:20px; text-align:center'>" +
                                        "<div>" +
                                            "Copyright &copy; <span>2014</span> <span>Estudio Bulard Falla Ezcurra +</span>.<br />" +
                                            "Diseñado por <a href='www.nube.la'>Nube Labs</a>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div style='font-size:0pt; line-height:0pt; height:25px'><img src='images/empty.gif' width='1' height='25' style='height:25px' alt='' /></div>" +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
            "</table>" +
        "</td>" +
    "</tr>" +
"</table>" +
"</body>" +
"</html>";
            return body;
        }

    }
}
