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
    public class PersonaBL : Base
    {
        public List<PersonaDTO> getPersonasPorGrupo(int idGrupo)//bool activeOnly = false
        {
            using (var context = getContext())
            {
                var result = context.Persona.Where(x => x.IdGrupoTrabajo == idGrupo).Select(x => new PersonaDTO
                {
                    IdPersona = x.IdPersona,
                    Nombre = x.Nombre,
                    Email = x.Email
                }).ToList();
                return result;
            }
        }

    }
}
