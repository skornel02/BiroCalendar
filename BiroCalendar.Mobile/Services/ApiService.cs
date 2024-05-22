using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

namespace BiroCalendar.Mobile.Services;

public static class ApiService
{
    private static readonly string BaseUrl = "http://10.0.2.2:5297";

    private static readonly HttpClient Client = new HttpClient()
    {
        BaseAddress = new Uri(BaseUrl)
    };

    public static async Task<string?> Login(string email, string password)
    {
        try
        {
            email = WebUtility.UrlEncode(email);
            password = WebUtility.UrlEncode(password);

            Debug.WriteLine($"Login!");

            var response = await Client.PostAsJsonAsync($"/account/login?email={email}&password={password}", new { });

            if (!response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<string>() ?? response.StatusCode.ToString();
            }

            return null;
        }
        catch (Exception ex)
        {
            return $"Unknonwn issue! ({ex.Message})";
        }
    }
}
