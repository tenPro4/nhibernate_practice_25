namespace NH.Service.Services.Abstract
{
    public interface IDataInitializationService
    {
        void InitializeDatabase();
        void TruncateAllTables();
        void CreateSampleData();
    }
} 