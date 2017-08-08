using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class FloatController: BaseController
    {
        public FloatController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"DOUBLE PRECISION";
        }
    }
}
