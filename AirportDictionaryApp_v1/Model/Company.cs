namespace AirportDictionaryApp_v1.Model
{
    // Company - класс, описывающий сущность авикомпании
    public class Company
    {
        // поля
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // связи
        public HashSet<Airport>? Airports { get; set; }
        public Company() { }
    }
}
