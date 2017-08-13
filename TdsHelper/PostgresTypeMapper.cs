using System;
using System.Collections.Generic;
using System.Text;
using ModuleNet.ModuleNet.Attributes;
using TdsHelper.Models;

namespace TdsHelper
{
    [Injectable]
    public class PostgresTypeMapper
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
