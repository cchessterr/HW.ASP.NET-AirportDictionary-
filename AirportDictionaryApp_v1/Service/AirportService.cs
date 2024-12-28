using AirportDictionaryApp_v1.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryApp_v1.Service
{
    // AirpotService - класс для выполнения операций с аэропортами
    public class AirportService
    {
        // объект для доступа в БД для выполнения операций
        private readonly ApplicationDbContext _db;

        public AirportService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Airport>> ListAllAsync()
        {
            return await _db.Airports.ToListAsync();
        }

        // получить аэропорт по id с выгрзкой информации о стране
        public async Task<Airport?> GetAsync(int id)
        {
            return await _db.Airports
                .Include(airport => airport.Country)
                .FirstOrDefaultAsync(airport => airport.Id == id);
        }

        // получить аэропорт по коду с выгрзкой информации о стране
        public async Task<Airport?> GetAsync(string code)
        {
            return await _db.Airports
                .Include(airport => airport.Country)
                .FirstOrDefaultAsync(airport => airport.Code == code);
        }

        public async Task AddAsync(Airport airport)
        {
            await _db.Airports.AddAsync(airport);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsExists(string code)
        {
            return await _db.Airports.Where(a => a.Code == code).CountAsync() > 0;
        }

        public async Task DeleteAsync(string code)
        {
            Airport? airport = await _db.Airports.FirstOrDefaultAsync(a => a.Code == code);
            if (airport != null)
            {
                _db.Airports.Remove(airport);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateAnnualPassengerTrafficAsync(int id, long newAnnualPassengerTraffic)
        {
            Airport? airport = await _db.Airports.FirstOrDefaultAsync(a => a.Id == id);
            if (airport == null)
            {
                return false;
            }

            airport.AnnualPassengerTraffic = newAnnualPassengerTraffic;
            _db.Airports.Update(airport);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Company>?> GetCompaniesByAirportIdAsync(int airportId)
        {
            Airport? airport = await _db.Airports
                .Include(a => a.Companies)
                .FirstOrDefaultAsync(a => a.Id == airportId);

            return airport?.Companies?.ToList();
        }

        public async Task<bool> AddCompanyToAirportAsync(int airportId, int companyId)
        {
            Airport? airport = await _db.Airports.Include(a => a.Companies).FirstOrDefaultAsync(a => a.Id == airportId);
            if (airport == null) return false;

            Company? company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (company == null) return false;

            if (airport.Companies == null)
            {
                airport.Companies = new HashSet<Company>();
            }

            // Добавляем компанию к аэропорту, если она ещё не привязана
            if (!airport.Companies.Any(c => c.Id == companyId))
            {
                airport.Companies.Add(company);
                _db.Airports.Update(airport);
                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveCompanyFromAirportAsync(int airportId, int companyId)
        {
            Airport? airport = await _db.Airports.Include(a => a.Companies).FirstOrDefaultAsync(a => a.Id == airportId);
            if (airport == null) return false;

            Company? company = airport.Companies?.FirstOrDefault(c => c.Id == companyId);
            if (company == null) return false;

            airport.Companies?.Remove(company);
            _db.Airports.Update(airport);
            await _db.SaveChangesAsync();

            return true;
        }


    }
}
