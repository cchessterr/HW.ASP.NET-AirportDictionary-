namespace AirportDictionaryApp_v1.Api
{
    // record-ы сообщений API

    // StringMessage - строковое сообщение
    public record StringMessage(string Message);

    // ErrorMessage - сообщение с ошибкой
    public record ErrorMessage(string Type, string Message);

    // CountryMessage - сообщение с данными о стране
    public record CountryMessage(string Name, string Code);

    // AirportListItemMessage - сообщение с данными об аэропорте в списке аэропортов
    public record AirportListItemMessage(int Id, string Name, string Code, string Location, string CountryCode);

    // CompanyMessage - сообщение с данными об компании
    public record CompanyMessage(int Id, string Name);

    // AirportMessage - сообщение с полными данными об аэропорте
    public record AirportMessage(
        int Id,
        string Name,
        string Code,
        int OpeningYear,
        int RunwayCount,
        long AnnualPassengerTraffic,
        string Location,
        int CountryId,
        CountryMessage Country
    );

    // AddAirportMessage - сообщение для добавления нового аэропорта
    public record AddAirportMessage(
        string Name,
        string Code,
        int OpeningYear,
        int RunwayCount,
        long AnnualPassengerTraffic,
        string Location,
        string CountryCode
        );

    // CompaniesByAirportIdMessage - сообщение для вывода списка компания по выбранному аэропорту
    public record CompaniesByAirportIdMessage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // AddCompanyMessage - сообщение для добавления новой компании
    public record AddCompanyMessage(string Name);
}
