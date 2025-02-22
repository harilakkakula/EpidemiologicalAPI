using Epidemiological.DAL.Interface;
using Epidemiological.DAL.Model;
using Epidemiological.DAL.Util;
using Epidemiological.DAL.ViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestSharp;

namespace Epidemiological.DAL.Services
{
    public class WeeklyInfectusDiseasesImpl : IWeeklyInfectusDiseases
    {
        private readonly IMongoCollection<WeeklyInfectusDiseases> _collection;
        private  APIIntegrationDetails _integrationSettings=new APIIntegrationDetails();
        public WeeklyInfectusDiseasesImpl(IOptions<StoreDatabaseSettings> StoreDatabaseSettings,
            IOptions<APIIntegrationDetails> integrationSettings)
        {
            var mongoClient = new MongoClient(
                StoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                StoreDatabaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<WeeklyInfectusDiseases>(
                StoreDatabaseSettings.Value.CollectionName);
            _integrationSettings.Api = integrationSettings.Value.Api;
            _integrationSettings.EndPoint = integrationSettings.Value.EndPoint;
        }
        public async Task FetchAndInsert()
        {
            if(string.IsNullOrWhiteSpace(_integrationSettings.EndPoint))
            {
                throw new ArgumentNullException("Unable to read the connection settings");
            }
            else
            {
                var options = new RestClientOptions(_integrationSettings.EndPoint)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(_integrationSettings.Api, Method.Get);
                RestResponse response = await client.ExecuteAsync(request);
                var serilize = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponse>(response.Content);
                Console.WriteLine(response);

                List<WeeklyInfectusDiseases> books = new List<WeeklyInfectusDiseases>();
                foreach (var book in serilize.result.records)
                {
                    var spiltYear = book.epi_week.Split("-W");
                    int year = Convert.ToInt32(spiltYear[0]);
                    int week = Convert.ToInt32(spiltYear[1]);
                    var fromAndTo = GetIsoWeekDates(year, week);
                    books.Add(new WeeklyInfectusDiseases()
                    {
                        disease = book.disease,
                        epi_week = book.epi_week,
                        _dataid = book._id,
                        No_of_cases = book.no_of_cases,
                        from = fromAndTo.Item1,
                        to = fromAndTo.Item2,
                        year = year,
                        MonthName = fromAndTo.Item1.ToString("MMMM"),
                        month = Convert.ToInt32(fromAndTo.Item1.ToString("MM"))
                    });
                }
                try
                {
                    await _collection.InsertManyAsync(books);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            
        }

        public async Task<List<WeeklyInfectusDiseases>> GetAsync()
        {
            var result= await _collection.Find(x => x.Id != null).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<InfectusDiseasesResponse?>> GetAsync(int year, int month)
        {
            var gtrm = new List<InfectusDiseasesResponse>();
            var data = await _collection.Find(x => x.year == year && x.month == month).ToListAsync();
            MapResponse(gtrm, data);
            return gtrm;
        }

        public static void MapResponse(List<InfectusDiseasesResponse> gtrm, List<WeeklyInfectusDiseases> data)
        {
            foreach (var item in data.OrderBy(x => x.disease))
            {
                if (Convert.ToInt32(item.No_of_cases) > 0)
                {
                    var model = new InfectusDiseasesResponse();
                    model.Name = item.disease;
                    model.Start = item.from.ToString("yyyy-MM-dd");
                    model.End = item.to.ToString("yyyy-MM-dd");
                    model.Description = item.No_of_cases;
                    gtrm.Add(model);
                }

            }
        }

        static (DateTime, DateTime) GetIsoWeekDates(int year, int week)
        {
          
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            if (daysOffset < 0)
                daysOffset += 7;

            DateTime firstThursday = jan1.AddDays(daysOffset);

            
            DateTime firstWeekStart = firstThursday.AddDays(-3);

            
            DateTime startDate = firstWeekStart.AddDays((week - 1) * 7);
            DateTime endDate = startDate.AddDays(6); 

            return (startDate, endDate);
        }

    }
}
