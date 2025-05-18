using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace SiDePeTra_APPITSG.views
{
    public partial class Administrativo : ContentPage
    {
        public Administrativo()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Obtener los datos desde preferencias locales
            string id = Preferences.Get("UsuarioID", "");
            string nombre = Preferences.Get("Nombre", "");
            string apePat = Preferences.Get("ApellidoPaterno", "");
            string apeMat = Preferences.Get("ApellidoMaterno", "");
            string tipo = Preferences.Get("TipoUsuario", "");

            lblNoEmpleado.Text = id;
            lblNombreCompleto.Text = $"{nombre} {apePat} {apeMat}";
            lblTipoUsuario.Text = tipo;
        }

        private async void CerrarSesion_Clicked(object sender, EventArgs e)
        {
            bool confirmacion = await DisplayAlert(
                "Cerrar sesión",
                "¿Estás seguro que deseas cerrar la sesión?",
                "Sí", "No");

            if (confirmacion)
            {
                // Borrar todos los datos guardados
                Preferences.Remove("UsuarioID");
                Preferences.Remove("Nombre");
                Preferences.Remove("ApellidoPaterno");
                Preferences.Remove("ApellidoMaterno");
                Preferences.Remove("TipoUsuario");

                // Regresar al login
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
