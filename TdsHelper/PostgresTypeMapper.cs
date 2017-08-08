using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper
{
    class PostgresTypeMapper
    {
        private readonly ControllerFactory _controllerFactory;

        public PostgresTypeMapper(ControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        public string ToPostgresColumnString(Column column)
        {
            return $"{column.ColumnName} {_controllerFactory.GetPostgresDataType(column)}";
        }
    }
}
