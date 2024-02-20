using System.Text.Json.Serialization;

namespace ReadRoulette.Persistence;

public class Book
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("title")]

  public string? Title { get; set; }

  [JsonPropertyName("author")]

  public string? Author { get; set; }
}
