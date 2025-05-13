using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SiDePeTra.BLL;

namespace SiDePeTra_APPITSG.views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string idIngresado = entryId.Text?.Trim();

            if (string.IsNullOrWhiteSpace(idIngresado))
            {
                await DisplayAlert("Error", "Por favor ingrese su ID", "OK");
                return;
            }

            btnIngresar.IsEnabled = false;
            overlayCargando.IsVisible = true;

            // Permite que se dibuje el overlay antes de entrar al Task pesado
            await Task.Delay(100);

            bool existe = await Task.Run(() => UsuarioBLL.ValidarExistenciaUsuario(idIngresado));

            overlayCargando.IsVisible = false;
            btnIngresar.IsEnabled = true;

            if (existe)
            {
                Preferences.Set("UsuarioID", idIngresado);
                App.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("ID inválido", "No se encontró un docente con ese ID", "OK");
            }
        }
    }
}
