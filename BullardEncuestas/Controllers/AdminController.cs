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
            List<EncuestaDTO> model = new List<EncuestaDTO>();
            for (int i = 0; i < 3; i++)
            {
                EncuestaDTO nuevo = new EncuestaDTO();
                nuevo.IdEncuesta = i;
                nuevo.IdPeriodo = 201400 + i;
                //nuevo.NombreEncuesta = "Profesional" + i.ToString();
                switch (i)
                {
                    case 0:
                        nuevo.NombreEncuesta = "Profesional";
                        break;
                    case 1:
                        nuevo.NombreEncuesta = "Practicante";
                        break;
                    case 2:
                        nuevo.NombreEncuesta = "Satisfaccion al cliente";
                        break;
                }
                /*Profesional - practicante - satisfaccion al cliente
                 */
                nuevo.Estado = true;
                PeriodoDTO nuevop = new PeriodoDTO();
                nuevop.IdPeriodo = i;
                nuevop.Descripcion = "2014";
                nuevo.Periodo = nuevop;
                model.Add(nuevo);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Encuesta(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            //-EncuestaBL objBL = new EncuestaBL();
            ViewBag.IdEncuesta = id;
            //-var objSent = TempData["Encuesta"];
            //-if (objSent != null) { TempData["Encuesta"] = null; return View(objSent); }
            List<SeccionDTO> listSeccion = GenerarEncuestaPrueba();
            ViewBag.EncuestaPeriodo = "2014-1";
            ViewBag.EncuestaNombre = "Satisfaccion al cliente";
            //if (id != null)
            //{
                //-ViewBag.listaEncuestaEvaluador = objBL.getEncuestasEvaluador((int)id);
                //-EncuestaDTO obj = objBL.getEncuesta((int)id);
                EncuestaDTO nuevo = new EncuestaDTO();
                nuevo.IdEncuesta = 1;
                nuevo.IdPeriodo = 201401;
                nuevo.NombreEncuesta = "Satisfaccion al cliente";
                nuevo.Estado = true;
                PeriodoDTO nuevop = new PeriodoDTO();
                nuevop.IdPeriodo = 1;
                nuevop.Descripcion = "2014-1";
                nuevo.Periodo = nuevop;
                nuevo.Secciones = listSeccion;

                return View(nuevo);
            //}


            //return View(listSeccion);
        }
        public ActionResult SendCorreo(int idEncuesta) {
            EncuestaBL oBL = new EncuestaBL();
            oBL.SendMailGrupo(idEncuesta);
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
                    nuevaP.Orden = j;
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
    }
}
