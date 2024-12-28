using System.Diagnostics.Metrics;
using AirportDictionaryApp_v1.Model;
using AirportDictionaryApp_v1.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryApp_v1.Api
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        // сервис для работы со странами
        private readonly CountryService _countries;

        public CountryController(CountryService countries)
        {
            _countries = countries;
        }

        [HttpGet]
        public async Task<List<CountryMessage>> ListAllAsync()
        {
            List<Country> countries = await _countries.ListAllAsync();
            return countries
                .Select(c => new CountryMessage(Name: c.Name, Code: c.Code))
                .ToList();
        }

        [HttpPut]
        public async Task<IActionResult> ImportAsync(List<CountryMessage> countries)
        {
            List<Country> imported = countries
                .Select(c => new Country() { Name = c.Name, Code = c.Code })
                .ToList();
            await _countries.ImportAsync(imported);
            // 204
            return NoContent();
        }

        // получить список аэропортов страны (задана по коду)
        [HttpGet("{code:alpha}/airports")]
        public async Task<IActionResult> GetAirportsByCountryCodeAsync(string code)
        {
            int? countryId = await _countries.GetIdByCode(code);
            if (countryId == null)
            {
                return NotFound(new ErrorMessage(Type: "CountryNotFound", Message: $"Country with code '{code}' not found"));
            }

            var airports = await _countries.GetAirportsByCountryIdAsync(countryId.Value);

            var result = airports.Select(a => new AirportListItemMessage(
                Id: a.Id,
                Name: a.Name,
                Code: a.Code,
                Location: a.Location,
                CountryCode: code
            )).ToList();

            return Ok(result);
        }

        // очистить данные всех стран с аэропортами
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAllCountriesAndAirportsAsync()
        {
            try
            {
                // Вызываем метод сервиса для очистки данных
                await _countries.ClearAllCountriesAndAirportsAsync();

                // Возвращаем успешный ответ (204 No Content)
                return NoContent();
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем внутреннюю ошибку
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorMessage("ServerError", $"An error occurred while clearing data: {ex.Message}"));
            }
        }


    }
}
