using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace ReadRoulette.Persistence;

public class BookClubMember
{
    [Key]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [ForeignKey("UserId")]
    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [ForeignKey("BookClubId")]
    [JsonPropertyName("bookClubId")]
    public string BookClubId { get; set; }
}
