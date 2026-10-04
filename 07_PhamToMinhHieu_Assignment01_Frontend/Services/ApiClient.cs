using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        var baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5190/api/";
        if (!baseUrl.EndsWith("/"))
        {
            baseUrl += "/";
        }
        _httpClient.BaseAddress = new Uri(baseUrl);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }

    public async Task<(bool Success, T? Data, string? Error)> PostAsync<T>(string endpoint, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<T>(responseString, _jsonOptions);
            return (true, result, null);
        }

        string errorMessage = ExtractErrorMessage(responseString);
        return (false, default, errorMessage);
    }

    public async Task<(bool Success, string? Error)> PutAsync(string endpoint, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(endpoint, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        string errorMessage = ExtractErrorMessage(responseString);
        return (false, errorMessage);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        string errorMessage = ExtractErrorMessage(responseString);
        return (false, errorMessage);
    }

    private string ExtractErrorMessage(string responseString)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseString);
            if (doc.RootElement.TryGetProperty("message", out var msgElement))
            {
                return msgElement.GetString() ?? "An error occurred.";
            }
            if (doc.RootElement.TryGetProperty("title", out var titleElement))
            {
                return titleElement.GetString() ?? "An error occurred.";
            }
        }
        catch
        {
            // not a json string
        }
        return string.IsNullOrWhiteSpace(responseString) ? "Operation failed." : responseString;
    }
}
