namespace ModuleNet.Abstractions
{
    public interface IModule
    {
        void Act(params object[] args);
    }
}
