using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace SiDePeTra_APPITSG
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas necesarias
            Routing.RegisterRoute("LoginPage", typeof(views.LoginPage));
            Routing.RegisterRoute("Solicitud", typeof(views.Solicitud));
            Routing.RegisterRoute("Administrativo", typeof(views.Administrativo));

            // Obtener el ID guardado
            var userId = Preferences.Get("UsuarioID", "");

            if (string.IsNullOrEmpty(userId))
            {
                // No hay sesión → ir a login
                GoToAsync("//LoginPage");
            }
            else
            {
                // Ya hay sesión → seleccionar la pestaña "Solicitud"
                CurrentItem = TabSolicitud;
            }
        }
    }
}
