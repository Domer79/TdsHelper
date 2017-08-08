using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class Datetime2Controller: BaseController
    {
        public Datetime2Controller(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"TIMESTAMP({Column.DatetimePrecision})";
        }
    }
}
