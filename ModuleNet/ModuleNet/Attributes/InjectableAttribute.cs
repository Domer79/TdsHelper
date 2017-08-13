using System;

namespace ModuleNet.ModuleNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute: Attribute
    {
        public InjectableAttribute() 
            : this((ModuleServiceLifetime) ModuleServiceLifetime.Singleton)
        {
        }

        public InjectableAttribute(ModuleServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }

        internal ModuleServiceLifetime ServiceLifetime { get; }

        internal Type ServiceType { get; set; }
    }

    public enum ModuleServiceLifetime
    {
        Singleton,
        Scoped,
        Transient,
    }
}
