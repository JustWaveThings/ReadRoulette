using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ReadRoulette.Persistence;

public class BookClubMember
{
    [Key]
    public int Id { get; set; }
    public int Order { get; set; }

    [ForeignKey("UserId")]
    public string UserId { get; set; }

    [ForeignKey("BookClubId")]
    public string BookClubId { get; set; }
}
