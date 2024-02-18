using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ReadRoulette.Persistence;

public class BookClub
{
    [Key]
    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified { get; set; }

    [ForeignKey("UserId")]
    public string UserId { get; set; }
}
