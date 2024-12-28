using AirportDictionaryApp_v1.Model;
using AirportDictionaryApp_v1.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryApp_v1.Api
{
    [Route("api/airport")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        // используемые сервисы
        private readonly AirportService _airports;
        private readonly CountryService _countries;

        public AirportController(AirportService airports, CountryService countries)
        {
            _airports = airports;
            _countries = countries;
        }

        [HttpGet]
        public async Task<List<AirportListItemMessage>> GetAllAsync()
        {
            // получить аэропорты и страны
            List<Airport> airports = await _airports.ListAllAsync();
            List<Country> countries = await _countries.ListAllAsync();

            // преобразовать список стран в словарь с ключами-id и значениями-кодами
            Dictionary<int, string> countryCodeById =
                countries.ToDictionary(
                    country => country.Id,
                    country => country.Code
                );

            // собрать список сообщений со аэропортами
            return airports.Select(airport => new AirportListItemMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                Location: airport.Location,
                CountryCode: countryCodeById[airport.CountryId]
            )).ToList();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Airport? airport = await _airports.GetAsync(id);
            if (airport == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"airport with id '{id}' not found"));
            }
            // 200
            AirportMessage result = new AirportMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                OpeningYear: airport.OpeningYear,
                RunwayCount: airport.RunwayCount,
                AnnualPassengerTraffic: airport.AnnualPassengerTraffic,
                Location: airport.Location,
                CountryId: airport.CountryId,
                Country: new CountryMessage(Name: airport.Country!.Name, Code: airport.Country!.Code)
            );
            return Ok(result);
        }

        [HttpGet("{code:alpha}")]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            Airport? airport = await _airports.GetAsync(code);
            if (airport == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"airport with code '{code}' not found"));
            }
            // 200
            AirportMessage result = new AirportMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                OpeningYear: airport.OpeningYear,
                RunwayCount: airport.RunwayCount,
                AnnualPassengerTraffic: airport.AnnualPassengerTraffic,
                Location: airport.Location,
                CountryId: airport.CountryId,
                Country: new CountryMessage(Name: airport.Country!.Name, Code: airport.Country!.Code)
            );
            return Ok(result);
        }

        // добавить аэропорт (с указанием страны по коду)
        [HttpPost]
        public async Task<IActionResult> PostAsync(AddAirportMessage airportMessage)
        {
            int? countryId = await _countries.GetIdByCode(airportMessage.CountryCode);
            if(countryId == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "CountryNotFoud", Message: $"country with code '{airportMessage.CountryCode}' not found"));
            }
            if (await _airports.IsExists(airportMessage.Code))
            {
                // 409
                return Conflict(new ErrorMessage(Type: "DuplicatedAirportCode", Message: $"airport with code '{airportMessage.Code}' already exists"));
            }
            Airport airport = new Airport()
            {
                Name = airportMessage.Name,
                Code = airportMessage.Code,
                OpeningYear = airportMessage.OpeningYear,
                RunwayCount = airportMessage.RunwayCount,
                AnnualPassengerTraffic = airportMessage.AnnualPassengerTraffic,
                Location = airportMessage.Location,
                CountryId = countryId.Value
            };
            await _airports.AddAsync(airport);
            return Created();
        }

        // удалить аэропорт по коду 
        [HttpDelete("{code:alpha}")]
        public async Task<IActionResult> DeleteByCodeAsync(string code)
        {
            if (await _airports.IsExists(code) == false)
            {
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"airport with id '{code}' not found"));
            }
            await _airports.DeleteAsync(code);
            return NoContent();
        }

        // обновить среднегодовой пассажиропоток аэропорта
        [HttpPatch("{id:int}/annual-passenger-traffic")]
        public async Task<IActionResult> UpdateAnnualPassengerTrafficAsync(int id, [FromBody] long newAnnualPassengerTraffic)
        {
            if (newAnnualPassengerTraffic < 0)
            {
                return BadRequest(new ErrorMessage(Type: "InvalidPassengerTraffic", Message: "Annual passenger traffic cannot be negative"));
            }

            bool isUpdated = await _airports.UpdateAnnualPassengerTrafficAsync(id, newAnnualPassengerTraffic);
            if (!isUpdated)
            {
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"Airport with id '{id}' not found"));
            }

            return NoContent(); // 204
        }

        // получить список авиакомпаний, присутствующих в аэропорте
        [HttpGet("{id:int}/companies")]
        public async Task<IActionResult> GetCompaniesByAirportIdAsync(int id)
        {
            var companies = await _airports.GetCompaniesByAirportIdAsync(id);
            if (companies == null)
            {
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"Airport with id '{id}' not found"));
            }

            var result = companies.Select(company => new CompaniesByAirportIdMessage
            {
                Id = company.Id,
                Name = company.Name
            }).ToList();

            return Ok(result);
        }

        // добавить авиакомпанию по id в обслуживание данным аэропортом по id
        [HttpPost("{airportId:int}/companies/{companyId:int}")]
        public async Task<IActionResult> AddCompanyToAirportAsync(int airportId, int companyId)
        {
            bool result = await _airports.AddCompanyToAirportAsync(airportId, companyId);

            if (!result)
            {
                return NotFound(new ErrorMessage(Type: "NotFound", Message: $"Either airport with id '{airportId}' or company with id '{companyId}' not found"));
            }

            return NoContent(); // 204
        }

        // удалить авиакомпанию по id в обслуживание данным аэропортом по id
        [HttpDelete("{airportId:int}/companies/{companyId:int}")]
        public async Task<IActionResult> RemoveCompanyFromAirportAsync(int airportId, int companyId)
        {
            bool result = await _airports.RemoveCompanyFromAirportAsync(airportId, companyId);

            if (!result)
            {
                return NotFound(new ErrorMessage(Type: "NotFound", Message: $"Either airport with id '{airportId}' or company with id '{companyId}' not found, or the company is not associated with the airport"));
            }

            return NoContent(); // 204
        }

    }
}
