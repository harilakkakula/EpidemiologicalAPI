using Epidemiological.DAL.Interface;
using Epidemiological.DAL.Model;
using Epidemiological.DAL.ViewModels;
using FakeItEasy;


namespace EpidemiologicalApp.Test
{
    public class WeeklyInfectusDiseasesTests
    {
        private readonly IWeeklyInfectusDiseases _weeklyInfectusDiseases;

        public WeeklyInfectusDiseasesTests()
        {
            _weeklyInfectusDiseases = A.Fake<IWeeklyInfectusDiseases>();
        }

       

        [Fact]
        public async Task GetAsync_ShouldReturnListOfWeeklyInfectusDiseases()
        {
            // Arrange
            var fakeData = new List<WeeklyInfectusDiseases> { new WeeklyInfectusDiseases() }; // Fix: Return a Task wrapped inside a ValueTask (to match the method's return type)
            A.CallTo(() => _weeklyInfectusDiseases.GetAsync()).Returns(Task.FromResult(fakeData)); // Ensure it returns Task<List<WeeklyInfectusDiseases>>


            // Act
            var result = await _weeklyInfectusDiseases.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task FetchAndInsert_ShouldBeCalledOnce()
        {
            // Act
            await _weeklyInfectusDiseases.FetchAndInsert();

            // Assert
            A.CallTo(() => _weeklyInfectusDiseases.FetchAndInsert()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAsync_WithYearAndMonth_ShouldReturnInfectusDiseasesResponse()
        {
            // Arrange
            int year = 2025, month = 2;
            var fakeResponse = getInfectusDiseasesResponseFake();
            A.CallTo(() => _weeklyInfectusDiseases.GetAsync(year, month)).
                Returns(Task.FromResult<IEnumerable<InfectusDiseasesResponse?>>(fakeResponse));

            // Act
            var result = await _weeklyInfectusDiseases.GetAsync(year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        #region Data Prep
        private  List<InfectusDiseasesResponse> getInfectusDiseasesResponseFake()
        {

            var fakeResponse = new List<InfectusDiseasesResponse?> {
                new InfectusDiseasesResponse() 
                { 
                    Description="21",
                    Name="Acute Viral hepatitis B",
                    End="2015-02-01",
                    Start="2015-02-07"
                } 
            };

            return fakeResponse;
        }
        #endregion
    }


}
