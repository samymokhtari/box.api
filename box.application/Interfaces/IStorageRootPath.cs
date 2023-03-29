namespace box.application.Interfaces
{
    public interface IStorageRootPath
    {
        string RootPath { get; }

        char DirectorySeparator { get; }
    }
}