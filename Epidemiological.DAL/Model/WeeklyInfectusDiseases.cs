using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Epidemiological.DAL.Model
{
    public class WeeklyInfectusDiseases
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? epi_week { get; set; }

        [BsonElement("disease")]
        public string disease { get; set; } = null!;

        [BsonElement("no._of_cases")]
        public string? No_of_cases { get; set; }

        public int _dataid { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public int year { get; set; }
        public string? MonthName { get; set; }
        public int month { get; set; }
    }
}
