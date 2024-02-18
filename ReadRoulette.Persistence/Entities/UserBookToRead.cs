using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ReadRoulette.Persistence;

public class UserBookToRead
{
    [Key]
    public int Id { get; set; }
    public int Order { get; set; }
    public int ReaderType { get; set; }

    [ForeignKey("UserId")]
    public string ReaderId { get; set; }

    [ForeignKey("BookId")]
    public int BookId { get; set; }

}
