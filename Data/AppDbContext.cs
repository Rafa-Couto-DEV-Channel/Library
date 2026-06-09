using Library.API.Models;
using Microsoft.EntityFrameworkCore;

/* Classes são as "casas" dentro desse endereço. Cada classe tem um único propósito
   e deve se manter fiel a ele — sem acumular responsabilidades que não são suas.
   
   Esse é o princípio do S no SOLID: Single Responsibility (Responsabilidade Única).
*/
public class AppDbContext : DbContext
{
    /*
     Esse método com o mesmo nome da classe é o construtor. Ele é executado automaticamente
     quando a classe é instanciada e serve para inicializar os valores que ela precisa
     para funcionar — geralmente recebidos via injeção de dependência.
    */
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<BookModel> Book { get; set; }
    public DbSet<ClientModel> Client { get; set; }
    public DbSet<ReservationModel> Reservation { get; set; }
}