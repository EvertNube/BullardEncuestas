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
                    Periodo = new PeriodoDTO { Descripcion = r.NombrePeriodo }
                    //GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.NombreGrupo }
                }).ToList();
                return result;
            }
        }

        public EncuestaDTO getEncuesta(int id)//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.Encuesta.Where(x => x.IdEncuesta == id)
                    .Select(r => new EncuestaDTO
                    {
                        IdEncuesta = r.IdEncuesta,
                        NombreEncuesta = r.Nombre,
                        IdPeriodo = r.IdPeriodo,
                        IdGrupoTrabajo = r.IdGrupoTrabajo,
                        Estado = r.Estado,
                        Periodo = new PeriodoDTO { Descripcion = r.Periodo.Descripcion },
                        GrupoTrabajo = new GrupoTrabajoDTO { Nombre = r.Nombre },
                        Secciones = r.Seccion.Where(x => x.IdSeccionPadre == null).Select(x => new SeccionDTO
                        {
                            IdSeccion = x.IdSeccion,
                            Nombre = x.Nombre,
                            Orden = x.Orden,
                            EsSocio = x.EsSocio,
                            Estado = x.Estado,
                            Preguntas = x.Pregunta.Select(y => new PreguntaDTO {
                                IdPregunta = y.IdPregunta,
                                Texto = y.Texto,
                                Descripcion = y.Descripcion,
                                Orden = y.Orden,
                                Estado = y.Estado
                            }).OrderBy(y => y.Orden).ToList(),
                            SubSecciones = r.Seccion.Where(y => y.IdSeccionPadre == x.IdSeccion).Select(y => new SeccionDTO {
                                IdSeccion = y.IdSeccion,
                                Nombre = y.Nombre,
                                Orden = y.Orden,
                                EsSocio = y.EsSocio,
                                Estado = y.Estado,
                                Preguntas = y.Pregunta.Select(z => new PreguntaDTO {
                                    IdPregunta = z.IdPregunta,
                                    Texto = z.Texto,
                                    Descripcion = z.Descripcion,
                                    Orden = z.Orden,
                                    Estado = z.Estado
                                }).OrderBy(z => z.Orden).ToList()
                            }).OrderBy(y => y.Orden).ToList(),
                        }).OrderBy(x => x.Orden).ToList(),
                    }).SingleOrDefault();
                return result;
            }
        }

        public EncuestaEvaluadorDTO getEncuestaEvaluador(int id)
        {
            using (var context = getContext())
            {
                var result = context.Encuesta;
                return new EncuestaEvaluadorDTO();
            }
        }

        public void SendMailGrupo(int idGrupo, string nombreEncuesta, string periodo)
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
        }

    }
}
