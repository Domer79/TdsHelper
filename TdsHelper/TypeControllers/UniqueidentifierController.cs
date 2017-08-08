using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class UniqueidentifierController: BaseController
    {
        public UniqueidentifierController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return "UUID";
        }
    }
}
