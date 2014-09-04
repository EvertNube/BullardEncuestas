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
    public class ReporteBL : Base
    {
        public List<ReporteDTO> ObtenerReportesEncuestas()
        {
            using (var context = getContext())
            {
                var result = context.SP_GetEncuestasReporte()
                    .Select(x => new ReporteDTO
                    {
                        IdEncuesta = x.IdEncuesta,
                        NombreEncuesta = x.NombreEncuesta,
                        Estado = x.Estado,
                        IdPeriodo = x.IdPeriodo,
                        NombrePeriodo = x.NombrePeriodo,
                        IdGrupo = x.IdGrupoTrabajo,
                        NombreGrupo = x.NombreGrupo,
                        //PromedioGeneral = x.PromedioGeneral
                        //s
                    }
                    ).ToList();

                //var result = context.SP_GetEncuestasReporte()
                //    .Select(x => new ReporteDTO
                //    {
                //        IdEncuesta = x.IdEncuesta,
                //        NombreEncuesta = x.NombreEncuesta,
                //        IdPeriodo = x.IdPeriodo,
                //        NombrePeriodo = x.NombrePeriodo,
                //        IdGrupo = x.IdGrupoTrabajo,
                //        NombreGrupo = x.NombreGrupo,
                //    }
                //    ).ToList();

                //var resultSeccion = new List<ReporteSeccionValorDTO>();
                //foreach (var item in result)
                //{
                //    resultSeccion = context.SP_GetEncuestasReporteSecciones(item.IdEncuesta, item.IdPeriodo, item.IdGrupo)
                //    .Select(y => new ReporteSeccionValorDTO
                //    {
                //        IdSeccion = y.IdSeccion,
                //        OrdenSeccion = y.OrdenSeccion,
                //        IdPregunta = y.IdPregunta,
                //        OrdenPregunta = y.IdPregunta,
                //        IdRespuesta = y.IdRespuestas,
                //        IdTipoRespuesta = y.IdTipoRespuesta,
                //        IdOpcionRespuesta = y.IdOpcion,
                //        Valor = y.Valor
                //    }).ToList();

                //    item.ListaSeccionProm = resultSeccion;
                //}



                return result;
            }

        }


    }
}
