using Newtonsoft.Json;
using System.Text.Json;

namespace Epidemiological.DAL.ViewModels
{
    public class APIResponse
    {
        public Result result { get; set; }
    }
    public class Field
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Record
    {
        public int _id { get; set; }
        public string epi_week { get; set; }
        public string disease { get; set; }

        [JsonProperty("no._of_cases")]
        public string no_of_cases { get; set; }
    }

    public class Result
    {
        public string resource_id { get; set; }
        public List<Field> fields { get; set; }
        public List<Record> records { get; set; }
    }

}
