
using MauiTempoAgora.Models;
using MauiTempoAgora.Services;
using System.Linq.Expressions;

namespace MauiTempoAgora
{
    public partial class MainPage : ContentPage
    {

        Tempo lst_Legal;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_previsao_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(ent_cidade.Text))
                {

                    Tempo? t = await DataService.GetWeather(ent_cidade.Text);

                    if (t != null)
                    {

                        lst_Legal = t;
                        BindingContext = lst_Legal;

                        string linkMapa = $"https://embed.windy.com/embed.html?type=map&location=coordinates&metricRain=mm&metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface&lat={t.lat.ToString().Replace(",", ".")}&lon={t.lon.ToString().Replace(",", ".")}";
                        windyMap.Source = linkMapa;



                        return;

                    }



                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK!");
            }
        }

        private async void btn_localizacao_Clicked(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest requestLocalizacao = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                Location? localizacaoAtual = await Geolocation.Default.GetLocationAsync(requestLocalizacao);
                if (localizacaoAtual == null) { throw new Exception("Localização nula!"); }

                GetCidade(localizacaoAtual.Latitude, localizacaoAtual.Longitude);

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro! Dispositivo não suporta GPS!", fnsEx.Message, "OK!");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro! Localização não ativada!", fneEx.Message, "OK!");
            }
            catch (PermissionException perEx)
            {
                await DisplayAlert("Erro! Permissão negada!", perEx.Message, "OK!");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK!");
            }
        }

        private async void GetCidade(double lat, double lon)
        {

            try
            {
                IEnumerable<Placemark> places = await Geocoding.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();
                if (place == null) { throw new Exception("Nenhum local encontrado!"); }

                ent_cidade.Text = place.Locality;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro!", ex.Message, "OK!");
            }



        }
    }

}
