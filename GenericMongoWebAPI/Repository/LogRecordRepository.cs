using GenericMongoWebAPI.IRepository;
using GenericMongoWebAPI.Model;
using MongoDB.Driver;

namespace GenericMongoWebAPI.Repository
{
    public class LogRecordRepository : ILogRecordRepository
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _database = null;
        private IMongoCollection<LogRecord> _logRecords = null;
        private IConfiguration _configuration;

        public LogRecordRepository(IConfiguration _configuration)
        {
            //------------------------------------
            this._configuration = _configuration;
            string ConString = this._configuration.GetValue<string>("MySettings:DbConnection");
            string DatabaseName = this._configuration.GetValue<string>("MySettings:Database");

            _mongoClient = new MongoClient(ConString);
            _database = _mongoClient.GetDatabase(DatabaseName);

            //------------------------------------


            ////------------------------------
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //IConfiguration _configuration = builder.Build();
            //var myConnectionString = _configuration.GetConnectionString("DefaultConnection");

            //_mongoClient = new MongoClient(myConnectionString);
            ////_database = _mongoClient.GetDatabase(myConnectionString.Split('/')[1]);

            ////------------------------------

            //_mongoClient = new MongoClient("mongodb://localhost:27017");
            //_database = _mongoClient.GetDatabase("OfficeDB");
            _logRecords = _database.GetCollection<LogRecord>("LogRecords");
        }

        public string Delete(string logId)
        {
            _logRecords.DeleteOne(x => x.Id == logId);
            return "Deleted";
        }

        public LogRecord Get(string logId)
        {
            return _logRecords.Find(x => x.Id == logId).FirstOrDefault();
        }

        public IEnumerable<LogRecord> GetAll()
        {
            return _logRecords.Find(FilterDefinition<LogRecord>.Empty).ToList();
        }

        public LogRecord Save(LogRecord logRecord)
        {
            var Obj = _logRecords.Find(x => x.Id == logRecord.Id).FirstOrDefault();
            if (Obj == null)
            {
                _logRecords.InsertOne(logRecord);
            }
            else { _logRecords.ReplaceOne(x => x.Id == logRecord.Id, logRecord); }
            return logRecord;

        }
    }
}
