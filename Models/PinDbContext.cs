// Kode for database i forhold til shop items. Enkelte nettsider kan ha flere slike database klasser

using Microsoft.EntityFrameworkCore; // importerer Entity framework core funksjoner som er nødvendig for database operasjoner

namespace PinThePlace.Models; // namespace: er en container som holder logisk gruppering av relaterte klasser, interfaces, structs, enums og delegates. Hjelper å organisere kode, unngå navnkonflikt og forbedre koden sin vedlikeholdbarhet

public class PinDbContext : DbContext // definerer at classen ItemDbContext arver fra DbContext. DbContext representerer en session med databasen. session(midlertidig interaktiv informasjons interchange) det er brukt til å query og lagre data til databasen
{
    public PinDbContext(DbContextOptions<PinDbContext> options) : base (options) // konstruktør. konfigurerer database connection string
    {
        //Database.EnsureCreated(); // lager en tom database hvis den ikke eksisterer en database fra før av som er ssosiert med nåværende DbContext
    }                              // lager database med schema(tables,indexes, etc) basert på nåværende model definert i DbContext
    public DbSet<Pin> Pins { get; set; } // metoder for å lagre instanser av Item
    public DbSet<User> Users{ get; set; }

/*
    public DbSet<Place> Places { get; set; }

    public DbSet<Favorite> Favorites{ get; set; }
    */
}