namespace NH.Entity.Interfaces
{
    public interface IDataInitializationService
    {
        void InitializeDatabase();
        void TruncateAllTables();
        void CreateSampleData();
    }
} 