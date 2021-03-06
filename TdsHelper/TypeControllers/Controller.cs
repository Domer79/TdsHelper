﻿using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class Controller : BaseController
    {
        public Controller(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return Column.DataType.ToUpper();
        }
    }
}
