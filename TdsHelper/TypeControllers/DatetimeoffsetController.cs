using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class DatetimeoffsetController: BaseController
    {
        public DatetimeoffsetController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"TIMESTAMP({Column.DatetimePrecision}) WITH TIME ZONE";
        }
    }
}
