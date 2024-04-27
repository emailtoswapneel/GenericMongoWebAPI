using GenericMongoWebAPI.Model;

namespace GenericMongoWebAPI.IRepository
{
    public interface ILogRecordRepository
    {
        LogRecord Save(LogRecord logRecord);
        LogRecord Get(string logId);
        IEnumerable<LogRecord> GetAll();
        string Delete(string logId);
    }
}
