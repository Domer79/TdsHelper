using System;
using System.Linq;
using System.Reflection;

namespace ModuleNet.ModuleNet
{
    internal class InternalService
    {
        private Type[] _allTypes;
        private static InternalService _internalService;

        public Type[] GetAllTypes()
        {
            return _allTypes ?? (_allTypes = Assembly.GetEntryAssembly().GetReferencedAssemblies().SelectMany(a => Assembly.Load(a).GetTypes()).Union(Assembly.GetEntryAssembly().GetTypes()).ToArray());
        }

        public static InternalService Instance => _internalService ?? (_internalService = new InternalService());
    }
}
