using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using SiDePeTra_APPITSG.BLL;
using SiDePeTra.DAL;

namespace SiDePeTra_APPITSG.views
{
    public partial class HistorialPage : ContentPage
    {
        public HistorialPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Mostrar placeholders mientras se cargan los datos
            collectionSolicitudes.ItemsSource = ObtenerPlaceholders();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Sin conexi�n", "No tienes conexi�n a internet. No se puede mostrar el historial.", "OK");
                collectionSolicitudes.ItemsSource = null;
                return;
            }

            string usuarioId = Preferences.Get("UsuarioID", "");
            var lista = await Task.Run(() => HistorialBLL.ObtenerSolicitudes(usuarioId));

            await Task.Delay(300); // simular transici�n m�s fluida
            collectionSolicitudes.ItemsSource = lista;
        }

        private List<SolicitudDTO> ObtenerPlaceholders()
        {
            var list = new List<SolicitudDTO>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new SolicitudDTO
                {
                    Estado = "Cargando...",
                    Rango = "Cargando...",
                    Fecha = DateTime.Now
                });
            }
            return list;
        }
    }
}
