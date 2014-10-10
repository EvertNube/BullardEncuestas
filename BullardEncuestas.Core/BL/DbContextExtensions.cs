using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using BullardEncuestas.Helpers;
using System.Data.Objects;

namespace BullardEncuestas.Core
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Execute stored procedure with single table value parameter.
        /// </summary>
        /// <typeparam name="T">Type of object to store.</typeparam>
        /// <param name="context">DbContext instance.</param>
        /// <param name="data">Data to store</param>
        /// <param name="procedureName">Procedure name</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="typeName">User table type name</param>
        public static int ExecuteTableValueProcedure(this DbContext context, string procedureName, string paramsName, params object[] parameters)
        {
            //// execute sp sql
            string sql = String.Format("EXEC {0} {1};", procedureName, paramsName);
            //var data = context.Database.SqlQuery<String>("exec @ReturnCode = spItemData @Code, @StatusLog OUT", returnCode, code, outParam);
            return context.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
