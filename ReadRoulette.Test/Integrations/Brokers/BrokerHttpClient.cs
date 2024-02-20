using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace ReadRoulette.Test;

public class BrokerHttpClient(HttpClient _client)
{
    private static async ValueTask<T?> DeserializeResponseContent<T>(HttpResponseMessage response) =>
        JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());

    private static void SetHeaders(HttpHeaders httpHeaders, IDictionary<string, string>? headers)
    {
        if (headers == null) return;

        foreach (string key in headers.Keys)
            httpHeaders.Add(key, headers[key]);
    }

    public async ValueTask<T?> GetAsync<T>(string url, IDictionary<string, string>? headers)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        SetHeaders(request.Headers, headers);
        _client.DefaultRequestHeaders.Clear();

        using var response = await _client.SendAsync(request);
        return await DeserializeResponseContent<T>(response);
    }

    public async ValueTask<T?> GetAsync<T>(string url, 
        IDictionary<string, string>? headers, 
        string bearer)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        SetHeaders(request.Headers, headers);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        _client.DefaultRequestHeaders.Clear();

        using var response = await _client.SendAsync(request);
        return await DeserializeResponseContent<T>(response);
    }

    public async ValueTask<T2?> PostAsync<T1, T2>(string url, T1 request, IDictionary<string, string> headers)
    {
        var json = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        SetHeaders(httpContent.Headers, headers);
        _client.DefaultRequestHeaders.Clear();

        var response = await _client.PostAsync(url, httpContent);
        return await DeserializeResponseContent<T2>(response);
    }

    public async Task<T2?> PutAsync<T1, T2>(string url, T1 request, IDictionary<string, string> headers)
    {
        var json = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        SetHeaders(httpContent.Headers, headers);
        _client.DefaultRequestHeaders.Clear();

        var response = await _client.PutAsync(url, httpContent);
        return await DeserializeResponseContent<T2>(response);
    }

    public async Task<T?> DeleteAsync<T>(string url, IDictionary<string, string> headers)
    {
        _client.DefaultRequestHeaders.Clear();
        SetHeaders(_client.DefaultRequestHeaders, headers);

        var response = await _client.DeleteAsync(url);
        return await DeserializeResponseContent<T>(response);
    }

    public async Task<object> AuthUser(string email, string password)
    {
        var login = new AccountLogin { Email = email, Password = password };
        var json = JsonSerializer.Serialize(login);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Clear();


        var response = await _client.PostAsync("/login?useCookies=true", stringContent);
        return response;
    }

    public class AccountLogin
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
