using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class BinaryController: BaseController
    {
        public BinaryController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"BYTEA";
        }
    }
}
