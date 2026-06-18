namespace Library.API.Models;

public class ReservationModel
{
    public Guid Id { get; set; }
    public Guid ClientModelId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid Book { get; set; }
    public ClientModel? Client { get; set; }
}
