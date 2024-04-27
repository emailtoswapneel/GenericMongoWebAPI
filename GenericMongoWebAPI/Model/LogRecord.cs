using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GenericMongoWebAPI.Model
{
    public class LogRecord
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string LogEvent { get; set; } = "";
        public string LogTimeStamp { get; set; } = System.DateTime.Now.ToString();        
        public string LogMessage { get; set; } = "";
    }
}
