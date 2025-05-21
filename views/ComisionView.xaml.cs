using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SiDePeTra.BLL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SiDePeTra_APPITSG.views
{
    public partial class ComisionView : ContentView
    {
        private byte[] archivoPDF;

        public event EventHandler VolverSolicitudes;

        public ComisionView()
        {
            InitializeComponent();
            InicializarDatos();
        }

        private void InicializarDatos()
        {
            lblFechaHoy.Text = DateTime.Now.ToString("dd/MM/yyyy");

            string nombre = Preferences.Get("NombreCompleto", "");
            string noEmpleado = Preferences.Get("UsuarioID", "");

            lblNombre.Text = $"Nombre: {nombre}";
            lblNoEmpleado.Text = $"No. Empleado: {noEmpleado}";
        }

        private async void BtnSeleccionarPDF_Clicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona un archivo PDF",
                FileTypes = FilePickerFileType.Pdf
            });

            if (result != null)
            {
                if (result.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    var stream = await result.OpenReadAsync();
                    if (stream.Length > 5 * 1024 * 1024)
                    {
                        await Application.Current.MainPage.DisplayAlert("Archivo muy grande", "El archivo no debe superar los 5 MB.", "OK");
                        return;
                    }

                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    archivoPDF = ms.ToArray();
                    lblArchivoSeleccionado.Text = result.FileName;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Formato no válido", "Solo se permiten archivos PDF.", "OK");
                }
            }
        }

        private async void BtnEnviarSolicitud_Clicked(object sender, EventArgs e)
        {
            try
            {
                string id = Preferences.Get("UsuarioID", "");
                string tipoUsuario = Preferences.Get("TipoUsuario", "");
                string area = pickerArea.SelectedItem?.ToString() ?? "";
                string actividad = txtActividad.Text?.Trim() ?? "";

                DateTime fechaComision = dpFechaComision.Date;
                TimeSpan horaInicio = tpHoraInicio.Time;
                TimeSpan horaFin = tpHoraFin.Time;
                DateTime hoy = DateTime.Now;

                if (string.IsNullOrEmpty(actividad) || archivoPDF == null || string.IsNullOrEmpty(area))
                {
                    await Application.Current.MainPage.DisplayAlert("Campos incompletos", "Por favor complete todos los campos y adjunte el archivo.", "OK");
                    return;
                }

                bool exito = SolicitudBLL.GuardarSolicitudComision(
                    id, tipoUsuario, hoy, fechaComision, horaInicio, horaFin, area, actividad, archivoPDF);

                if (exito)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Solicitud enviada correctamente.", "OK");
                    txtActividad.Text = "";
                    archivoPDF = null;
                    lblArchivoSeleccionado.Text = "Ningún archivo seleccionado";
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Hubo un problema al guardar la solicitud.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Excepción", ex.Message, "OK");
            }
        }

        private void BtnVolver_Clicked(object sender, EventArgs e)
        {
            VolverSolicitudes?.Invoke(this, EventArgs.Empty);
        }

        private async void tpHoraFin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time" && tpHoraFin.IsEnabled)
            {
                var horaInicio = tpHoraInicio.Time;
                var horaFin = tpHoraFin.Time;

                if (horaFin <= horaInicio)
                {
                    await Application.Current.MainPage.DisplayAlert("Horario inválido", "La hora de fin debe ser mayor a la hora de inicio.", "OK");

                    try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200)); } catch { }

                    tpHoraFin.BackgroundColor = Colors.Red;
                    var sugerencia = horaInicio.Add(TimeSpan.FromHours(1));
                    tpHoraFin.Time = sugerencia;

                    await Task.Delay(1000);
                    tpHoraFin.BackgroundColor = Color.FromArgb("#1E1E1E");
                }
            }
        }

        private async void tpHoraInicio_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time" && tpHoraFin.IsEnabled)
            {
                var horaInicio = tpHoraInicio.Time;
                var horaFin = tpHoraFin.Time;

                if (horaFin <= horaInicio)
                {
                    await Application.Current.MainPage.DisplayAlert("Horario inválido", "La hora de fin debe ser mayor a la hora de inicio.", "OK");

                    try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200)); } catch { }

                    tpHoraFin.BackgroundColor = Colors.Red;
                    var sugerencia = horaInicio.Add(TimeSpan.FromHours(1));
                    tpHoraFin.Time = sugerencia;

                    await Task.Delay(1000);
                    tpHoraFin.BackgroundColor = Color.FromArgb("#1E1E1E");
                }
            }
        }
    }
}
