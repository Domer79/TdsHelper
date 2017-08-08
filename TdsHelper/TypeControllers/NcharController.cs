using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class NcharController: BaseController
    {
        public NcharController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"CHAR({Column.CharacterMaximumLength})";
        }
    }
}
