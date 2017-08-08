using System;
using TdsHelper.Abstractions;
using TdsHelper.Models;

namespace TdsHelper.TypeControllers
{
    abstract class BaseController : IController
    {
        public Column Column { get; }

        protected BaseController(Column column)
        {
            Column = column ?? throw new ArgumentNullException(nameof(column));
        }

        public abstract string ToPostgresTypeString();
    }
}