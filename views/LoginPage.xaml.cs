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
            this.Opacity = 0;
            this.TranslationY = 80;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Animación combinada: sube y aparece
            var fade = this.FadeTo(1, 400, Easing.CubicIn);
            var slide = this.TranslateTo(0, 0, 400, Easing.CubicOut);

            await Task.WhenAll(fade, slide);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // ?? Efecto rebote visual
            await btnIngresar.ScaleTo(0.9, 80, Easing.CubicIn);   // se encoge
            await btnIngresar.ScaleTo(1.05, 80, Easing.CubicOut); // rebote
            await btnIngresar.ScaleTo(1, 60);                     // regresa a tamaño normal

            string idIngresado = entryId.Text?.Trim();

            if (string.IsNullOrWhiteSpace(idIngresado))
            {
                await DisplayAlert("Error", "Por favor ingrese su ID", "OK");
                return;
            }

            btnIngresar.IsEnabled = false;
            overlayCargando.IsVisible = true;

            await Task.Delay(100); // dejar que se muestre el loader

            bool existe = await Task.Run(() => UsuarioBLL.ValidarExistenciaUsuario(idIngresado));

            overlayCargando.IsVisible = false;
            btnIngresar.IsEnabled = true;

            if (existe)
            {
                Preferences.Set("UsuarioID", idIngresado);

                await this.FadeTo(0, 300); // fade out
                App.Current.MainPage = new AppShell();
                await App.Current.MainPage.FadeTo(1, 300); // fade in
            }
            else
            {
                await DisplayAlert("ID inválido", "No se encontró un docente con ese ID", "OK");
            }
        }

    }
}
