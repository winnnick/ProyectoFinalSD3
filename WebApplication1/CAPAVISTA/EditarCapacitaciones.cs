using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CAPAVISTA
{
    public partial class EditarCapacitaciones : Form
    {
        public int selectedId = 0;
        public EditarCapacitaciones(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadCapacitacionData();
        }
        private async void LoadCapacitacionData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseEmpleados = await client.GetAsync("Empleados/Listar");
                if (responseEmpleados.IsSuccessStatusCode)
                {
                    string jsonEmpleados = await responseEmpleados.Content.ReadAsStringAsync();
                    var empleados = JsonConvert.DeserializeObject<List<Empleados>>(jsonEmpleados);

                    comboBox1.DataSource = empleados;
                    comboBox1.DisplayMember = "Nombre";  
                    comboBox1.ValueMember = "IdEmpleado"; 
                }
                else
                {
                    MessageBox.Show("Error al cargar empleados.");
                    return;
                }

                HttpResponseMessage responseCapacitaciones = await client.GetAsync("Capacitaciones/Listar");
                if (responseCapacitaciones.IsSuccessStatusCode)
                {
                    string jsonResponse = await responseCapacitaciones.Content.ReadAsStringAsync();
                    var capacitaciones = JsonConvert.DeserializeObject<List<Capacitaciones>>(jsonResponse);

                    var idsCapacitaciones = capacitaciones.Select(c => c.IdCapacitaciones).ToList();
                    if (!idsCapacitaciones.Contains(selectedId))
                    {
                        MessageBox.Show($"Capacitación con ID {selectedId} no encontrada. IDs disponibles: {string.Join(", ", idsCapacitaciones)}");
                        return;
                    }

                    Capacitaciones capacitacion = capacitaciones.Find(c => c.IdCapacitaciones == selectedId);
                    if (capacitacion != null)
                    {
                        comboBox1.SelectedValue = capacitacion.IdEmpleado; 
                        dateTimePicker1.Value = capacitacion.FechaInicio;
                        dateTimePicker2.Value = capacitacion.FechaFin;
                        textBox1.Text = capacitacion.TipoCapacitacion;
                    }
                    else
                    {
                        MessageBox.Show("Capacitación no encontrada.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar datos de la capacitación: {responseCapacitaciones.StatusCode}");
                }
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedId > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Capacitaciones/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    Capacitaciones capacitacionEditada = new Capacitaciones
                    {
                        IdCapacitaciones = selectedId,
                        IdEmpleado = (int)comboBox1.SelectedValue, 
                        FechaInicio = dateTimePicker1.Value,
                        FechaFin = dateTimePicker2.Value,
                        TipoCapacitacion = textBox1.Text
                    };

                    string jsonCapacitacion = JsonConvert.SerializeObject(capacitacionEditada);
                    StringContent content = new StringContent(jsonCapacitacion, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"Actualizar/{selectedId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Capacitación editada correctamente.");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show($"Error al editar capacitación: {response.StatusCode}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una capacitación para editar.");
            }

        }
    }
}
