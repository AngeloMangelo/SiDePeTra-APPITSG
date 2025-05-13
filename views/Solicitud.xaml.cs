namespace SiDePeTra_APPITSG.views;
using System.Timers;
using SiDePeTra.BLL;

public partial class Solicitud : ContentPage
{
    private Timer timer;
	public Solicitud()
	{
		InitializeComponent();
        IniciarActualizacionHora();
	}

    private async void BtnBuscar_Clicked(object sender, EventArgs e)
    {
        string noEmpleado = txtIdUsuario.Text;

        if (string.IsNullOrWhiteSpace(noEmpleado))
        {
            await DisplayAlert("Error", "Ingrese un ID de usuario", "OK");
            return;
        }

        try
        {
            var resultado = UsuarioBLL.ObtenerUsuarioPorId(noEmpleado);

            if (resultado.Rows.Count > 0)
            {
                var row = resultado.Rows[0];
                lblNombre.Text = $"Nombre: {row["Nombre"]} {row["ApellidoPaterno"]} {row["ApellidoMaterno"]}";
                lblNoEmpleado.Text = $"No. Empleado: {row["NoEmpleado"].ToString()}";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void IniciarActualizacionHora()
    {
        timer = new Timer(1000);
        timer.Elapsed += (sender, e) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                lblHoraFecha.Text = DateTime.Now.ToString("hh:mm:ss tt - dd/MM/yyyy");
            });
        };
        timer.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        timer?.Stop(); // Detiene el timer cuando la página desaparece
    }
}