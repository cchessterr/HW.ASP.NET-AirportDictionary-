using AirportDictionaryApp_v1.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryApp_v1.Service
{
    // CompanyService - класс для выполнения операций с компаниями
    public class CompanyService
    {
        // объект для доступа в БД для выполнения операций
        private readonly ApplicationDbContext _db;

        public CompanyService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Company>> ListAllAsync()
        {
            return await _db.Companies.ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Airport>?> GetAirportsByCompanyIdAsync(int companyId)
        {
            Company company = await _db.Companies
                .Include(c => c.Airports)
                .ThenInclude(a => a.Country) // Для включения информации о странах
                .FirstOrDefaultAsync(c => c.Id == companyId);

            return company?.Airports?.ToList();
        }

        public async Task AddAsync(Company company)
        {
            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsExists(string name)
        {
            return await _db.Companies.AnyAsync(c => c.Name == name);
        }

        public async Task<bool> IsExists(int id)
        {
            return await _db.Companies.AnyAsync(c => c.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            Company? company = await _db.Companies.FindAsync(id);
            if (company != null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
            }
        }


    }
}
