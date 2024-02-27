using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReadRoulette.Test;

public class BrokerHttpClient(HttpClient _client)
{
    private static async ValueTask<T?> DeserializeResponseContent<T>(HttpResponseMessage response) =>
        JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());

    private static void SetHeaders(HttpHeaders httpHeaders, IDictionary<string, string>? headers)
    {
        if (headers == null) return;

        foreach (string key in headers.Keys)
        {
            if (!httpHeaders.Any(h => h.Key == key))
                httpHeaders.Add(key, headers[key]);
        }
    }

    public void ClearAllHeaders()
    {
        _client.DefaultRequestHeaders.Clear();
    }

    public async ValueTask<T?> GetAsync<T>(string url, IDictionary<string, string>? headers)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        SetHeaders(request.Headers, headers);

        using var response = await _client.SendAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK) return default;
        return await DeserializeResponseContent<T>(response);
    }

    public async ValueTask<T?> GetAsync<T>(string url, 
        IDictionary<string, string>? headers, 
        string bearer)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        SetHeaders(request.Headers, headers);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

        using var response = await _client.SendAsync(request);
        return await DeserializeResponseContent<T>(response);
    }

    public async ValueTask<T2?> PostAsync<T1, T2>(string url, T1 request, IDictionary<string, string>? headers)
    {
        var json = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        SetHeaders(httpContent.Headers, headers);

        var response = await _client.PostAsync(url, httpContent);
        if (response.StatusCode != System.Net.HttpStatusCode.OK) return default;
        return await DeserializeResponseContent<T2>(response);
    }

    public async Task<T2?> PutAsync<T1, T2>(string url, T1 request, IDictionary<string, string>? headers)
    {
        var json = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        SetHeaders(httpContent.Headers, headers);

        var response = await _client.PutAsync(url, httpContent);
        if (response.StatusCode != System.Net.HttpStatusCode.OK) return default;
        return await DeserializeResponseContent<T2>(response);
    }

    public async Task<T?> DeleteAsync<T>(string url, IDictionary<string, string>? headers)
    {
        SetHeaders(_client.DefaultRequestHeaders, headers);

        var response = await _client.DeleteAsync(url);
        if (response.StatusCode != System.Net.HttpStatusCode.OK) return default;
        return await DeserializeResponseContent<T>(response);
    }

    public async Task AuthUser(string email, string password)
    {
        var login = new AccountLogin { Email = email, Password = password };
        var json = JsonSerializer.Serialize(login);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Clear();

        var response = await _client.PostAsync("/login", stringContent);
        var result = JsonSerializer.Deserialize<AuthResponse>(await response.Content.ReadAsStringAsync());
        if (result is null) return;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
    }

    public async Task RegisterUser(string email, string password)
    {
        var registration = new AccountLogin { Email = email, Password = password };
        var json = JsonSerializer.Serialize(registration);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Clear();

        await _client.PostAsync("/register", stringContent);
    }

    public class AccountLogin
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }

    public class AuthResponse
    {
        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }
        [JsonPropertyName("expiresIn")]
        public int? ExpiresIn { get; set; }
        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
