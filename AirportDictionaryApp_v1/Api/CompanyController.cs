using AirportDictionaryApp_v1.Model;
using AirportDictionaryApp_v1.Service;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryApp_v1.Api
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        // используемые сервисы
        private readonly AirportService _airports;
        private readonly CountryService _countries;
        private readonly CompanyService _companies;

        public CompanyController(AirportService airports, CountryService countries, CompanyService companies)
        {
            _airports = airports;
            _countries = countries;
            _companies = companies;
        }

        // получить список всех авиакомпаний
        [HttpGet]
        public async Task<List<CompanyMessage>> GetAllAsync()
        {
            List<Company> companies = await _companies.ListAllAsync();
            return companies.Select(company => new CompanyMessage(Id: company.Id, Name: company.Name)).ToList();
        }

        // получить авиакомпанию по id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Company? company = await _companies.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound(new ErrorMessage(Type: "CompanyNotFound", Message: $"Company with id '{id}' not found"));
            }

            var result = new CompanyMessage(Id: company.Id, Name: company.Name);
            return Ok(result);
        }

        // получить список аэропортов, в которых присутствует заданная авиакомпания
        [HttpGet("{companyId:int}/airports")]
        public async Task<IActionResult> GetAirportsByCompanyIdAsync(int companyId)
        {
            var airports = await _companies.GetAirportsByCompanyIdAsync(companyId);
            if (airports == null)
            {
                return NotFound(new ErrorMessage(Type: "CompanyNotFound", Message: $"Company with id '{companyId}' not found"));
            }

            var result = airports.Select(airport => new AirportListItemMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                Location: airport.Location,
                CountryCode: airport.Country != null ? airport.Country.Code : string.Empty
            )).ToList();

            return Ok(result);
        }

        // добавить авиакомпанию (без привязки к аэропортам)
        [HttpPost]
        public async Task<IActionResult> AddCompanyAsync(AddCompanyMessage companyMessage)
        {
            if (await _companies.IsExists(companyMessage.Name))
            {
                return Conflict(new ErrorMessage(Type: "DuplicateCompany", Message: $"Company with name '{companyMessage.Name}' already exists"));
            }

            var company = new Company
            {
                Name = companyMessage.Name
            };

            await _companies.AddAsync(company);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = company.Id }, new CompanyMessage(Id: company.Id, Name: company.Name));
        }

        // удалить авиакомпанию
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCompanyAsync(int id)
        {
            if (!await _companies.IsExists(id))
            {
                return NotFound(new ErrorMessage(Type: "CompanyNotFound", Message: $"Company with id '{id}' not found"));
            }

            await _companies.DeleteAsync(id);
            return NoContent();
        }

    }
}
