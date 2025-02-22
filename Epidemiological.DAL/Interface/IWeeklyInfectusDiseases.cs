using Epidemiological.DAL.Model;
using Epidemiological.DAL.ViewModels;

namespace Epidemiological.DAL.Interface
{
    public interface IWeeklyInfectusDiseases
    {
        Task FetchAndInsert();
        Task<List<WeeklyInfectusDiseases>> GetAsync();
        Task<IEnumerable<InfectusDiseasesResponse?>> GetAsync(int year, int month);
    }
}
