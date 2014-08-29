using BullardEncuestas.Core.BL;
using BullardEncuestas.Core.DTO;
using BullardEncuestas.Helpers;
using BullardEncuestas.Helpers.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PagedList;

namespace BullardEncuestas.Controllers
{
    public class AdminController : Controller
    {
        private bool currentUser()
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["User"] != null) { return true; }
            else { return false; }
        }
        private UsuarioDTO getCurrentUser()
        {
            if (this.currentUser())
            {
                return (UsuarioDTO)System.Web.HttpContext.Current.Session["User"];
            }
            return null;
        }
        private bool isSuperAdministrator()
        {
            if (getCurrentUser().IdRol == 1) return true;
            return false;
        }
        private bool isAdministrator()
        {
            if (getCurrentUser().IdRol <= 2) return true;
            return false;
        }
        private void createResponseMessage(string status, string message = "", string status_field = "status", string message_field = "message")
        {
            TempData[status_field] = status;
            if (!String.IsNullOrWhiteSpace(message))
            {
                TempData[message_field] = message;
            }
        }

        public AdminController()
        {
            UsuarioDTO user = getCurrentUser();
            if (user != null)
            {
                ViewBag.currentUser = user;
                ViewBag.EsAdmin = isAdministrator();
                ViewBag.EsSuperAdmin = isSuperAdministrator();
                ViewBag.IdRol = user.IdRol;
            }
            else { ViewBag.EsAdmin = false; ViewBag.EsSuperAdmin = false; }
        }

        public ActionResult Ingresar()
        {
            return View();
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index(int? page)
        {
            EncuestaBL oBL = new EncuestaBL();
            var model = oBL.getEncuestas();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Encuesta(int? id = null)
        {
            var objSent = (EncuestaDTO)TempData["Encuesta"];
            if (objSent != null)
            {
                TempData["Encuesta"] = null;
                return View(objSent);
            }
            if (id != null)
            {
                EncuestaBL objBL = new EncuestaBL();
                EncuestaDTO obj = objBL.getEncuesta((int)id);
                return View(obj);
            }
            return View();
        }
        public ActionResult SendCorreo(int idGrupo, string nombreEncuesta, string periodo)
        {
            EncuestaBL oBL = new EncuestaBL();
            oBL.SendMailGrupo(idGrupo, nombreEncuesta, periodo);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(UsuarioDTO user)
        {
            if (ModelState.IsValid)
            {
                UsuariosBL usuariosBL = new UsuariosBL();
                if (usuariosBL.isValidUser(user))
                {
                    System.Web.HttpContext.Current.Session["User"] = usuariosBL.getUsuarioByCuenta(user);//new UsuarioDTO() { Nombre = "NubeLabs", IdUsuario = 1, IdRol = 1 }; //{ Nombre = "Responsable 1", IdUsuario = 2, IdRol = 3 };  //usuariosBL.getUsuarioByCuenta(user);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Ingresar");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Ingresar");
        }

        public List<SeccionDTO> GenerarEncuestaPrueba()
        {
            List<SeccionDTO> listaSeccion = new List<SeccionDTO>();

            for (int i = 0; i < 4; i++)
            {
                SeccionDTO nuevoS = new SeccionDTO();
                nuevoS.Nombre = "Seccion" + i.ToString();
                nuevoS.IdEncuesta = 1;
                nuevoS.Orden = i;

                List<PreguntaDTO> listaPregunta = new List<PreguntaDTO>();
                for (int j = 0; j < 4; j++)
                {
                    PreguntaDTO nuevaP = new PreguntaDTO();
                    nuevaP.IdPregunta = (10 + i) * 10 + j;
                    switch (j)
                    {
                        case 0:
                            nuevaP.Descripcion = "¿Como calificaria el servicio ofrecido durante los 3 ultimos meses?";
                            break;
                        case 1:
                            nuevaP.Descripcion = "¿Como calificarias el desempeño de los trabajadores?";
                            break;
                        case 2:
                            nuevaP.Descripcion = "¿Como calificarias la calidad de respuesta de tu asesor?";
                            break;
                        case 3:
                            nuevaP.Descripcion = "¿Como calificarias el nivel de atención de los profesionales asociados?";
                            break;
                    }
                    nuevaP.IdSeccion = i;
                    nuevaP.OrdenPregunta = j;
                    listaPregunta.Add(nuevaP);
                }
                List<SeccionDTO> listaSecciones = new List<SeccionDTO>();
                for (int k = 0; k < 4; k++)
                {
                    SeccionDTO auxSec = new SeccionDTO();

                    auxSec.IdSeccionPadre = i;
                    auxSec.IdSeccion = k;
                    auxSec.Nombre = "SeccionAux" + k.ToString();
                    auxSec.Orden = k;

                    listaSecciones.Add(auxSec);
                }

                nuevoS.Preguntas = listaPregunta;
                nuevoS.SubSecciones = listaSecciones;
                listaSeccion.Add(nuevoS);
            }
            return listaSeccion;
        }

        public ActionResult Formulario()
        { return View(); }

        public ActionResult LlenarEncuesta(int idEncuesta, int idGrupo, int idEvaluador)
        {
            EncuestaBL oBL = new EncuestaBL();
            PersonaBL oPersonaBL = new PersonaBL();
            OpcionesRespuestaBL oOpcionesRespuestaBL = new OpcionesRespuestaBL();
            //Id por defecto 1
            //ViewBag.Encuesta = oBL.getEncuesta(1);
            ViewBag.Evaluados = oPersonaBL.getPersonasPorGrupo(idGrupo);
            ViewBag.Items_SelectSINO = oOpcionesRespuestaBL.getOpcionesRespuesta(3, true);
            var model = oBL.getEncuestaEvaluador(idEncuesta, idEvaluador);
            return View(model); //View(new EncuestaEvaluadorDTO());
        }

        public ActionResult AddEncuesta(EncuestaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                EncuestaBL objBL = new EncuestaBL();
                if (dto.IdEncuesta == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Encuesta", new { id = dto.IdEncuesta });
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else if (dto.IdEncuesta != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Encuesta", new { id = dto.IdEncuesta });
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            catch
            {
                if (dto.IdEncuesta != 0) createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Encuesta"] = dto;
            return RedirectToAction("Encuesta");
        }

        public ActionResult AddSeccion(SeccionDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                SeccionBL objBL = new SeccionBL();
                if (dto.IdSeccion == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Encuesta", new { id = dto.IdEncuesta });
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else if (dto.IdSeccion != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Encuesta", new { id = dto.IdEncuesta });
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            catch
            {
                if (dto.IdSeccion != 0) createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Seccion"] = dto;
            return RedirectToAction("Encuesta");
        }

        public ActionResult AddPregunta(PreguntaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                PreguntaBL objBL = new PreguntaBL();
                if (dto.IdPregunta == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Encuesta", new { id = dto.IdEncuesta });
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else if (dto.IdPregunta != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Encuesta", new { id = dto.IdEncuesta });
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            catch
            {
                if (dto.IdPregunta != 0) createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Pregunta"] = dto;
            return RedirectToAction("Encuesta");
        }

        public ActionResult AddEncuestaEvaluador(EncuestaEvaluadorDTO dto)
        {
            //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                EncuestaEvaluadorBL objBL = new EncuestaEvaluadorBL();
                dto.Respuestas = new List<RespuestasDTO>();
                for (int i = 0; i < dto.listaRespuestas.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dto.listaRespuestas[i].Trim()) && dto.listaRespuestas[i] != "0")
                    {
                        RespuestasDTO respuesta = new RespuestasDTO();
                        respuesta.IdPregunta = dto.listaPreguntas[i];
                        respuesta.IdEvaluado = dto.listaEvaluados[i];
                        respuesta.Valor = dto.listaRespuestas[i];
                        dto.Respuestas.Add(respuesta);
                    }
                }
                if (dto.IdEncuestaEvaluador == 0)
                {
                    if (objBL.add(dto))
                    {
                        //dto.IdProtocolo = idProtocolo;
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Protocolos");
                    }
                }
                else if (dto.IdEncuestaEvaluador != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Protocolos");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch
            {
                if (dto.IdEncuestaEvaluador != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Protocolo"] = dto;
            return RedirectToAction("LlenarEncuesta");
        }

        #region APIS
        [HttpGet]
        public ActionResult GetTiposRespuesta(bool AsSelectList = false)
        {
            //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            TipoRespuestaBL objBL = new TipoRespuestaBL();
            var lista = objBL.getTiposRespuesta(true);
            if (AsSelectList)
                lista.Insert(0, new TipoRespuestaDTO { IdTipoRespuesta = 0, Nombre = "Seleccione un Tipo Respuesta" });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetPeriodos(bool AsSelectList = false)
        {
            //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            PeriodoBL objBL = new PeriodoBL();
            var lista = objBL.getPeriodos(true);
            if (AsSelectList)
                lista.Insert(0, new PeriodoDTO { IdPeriodo = 0, Descripcion = "Seleccione un Periodo" });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetGruposEvaluados(bool AsSelectList = false)
        {
            //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            GrupoTrabajoBL objBL = new GrupoTrabajoBL();
            var lista = objBL.getGruposEvaluados(true);
            if (AsSelectList)
                lista.Insert(0, new GrupoTrabajoDTO { IdGrupoTrabajo = 0, Nombre = "Seleccione un Grupo" });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult GetGruposEvaluadores(bool AsSelectList = false)
        //{
        //    //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
        //    GrupoTrabajoBL objBL = new GrupoTrabajoBL();
        //    var lista = objBL.getGruposEvaluadores(true);
        //    if (AsSelectList)
        //        lista.Insert(0, new GrupoTrabajoDTO { IdGrupoTrabajo = 0, Nombre = "Seleccione un Grupo" });
        //    return Json(lista, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}
