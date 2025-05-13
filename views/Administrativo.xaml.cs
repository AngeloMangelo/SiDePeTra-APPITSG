using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Data;
using SiDePeTra.DAL;

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
            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            string idUsuario = Preferences.Get("UsuarioID", "");

            if (string.IsNullOrEmpty(idUsuario))
            {
                DisplayAlert("Error", "No se encontró sesión activa", "OK");
                return;
            }

            string consulta = $@"
                SELECT NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario
                FROM Usuarios
                WHERE NoEmpleado = '{idUsuario}'";

            DataTable resultado = ConexionBD.EjecutarConsulta(consulta);

            if (resultado.Rows.Count > 0)
            {
                var fila = resultado.Rows[0];
                lblNoEmpleado.Text = fila["NoEmpleado"].ToString();
                lblNombreCompleto.Text = $"{fila["Nombre"]} {fila["ApellidoPaterno"]} {fila["ApellidoMaterno"]}";
                lblTipoUsuario.Text = fila["TipoUsuario"].ToString();
            }
            else
            {
                DisplayAlert("Error", "No se encontraron los datos del usuario", "OK");
            }
        }

        private async void CerrarSesion_Clicked(object sender, EventArgs e)
        {
            bool confirmacion = await DisplayAlert(
                "Cerrar sesión",
                "¿Estás seguro que deseas cerrar la sesión?",
                "Sí", "No");

            if (confirmacion)
            {
                Preferences.Remove("UsuarioID");
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
