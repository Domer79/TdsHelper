using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class CharacterController: BaseController
    {
        public CharacterController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"CHARACTER({Column.CharacterMaximumLength})";
        }
    }
}
