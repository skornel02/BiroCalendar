using System.Net.Http;

namespace BiroCalendar.Desktop;

public static class Globals
{
    //public const string ServerAddress = "http://localhost:5297";
    public const string ServerAddress = "https://birocalendar.skornel02.hu/";
    public readonly static Uri Server = new(ServerAddress);

    public readonly static Lazy<HttpClient> Client = new Lazy<HttpClient>(() =>  new HttpClient()
    {
        BaseAddress = Server
    });
}
