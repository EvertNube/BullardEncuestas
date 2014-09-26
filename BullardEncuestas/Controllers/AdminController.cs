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
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;

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
        private void createMessage(string status, string message = "", string status_field = "status_", string message_field = "message_")
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
        /*public ActionResult SendCorreo(int idGrupo, string nombreEncuesta, string periodo)
        {
            EncuestaBL oBL = new EncuestaBL();
            oBL.SendMailGrupo(idGrupo, nombreEncuesta, periodo);
            return RedirectToAction("Index");
        }*/
        //[HttpPost]
        //public ActionResult SendCorreo(int idGrupo, string nombreEncuesta, string periodo)
        //{
        //    EncuestaBL oBL = new EncuestaBL();
        //    var response = oBL.SendMailGrupo(idGrupo, nombreEncuesta, periodo);
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        


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

        public ActionResult LlenarEncuesta(int? idEncuesta, int? idGrupoEvaluado, int? idEvaluador)
        {
            EncuestaEvaluadorBL oBL = new EncuestaEvaluadorBL();
            PersonaBL oPersonaBL = new PersonaBL();
            OpcionesRespuestaBL oOpcionesRespuestaBL = new OpcionesRespuestaBL();
            ViewBag.EsSocio = oPersonaBL.esSocio((int)idEvaluador);
            ViewBag.Evaluados = oPersonaBL.getPersonasPorGrupo((int)idGrupoEvaluado);
            ViewBag.Items_SelectSINO = oOpcionesRespuestaBL.getOpcionesRespuesta(3, true);
            var objSent = (EncuestaEvaluadorDTO)TempData["EncuestaEvaluador"];
            if (objSent != null)
            {
                TempData["EncuestaEvaluador"] = null;
                objSent.IdGrupoEvaluado = idGrupoEvaluado ?? 0;
                objSent.Encuesta = (EncuestaDTO)TempData["Encuesta_"];
                return View(objSent);
            }
            if (idEncuesta != 0 && idEvaluador != 0)
            {
                var model = oBL.getEncuestaEvaluador((int)idEncuesta, (int)idEvaluador);
                model.IdGrupoEvaluado = idGrupoEvaluado ?? 0;
                TempData["Encuesta_"] = model.Encuesta;
                var fechaActual = DateTime.Now.Date;
                if (fechaActual < model.Encuesta.FechaInicio || model.Encuesta.FechaCierre < fechaActual)
                {
                    TempData["MensajeEncuesta"] = "La encuesta se encuentra cerrada.";
                    return RedirectToAction("MensajeEncuesta");
                }
                if (model.EstaCompleto) //if (model.EstadoEncuesta == true)
                {
                    TempData["MensajeEncuesta"] = "Usted ya respondió la encuesta previamente.";
                    return RedirectToAction("MensajeEncuesta");
                }
                return View(model);
            }
            return View();
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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                        return RedirectToAction("Index");
                    }
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

        //public ActionResult AddEncuestaEvaluador(EncuestaEvaluadorDTO dto)
        //{
        //    //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
        //    try
        //    {
        //        EncuestaEvaluadorBL objBL = new EncuestaEvaluadorBL();

        //        if (dto.Accion == 1)
        //        {
        //            //Save
        //            TempData["EncuestaEvaluador"] = dto;
        //            if (dto.IdEncuestaEvaluador == 0)
        //            {
        //                if (objBL.add(dto))
        //                {
        //                    createResponseMessage(CONSTANTES.SUCCESS);
        //                    return RedirectToAction("LlenarEncuesta", new { idGrupoEvaluado = dto.IdGrupoEvaluado, idEncuesta = dto.IdEncuesta, idEvaluador = dto.IdEvaluador });
        //                }
        //            }
        //            else if (dto.IdEncuestaEvaluador != 0)
        //            {
        //                if (objBL.update(dto))
        //                {
        //                    createResponseMessage(CONSTANTES.SUCCESS);
        //                    return RedirectToAction("LlenarEncuesta", new { idGrupoEvaluado = dto.IdGrupoEvaluado, idEncuesta = dto.IdEncuesta, idEvaluador = dto.IdEvaluador });
        //                }
        //                else
        //                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
        //            }
        //            else
        //                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
        //        }
        //        else if (dto.Accion == 2)
        //        {
        //            //Enviar
        //            var conta = dto.listaRespuestas.Where(x => x == "0").Count();
        //            if (conta == 0)
        //            {
        //                if (dto.IdEncuestaEvaluador == 0)
        //                {
        //                    if (objBL.add(dto))
        //                    {
        //                        objBL.updateEstadoEncuesta(dto.IdEncuestaEvaluador);
        //                        return RedirectToAction("MensajeEncuesta");
        //                    }
        //                }
        //                else if (dto.IdEncuestaEvaluador != 0)
        //                {
        //                    if (objBL.update(dto))
        //                    {
        //                        objBL.updateEstadoEncuesta(dto.IdEncuestaEvaluador);
        //                        return RedirectToAction("MensajeEncuesta");
        //                    }
        //                    else
        //                        createMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
        //                }
        //                else
        //                    createMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        if (dto.IdEncuestaEvaluador != 0)
        //            createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
        //        else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
        //    }
        //    TempData["EncuestaEvaluador"] = dto;
        //    return RedirectToAction("LlenarEncuesta");
        //}

        public ActionResult MensajeEncuesta()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            UsuariosBL usuariosBL = new UsuariosBL();
            UsuarioDTO currentUser = getCurrentUser();
            return View(usuariosBL.getUsuarios(currentUser.IdRol));//(CONSTANTES.ROL_RESPONSABLE));
        }

        public ActionResult Usuario(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            UsuarioDTO currentUser = getCurrentUser();
            if (!this.isAdministrator() && id != currentUser.IdUsuario) { return RedirectToAction("Index"); }
            if (id == 1 && !this.isSuperAdministrator()) { return RedirectToAction("Index"); }
            UsuariosBL usuariosBL = new UsuariosBL();
            IList<RolDTO> roles = usuariosBL.getRoles();
            //var rolesList = roles.ToList();
            roles.Insert(0, new RolDTO() { IdRol = 0, Nombre = "Seleccione un Rol" });
            ViewBag.Roles = roles;//.AsEnumerable();
            var objSent = TempData["Usuario"];
            if (objSent != null) { TempData["Usuario"] = null; return View(objSent); }
            if (id != null)
            {
                UsuarioDTO usuario = usuariosBL.getUsuario((int)id);
                return View(usuario);
            }
            return View();
        }

        public ActionResult AddUser(UsuarioDTO user, string passUser = "", string passChange = "")
        {

            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            UsuarioDTO currentUser = getCurrentUser();
            if (!this.isAdministrator() && user.IdUsuario != currentUser.IdUsuario) { return RedirectToAction("Index"); }
            if (user.IdUsuario == 1 && !this.isSuperAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                UsuariosBL usuariosBL = new UsuariosBL();
                if (user.IdUsuario == 0 && usuariosBL.validateUsuario(user))
                {
                    usuariosBL.add(user);
                    createResponseMessage(CONSTANTES.SUCCESS);
                    return RedirectToAction("Usuarios");
                }
                else if (user.IdUsuario != 0)
                {
                    if (usuariosBL.update(user, passUser, passChange, this.getCurrentUser()))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        if (user.IdUsuario == this.getCurrentUser().IdUsuario)
                        {
                            System.Web.HttpContext.Current.Session["User"] = usuariosBL.getUsuario(user.IdUsuario);
                            if (!this.getCurrentUser().Active) System.Web.HttpContext.Current.Session["User"] = null;
                        }
                        return RedirectToAction("Usuarios");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE + "<br>Si está intentando actualizar la contraseña, verifique que ha ingresado la contraseña actual correctamente.");
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch
            {
                if (user.IdUsuario != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Usuario"] = user;
            return RedirectToAction("Usuario");
        }
        public ActionResult GruposTrabajo()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            GrupoTrabajoBL grupoBL = new GrupoTrabajoBL();
            PersonaBL personaBL = new PersonaBL();
            EmpresaBL empresaBL = new EmpresaBL();

            ViewBag.Clientes = personaBL.getPersonas();
            ViewBag.Empresas = empresaBL.getEmpresas();

            return View(grupoBL.getGruposEvaluados());
        }
        public ActionResult GrupoTrabajo(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            if (id == 1 && !this.isSuperAdministrator()) { return RedirectToAction("Index"); }

            GrupoTrabajoBL grupoTrabajoBL = new GrupoTrabajoBL();
            if (id != null)
            {
                GrupoTrabajoDTO grupoT = grupoTrabajoBL.getGrupoTrabajo((int)id);
                return View(grupoT);
            }
            return View();
        }
        public ActionResult AddGrupoTrabajo(GrupoTrabajoDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            if (!this.isSuperAdministrator()) { return RedirectToAction("Index"); }

            try
            {
                GrupoTrabajoBL objBL = new GrupoTrabajoBL();
                if (dto.IdGrupoTrabajo == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("GruposTrabajo");
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else if (dto.IdGrupoTrabajo != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("GruposTrabajo");
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
            }
            catch
            {
                if (dto.IdGrupoTrabajo != 0) createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                    return RedirectToAction("GrupoTrabajo");
                }
            }
            TempData["GrupoTrabajo"] = dto;
            return RedirectToAction("GruposTrabajo");
        }
        public ActionResult Personas(int idGrupo)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            PersonaBL personaBL = new PersonaBL();
            if (idGrupo != null && idGrupo != 0)
                return View(personaBL.getPersonasPorGrupo(idGrupo));
            else
                return RedirectToAction("GruposTrabajo");
        }
        public ActionResult Persona(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            UsuarioDTO currentUser = getCurrentUser();
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            if (id == 1 && !this.isSuperAdministrator()) { return RedirectToAction("Index"); }

            GrupoTrabajoBL grupoTBL = new GrupoTrabajoBL();
            EmpresaBL empresaBL = new EmpresaBL();

            IList<GrupoTrabajoDTO> gruposTrabajo = grupoTBL.getGruposEvaluados();
            IList<EmpresaDTO> Empresas = empresaBL.getEmpresas();

            gruposTrabajo.Insert(0, new GrupoTrabajoDTO() { IdGrupoTrabajo = 0, Nombre = "Seleccione un grupo" });
            Empresas.Insert(0, new EmpresaDTO() { IdEmpresa = 0, Nombre = "Sin Empresa" });

            ViewBag.GruposTrabajo = gruposTrabajo;
            ViewBag.Empresas = Empresas;
            ViewBag.GruposTrabajoPersona = new List<GrupoTrabajoDTO>();

            PersonaBL PersonaBL = new PersonaBL();
            if (id != null)
            {
                PersonaDTO dto = PersonaBL.getPersona((int)id);
                ViewBag.GruposTrabajoPersona = dto.GruposTrabajo;
                return View(dto);
            }
            return View();
        }
        public ActionResult AddPersona(PersonaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            if (!this.isSuperAdministrator()) { return RedirectToAction("Index"); }

            try
            {
                PersonaBL objBL = new PersonaBL();
                if (dto.IdPersona == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("GruposTrabajo");
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else if (dto.IdPersona != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("GruposTrabajo");
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
            }
            catch
            {
                if (dto.IdPersona != 0) createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Persona"] = dto;
            return RedirectToAction("GruposTrabajo");
        }
        public ActionResult Empresa(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            UsuarioDTO currentUser = getCurrentUser();
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            if (id == 1 && !this.isSuperAdministrator()) { return RedirectToAction("Index"); }

            EmpresaBL empresaBL = new EmpresaBL();
            if (id != null)
            {
                EmpresaDTO dto = empresaBL.getEmpresa((int)id);
                return View(dto);
            }
            return View();
        }
        public ActionResult AddEmpresa(EmpresaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            if (!this.isSuperAdministrator()) { return RedirectToAction("Index"); }

            try
            {
                EmpresaBL objBL = new EmpresaBL();
                if (dto.IdEmpresa == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("GruposTrabajo");
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else if (dto.IdEmpresa != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("GruposTrabajo");
                    }
                    else
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                }
                else
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
            }
            catch
            {
                if (dto.IdEmpresa != 0) createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Persona"] = dto;
            return RedirectToAction("Personas");
        }

        public ActionResult Reportes()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            EncuestaBL encuestaBL = new EncuestaBL();
            PeriodoBL periodoBL = new PeriodoBL();

            IList<PeriodoDTO> lperiodos = periodoBL.getPeriodos(true);

            lperiodos.Insert(0, new PeriodoDTO() { IdPeriodo = 0, Descripcion = "Seleccione un periodo" });

            return View(lperiodos);
        }
        public ActionResult ReportesDetalle(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            EncuestaBL encuestaBL = new EncuestaBL();
            var model = new EncuestaDTO();
            if (id != null)
            {
                model = encuestaBL.getEncuestaReporteDetalle((int)id);
                Session["reporte"] = model;
            }
            return View(model);
        }

        public ActionResult ExportExcelDataReporte()
        {
            var data = (EncuestaDTO)Session["reporte"];

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            //int npre = data.listaReportePreguntas.Count;
            //int nper = data.listaReportePersonas.Count;
            dt.Columns.Add("Preguntas");

            foreach (var persona in data.listaReportePersonas)
                dt.Columns.Add(persona.Nombre);

            dt.Columns.Add("Promedio por pregunta");

            List<SeccionDTO> indices = new List<SeccionDTO>();

            int cont = 7;
            int tempIdSeccion = 0;

            //Tabla Preguntas y Evaluados Detalle
            //for (int pre = 0; pre < npre; pre++)
            foreach (var pregunta in data.listaReportePreguntas)
            {
                if (tempIdSeccion == 0)
                {
                    SeccionDTO nuevaSeccion = new SeccionDTO();
                    nuevaSeccion.Valor = cont;
                    //nuevaSeccion.Nombre = data.listaReportePreguntas[pre].NombreSeccion;
                    nuevaSeccion.Nombre = pregunta.NombreSeccion;
                    indices.Add(nuevaSeccion);
                    cont = 0;
                }
                else
                {
                    if (tempIdSeccion != 0 && tempIdSeccion != pregunta.IdSeccion && pregunta.IdTipoRespuesta == 3)//data.listaReportePreguntas[pre].IdSeccion)
                    {
                        SeccionDTO nuevaSeccion = new SeccionDTO();
                        nuevaSeccion.Valor = indices[indices.Count - 1].Valor + cont + 1;
                        //nuevaSeccion.Nombre = data.listaReportePreguntas[pre].NombreSeccion;
                        nuevaSeccion.Nombre = pregunta.NombreSeccion;
                        indices.Add(nuevaSeccion);

                        cont = 0;
                    }
                }

                tempIdSeccion = pregunta.IdSeccion;
                //tempIdSeccion = data.listaReportePreguntas[pre].IdSeccion;

                System.Data.DataRow row = dt.NewRow();
                //row[0] = data.listaReportePreguntas[pre].Texto;
                if(pregunta.IdTipoRespuesta == 3)
                    row["Preguntas"] = pregunta.Texto;

                //for (int j = 0; j < nper; j++)
                foreach (var evaluado in data.listaReportePersonas)
                {
                    var promedio = data.listaReporteDetalle.Where(x => x.IdPregunta == pregunta.IdPregunta && x.IdEvaluado == evaluado.IdPersona && x.IdTipoRespuesta == 3).Select(x => x.PromedioPreguntaXEvaluado).SingleOrDefault();
                    row[evaluado.Nombre] = promedio.ToString(CultureInfo.InvariantCulture);
                }
                if (pregunta.IdTipoRespuesta == 3)
                { 
                    //Agregar Promedio General x pregunta
                    row["Promedio por pregunta"] = pregunta.Promedio.ToString(CultureInfo.InvariantCulture);
                    dt.Rows.Add(row);
                }

                cont++;
            }

            //Seccion para los Promedios por Persona
            SeccionDTO aux = new SeccionDTO();
            aux.Valor = indices[indices.Count - 1].Valor + cont + 1;
            aux.Nombre = "Promedio por persona";
            indices.Add(aux);

            //Row de Promedios generales por persona
            System.Data.DataRow rowPromPer = dt.NewRow();
            rowPromPer["Preguntas"] = "Promedios Generales";
            //for(int i=0; i<nper; i++)
            foreach (var evaluado in data.listaReportePersonas)
            {
                rowPromPer[evaluado.Nombre] = evaluado.Promedio.ToString(CultureInfo.InvariantCulture);
            }
            dt.Rows.Add(rowPromPer);

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                AddSuperHeader(gv, "INFORME REPORTES 2014");
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PROMEDIO GENERAL : " + data.PromedioGeneral.ToString(CultureInfo.InvariantCulture));
                AddWhiteHeader(gv, 3, "CANTIDAD EVALUADORES : " + data.CantEvaluadores.ToString());
                AddWhiteHeader(gv, 4, "CANTIDAD ENCUESTAS RESUELTAS : " + data.CantEncuestasResueltas.ToString());
                AddWhiteHeader(gv, 5, "");

                foreach (var item in indices)
                {
                    AddHeader(gv, item.Valor, item.Nombre);
                }
                //Change the Header Row back to white color
                gv.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
                {
                    gv.HeaderRow.Cells[i].Style.Add("background-color", "#b8bbc2");
                }
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in gv.Rows)
                {
                    gvrow.BackColor = System.Drawing.Color.White;
                    gvrow.Style.Add("vertical-align", "middle");
                }
                //for para no mostrar el row de comentarios

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Reporte de Encuestas.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }

            return View(); //RedirectToAction("Procesos");
        }

        public ActionResult Pdf()
        {
            return View((EncuestaDTO)Session["reporte"]);
        }

        #region APIS
        [HttpPost]
        public ActionResult SaveEncuestaEvaluador(string encuestaEvaluador)
        {
            bool response = false;
            int IdEncuestaEvaluador = 0;
            try
            {
                EncuestaEvaluadorBL objBL = new EncuestaEvaluadorBL();
                var dto = new JavaScriptSerializer().Deserialize<EncuestaEvaluadorDTO>(encuestaEvaluador);
                IdEncuestaEvaluador = dto.IdEncuestaEvaluador;
                if (dto.IdEncuestaEvaluador == 0)
                {
                    IdEncuestaEvaluador = objBL.add(dto);
                    response = (IdEncuestaEvaluador != 0 ? true : false);
                }
                else
                    response = objBL.update(dto);
            }
            catch { response = false; }
            return Json(new { Response = response, Id = IdEncuestaEvaluador }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendEncuestaEvaluador(string encuestaEvaluador)
        {
            bool response = false;
            int IdEncuestaEvaluador = 0;
            try
            {
                EncuestaEvaluadorBL objBL = new EncuestaEvaluadorBL();
                var dto = new JavaScriptSerializer().Deserialize<EncuestaEvaluadorDTO>(encuestaEvaluador);
                var conta = dto.listaRespuestas.Where(x => x == "0").Count();
                if (conta == 0)
                {
                    IdEncuestaEvaluador = dto.IdEncuestaEvaluador;
                    if (dto.IdEncuestaEvaluador == 0)
                    {
                        IdEncuestaEvaluador = objBL.add(dto);
                        response = (IdEncuestaEvaluador != 0 ? true : false);
                    }
                    else
                        response = objBL.update(dto);
                    response = objBL.updateEstadoProcesoEncuesta(dto.IdEncuesta);
                }
            }
            catch { response = false; }
            return Json(new { Response = response, Id = IdEncuestaEvaluador }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendCorreoEncuesta(string grupoEvaluadores, int idEncuesta, int idGrupoEvaluado, string nombreEncuesta, string periodo)
        {
            EncuestaBL oBL = new EncuestaBL();
            var response = oBL.SendMailGrupo(grupoEvaluadores, idEncuesta, idGrupoEvaluado, nombreEncuesta, periodo);
            if (response) response = oBL.updateEstado(idEncuesta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SavePeriodo(string descripcion)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            PeriodoBL objBL = new PeriodoBL();
            PeriodoDTO oPeriodoDTO = new PeriodoDTO();
            oPeriodoDTO.Descripcion = descripcion;
            return Json(objBL.add(oPeriodoDTO), JsonRequestBehavior.AllowGet);
        }
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
        [HttpGet]
        public ActionResult GetEncuestasEnPeriodo(int idPeriodo)
        {
            EncuestaBL objBL = new EncuestaBL();
            var model = objBL.getEncuestaEnPeriodo(idPeriodo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region
        private static void AddSuperHeader(GridView gridView, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#cbcfd6");
            myTable.Rows.AddAt(0, myNewRow);
            //myTable.EnableViewState = false;
        }
        private static void AddHeader(GridView gridView, int index, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#cbcfd6");
            myNewRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            myTable.Rows.AddAt(index, myNewRow);
            //myTable.EnableViewState = false;
        }
        private static void AddWhiteHeader(GridView gridView, int index, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#ffffff");
            myNewRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            myTable.Rows.AddAt(index, myNewRow);
        }
        private static TableHeaderCell MakeCell(string text = null, int span = 1)
        {
            return new TableHeaderCell() { ColumnSpan = span, Text = text ?? string.Empty, CssClass = "table-header" };
        }
        #endregion
    }
}
