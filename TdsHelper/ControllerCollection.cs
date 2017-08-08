using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TdsHelper.Models;
using TdsHelper.TypeControllers;

namespace TdsHelper
{
    class ControllerCollection: IEnumerable<Type>
    {
        private readonly List<Type> _controllers = new List<Type>();

        public ControllerCollection()
        {
            LoadControllers();
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return _controllers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void LoadControllers()
        {
            var controllers = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => TypeExtensions.GetInterfaces(t).Contains(typeof(IController)));

            _controllers.AddRange(controllers);
        }

        public IController GetController(Column column)
        {
            var controllerName = $"{column.DataType}controller";

            var type = _controllers.FirstOrDefault(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
            if (type == null)
                return new Controller(column);

            return (IController) Activator.CreateInstance(type, column);
        }
    }
}