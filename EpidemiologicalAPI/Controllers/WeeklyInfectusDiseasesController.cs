using Epidemiological.DAL.Interface;
using Epidemiological.DAL.Model;
using Epidemiological.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EpidemiologicalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyInfectusDiseasesController : ControllerBase
    {
        private readonly IWeeklyInfectusDiseases _service;

        public WeeklyInfectusDiseasesController(IWeeklyInfectusDiseases service) =>
            _service = service;

        [HttpGet]
        public async Task<List<WeeklyInfectusDiseases>> Get() =>
            await _service.GetAsync();

        [HttpGet("getByYear")]
        public async Task<ActionResult<IEnumerable<InfectusDiseasesResponse>>> Get(int year,int month)
        {
            var book = await _service.GetAsync(year, month);

            if (book is null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// Get the data from Dataset and insert into Mongo DB
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _service.FetchAndInsert();

            return Ok();
        }

    }
   }
