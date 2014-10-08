using BullardEncuestas.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullardEncuestas.Helpers;

namespace BullardEncuestas.Core
{
    public class Base
    {
        protected BULLARDEncuestasEntities getContext()
        {
            return new BULLARDEncuestasEntities();
        }
        protected string getHost()
        {
            Uri uri = System.Web.HttpContext.Current.Request.Url;
            string host = uri.GetLeftPart(UriPartial.Authority);
            return host;
        }
        ///// <summary>
        ///// Execute stored procedure with single table value parameter.
        ///// </summary>
        ///// <typeparam name="T">Type of object to store.</typeparam>
        ///// <param name="context">DbContext instance.</param>
        ///// <param name="data">Data to store</param>
        ///// <param name="procedureName">Procedure name</param>
        ///// <param name="paramName">Parameter name</param>
        ///// <param name="typeName">User table type name</param>
        //public static int ExecuteTableValueProcedure<T>(this BULLARDEncuestasEntities context, IEnumerable<T> data, string procedureName, string paramName, string typeName)
        //{
        //    //// convert source data to DataTable
        //    DataTable table = data.ToDataTable();

        //    //// create parameter
        //    SqlParameter parameter = new SqlParameter(paramName, table);
        //    parameter.SqlDbType = SqlDbType.Structured;
        //    parameter.TypeName = typeName;

        //    //// execute sp sql
        //    string sql = String.Format("EXEC {0} {1};", procedureName, paramName);

        //    //// execute sql
        //    return context.Database.ExecuteSqlCommand(sql, parameter);
        //}
    }
}
