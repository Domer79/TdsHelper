using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class IntController: BaseController
    {
        public IntController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"INT";
        }
    }
}
