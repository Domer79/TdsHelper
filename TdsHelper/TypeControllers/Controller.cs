using System;
using System.Collections.Generic;
using System.Text;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    class Controller : IController
    {
        public Controller(Column column)
        {
            Column = column ?? throw new ArgumentNullException(nameof(column));
        }

        public Column Column { get; }

        public virtual string ToPostgresTypeString()
        {
            return Column.DataType;
        }
    }

    class CharController: Controller
    {
        public CharController(Column column) : base(column)
        {
        }

        public override string ToPostgresTypeString()
        {
            return $"char({Column.CharacterMaximumLength})";
        }
    }

    internal interface IController
    {
        string ToPostgresTypeString();
    }
}
