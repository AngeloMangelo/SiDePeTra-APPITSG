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
    protected override void OnAppearing()
    {
        base.OnAppearing();

        string id = Preferences.Get("UsuarioID", "");
        string nombre = $"{Preferences.Get("Nombre", "")} {Preferences.Get("ApellidoPaterno", "")} {Preferences.Get("ApellidoMaterno", "")}";

        lblNoEmpleado.Text = $"No. Empleado: {id}";
        lblNombre.Text = $"Nombre: {nombre}";
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        timer?.Stop(); // Detiene el timer cuando la página desaparece
    }

    private async void BtnEnviarSolicitud_Clicked(object sender, EventArgs e)
    {
        // Validación
        if (string.IsNullOrWhiteSpace(txtDiasSolicitados.Text) ||
            string.IsNullOrWhiteSpace(txtMotivo.Text))
        {
            await DisplayAlert("Campos incompletos", "Por favor complete todos los campos.", "OK");
            return;
        }

        if (pickerArea.SelectedIndex == -1)
        {
            await DisplayAlert("Campo requerido", "Seleccione un área de adscripción.", "OK");
            return;
        }

        string area = pickerArea.SelectedItem.ToString();


        string id = Preferences.Get("UsuarioID", "");
        string nombre = Preferences.Get("Nombre", "") + " " +
                        Preferences.Get("ApellidoPaterno", "") + " " +
                        Preferences.Get("ApellidoMaterno", "");

        string motivo = txtMotivo.Text.Trim();
        int dias = int.Parse(txtDiasSolicitados.Text);
        DateTime fechaInicio = dpFechaInicio.Date;
        DateTime fechaFin = dpFechaFin.Date;
        TimeSpan horaInicio = tpHoraInicio.Time;
        TimeSpan horaFin = tpHoraFin.Time;

        // Aquí deberías llamar a una función de BLL o DAL para guardar en base de datos
        bool exito = SolicitudBLL.GuardarSolicitud(id, dias, fechaInicio, fechaFin, horaInicio, horaFin, motivo, area);

        if (exito)
        {
            await DisplayAlert("Éxito", "La solicitud se ha enviado correctamente.", "OK");
            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo enviar la solicitud. Inténtelo de nuevo.", "OK");
        }
    }

    private void LimpiarCampos()
    {
        txtDiasSolicitados.Text = "";
        dpFechaInicio.Date = DateTime.Today;
        dpFechaFin.Date = DateTime.Today;
        tpHoraInicio.Time = new TimeSpan(8, 0, 0);
        tpHoraFin.Time = new TimeSpan(9, 0, 0);
        txtMotivo.Text = "";
    }

}