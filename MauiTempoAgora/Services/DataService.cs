using MauiTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiTempoAgora.Services
{
    public class DataService
    {

        public static async Task<Tempo?> GetWeather(string cidade)
        {
            Tempo? t = null;

            string appid = "01ff97cc475aad84b3ae7ed0a16402d9";
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appid}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode) 
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((int)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((int)rascunho["sys"]["sunset"]).ToLocalTime();


                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        visibility = (int)rascunho["visibility"],
                        windSpeed = (double)rascunho["wind"]["speed"],
                        sunrise = sunrise,
                        sunset = sunset

                    };



                } else if (resp.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new Exception("Você está sem conexão com a internet!");

                } else if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Erro, Cidade não encontrada!");
                }

            }


                return t;
        }

    }
}
