using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class DecimalController: BaseController
    {
        public DecimalController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"DECIMAL({Column.NumericPrecision},{Column.NumericScale})";
        }
    }
}
