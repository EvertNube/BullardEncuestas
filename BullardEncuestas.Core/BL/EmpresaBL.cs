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
    public class EmpresaBL : Base
    {
        public List<EmpresaDTO> getEmpresas()//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public IList<EmpresaDTO> getEmpresasIList()//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public EmpresaDTO getEmpresa(int id)
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Where(x => x.IdEmpresa == id)
                    .Select(r => new EmpresaDTO
                    {
                        IdEmpresa = r.IdEmpresa,
                        Nombre = r.Nombre,
                        Estado = r.Estado
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(EmpresaDTO empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    Empresa nuevo = new Empresa();
                    nuevo.Nombre = empresa.Nombre;
                    nuevo.Estado = empresa.Estado;
                    context.Empresa.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(EmpresaDTO empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var empre = context.Empresa.Where(x => x.IdEmpresa == empresa.IdEmpresa).SingleOrDefault();
                    empre.Nombre = empresa.Nombre;
                    empre.Estado = empresa.Estado;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
