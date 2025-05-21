namespace SiDePeTra_APPITSG.views;
using System.Timers;
using SiDePeTra.BLL;
using Microsoft.Maui.Devices;


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

        txtDiasSolicitados.Text = "0";
        tpHoraInicio.Time = new TimeSpan(8, 0, 0); // 08:00 AM por defecto
        tpHoraFin.Time = new TimeSpan(9, 0, 0); // 09:00 AM por defecto
        AplicarLogicaDias();           

        string id = Preferences.Get("UsuarioID", "");
        string nombre = $"{Preferences.Get("Nombre", "")} {Preferences.Get("ApellidoPaterno", "")} {Preferences.Get("ApellidoMaterno", "")}";

        lblNoEmpleado.Text = $"No. Empleado: {id}";
        lblNombre.Text = $"Nombre: {nombre}";
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        timer?.Stop();
    }
    public async Task MostrarCargando(bool mostrar)
    {
        if (overlayCargando != null)
        {
            overlayCargando.IsVisible = mostrar;
            await Task.Delay(100); // peque�a espera visual
        }
    }

    private void pickerTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        MostrarVistaSeleccionada();
    }

    private void MostrarVistaSeleccionada()
    {
        if (pickerTipo.SelectedIndex == 0)
        {
            viewSolicitudNormal.IsVisible = true;
            viewComision.IsVisible = false;
        }
        else
        {
            viewSolicitudNormal.IsVisible = false;
            viewComision.IsVisible = true;
        }
    }

    private void viewComision_VolverSolicitudes(object sender, EventArgs e)
    {
        pickerTipo.SelectedIndex = 0;
        MostrarVistaSeleccionada();
    }


    private void AplicarLogicaDias()
    {
        // Si est� vac�o, poner 0
        if (string.IsNullOrWhiteSpace(txtDiasSolicitados.Text))
        {
            txtDiasSolicitados.Text = "0";
        }

        if (!int.TryParse(txtDiasSolicitados.Text, out int dias) || dias <= 0)
        {
            //Modo por HORAS
            tpHoraInicio.IsEnabled = true;
            tpHoraFin.IsEnabled = true;

            dpFechaInicio.IsEnabled = true;
            dpFechaFin.IsEnabled = false;

            dpFechaInicio.Date = DateTime.Today;
            dpFechaFin.Date = DateTime.Today;

            lblTipoSolicitud.Text = "Solicitud por horas";
            lblTipoSolicitud.TextColor = Color.FromArgb("#00BFFF");
        }
        else
        {
            //Modo por D�AS
            tpHoraInicio.IsEnabled = false;
            tpHoraFin.IsEnabled = false;

            dpFechaInicio.IsEnabled = true;
            dpFechaFin.IsEnabled = false;

            DateTime inicio = CalcularProximoDiaHabil(DateTime.Today);
            DateTime fin = CalcularFechaFinDiasHabiles(inicio, dias - 1);

            dpFechaInicio.Date = inicio;
            dpFechaFin.Date = fin;

            lblTipoSolicitud.Text = $"Solicitud por {dias} d�a(s)";
            lblTipoSolicitud.TextColor = Color.FromArgb("#FF6B6B");
        }
    }


    private async void BtnEnviarSolicitud_Clicked(object sender, EventArgs e)
    {
        decimal horasFrenteGrupo = 0;
        decimal horasApoyo = 0;

        decimal.TryParse(txtHorasFrenteGrupo.Text, out horasFrenteGrupo);
        decimal.TryParse(txtHorasApoyo.Text, out horasApoyo);

        // Validaci�n
        if (string.IsNullOrWhiteSpace(txtDiasSolicitados.Text) ||
            string.IsNullOrWhiteSpace(txtMotivo.Text))
        {
            await DisplayAlert("Campos incompletos", "Por favor complete todos los campos.", "OK");
            return;
        }

        if (pickerArea.SelectedIndex == -1)
        {
            await DisplayAlert("Campo requerido", "Seleccione un �rea de adscripci�n.", "OK");
            return;
        }


        string area = pickerArea.SelectedItem.ToString();
        string id = Preferences.Get("UsuarioID", "");
        string tipoUsuario = Preferences.Get("TipoUsuario", "");
        string motivo = txtMotivo.Text.Trim();
        int dias = int.Parse(txtDiasSolicitados.Text);
        DateTime fechaInicio = dpFechaInicio.Date;
        DateTime fechaFin = dpFechaFin.Date;
        TimeSpan horaInicio = tpHoraInicio.Time;
        TimeSpan horaFin = tpHoraFin.Time;

        await MostrarCargando(true);
        await Task.Delay(100); // dejar que se muestre el loader
        bool exito = await Task.Run(() => SolicitudBLL.GuardarSolicitud(id, dias, fechaInicio, fechaFin, horaInicio, horaFin, motivo, area, horasFrenteGrupo, horasApoyo));
        await MostrarCargando(false);

        if (exito)
        {
            await DisplayAlert("�xito", "La solicitud se ha enviado correctamente.", "OK");
            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo enviar la solicitud. Int�ntelo de nuevo.", "OK");
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

    private void TxtDiasSolicitados_Focused(object sender, FocusEventArgs e)
    {
        if (txtDiasSolicitados.Text == "0")
        {
            txtDiasSolicitados.Text = "";
        }
    }

    private void TxtDiasSolicitados_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtDiasSolicitados.Text))
        {
            txtDiasSolicitados.Text = "0";
        }

        AplicarLogicaDias();
    }

    private void TxtDiasSolicitados_TextChanged(object sender, TextChangedEventArgs e)
    {
        AplicarLogicaDias();
    }

    private async void TpHoraInicio_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time" && tpHoraFin.IsEnabled)
        {
            var horaInicio = tpHoraInicio.Time;
            var horaFin = tpHoraFin.Time;

            if (horaFin <= horaInicio)
            {
                await DisplayAlert("Horario inv�lido", "La hora de fin debe ser mayor a la hora de inicio.", "OK");

                try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200)); } catch { }

                tpHoraFin.BackgroundColor = Colors.Red;
                var sugerencia = horaInicio.Add(TimeSpan.FromHours(1));
                tpHoraFin.Time = sugerencia;

                await Task.Delay(1000);
                tpHoraFin.BackgroundColor = Color.FromArgb("#1E1E1E");
            }
        }
    }

    private async void TpHoraFin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time" && tpHoraInicio.IsEnabled)
        {
            var horaInicio = tpHoraInicio.Time;
            var horaFin = tpHoraFin.Time;

            if (horaFin <= horaInicio)
            {
                await DisplayAlert("Horario inv�lido", "La hora de fin debe ser mayor a la hora de inicio.", "OK");

                try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200)); } catch { }

                tpHoraFin.BackgroundColor = Colors.Red;
                var sugerencia = horaInicio.Add(TimeSpan.FromHours(1));
                tpHoraFin.Time = sugerencia;

                await Task.Delay(1000);
                tpHoraFin.BackgroundColor = Color.FromArgb("#1E1E1E");
            }
        }
    }

    private DateTime CalcularProximoDiaHabil(DateTime fecha)
    {
        fecha = fecha.AddDays(1);
        while (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
        {
            fecha = fecha.AddDays(1);
        }
        return fecha;
    }

    private DateTime CalcularFechaFinDiasHabiles(DateTime inicio, int diasHabiles)
    {
        int diasAgregados = 0;
        DateTime fecha = inicio;

        while (diasAgregados < diasHabiles)
        {
            fecha = fecha.AddDays(1);
            if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
            {
                diasAgregados++;
            }
        }

        return fecha;
    }

    private void dpFechaInicio_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (int.TryParse(txtDiasSolicitados.Text, out int dias) && dias >= 0)
        {
            DateTime nuevaFechaInicio = dpFechaInicio.Date;

            DateTime nuevaFechaFin = CalcularFechaFinDiasHabiles(nuevaFechaInicio, dias - 1);

            dpFechaFin.Date = nuevaFechaFin;
        }
    }
}