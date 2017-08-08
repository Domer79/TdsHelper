using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class DatetimeController: BaseController
    {
        public DatetimeController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"TIMESTAMP(3)";
        }
    }
}
