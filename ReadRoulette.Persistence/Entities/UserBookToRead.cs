using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace ReadRoulette.Persistence;

public class UserBookToRead
{
    [Key]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("readerType")]
    public int ReaderType { get; set; }

    [ForeignKey("UserId")]
    [JsonPropertyName("readerId")]
    public string ReaderId { get; set; }

    [ForeignKey("BookId")]
    [JsonPropertyName("bookId")]
    public int BookId { get; set; }

}
