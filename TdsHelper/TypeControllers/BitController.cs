﻿using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class BitController: BaseController
    {
        public BitController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"BOOLEAN";
        }
    }
}
