namespace box.application.Interfaces
{
    public interface IStorageRootPath
    {
        string RootPath { get; }

        static char DirectorySeparator { get; }
    }
}