using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAPAVISTA
{
    public partial class EditarPermisos : Form
    {
        public int selectedId = 0;

        public EditarPermisos(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadPermisoData();
        }

        private async void LoadPermisoData()
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

                HttpResponseMessage responsePermisos = await client.GetAsync("Permisos/Listar");
                if (responsePermisos.IsSuccessStatusCode)
                {
                    string jsonResponse = await responsePermisos.Content.ReadAsStringAsync();
                    var permisos = JsonConvert.DeserializeObject<List<Permisos>>(jsonResponse);

                    var idsPermisos = permisos.Select(p => p.IdPermisos).ToList();
                    if (!idsPermisos.Contains(selectedId))
                    {
                        MessageBox.Show($"Permiso con ID {selectedId} no encontrado. IDs disponibles: {string.Join(", ", idsPermisos)}");
                        return;
                    }

                    Permisos permiso = permisos.Find(p => p.IdPermisos == selectedId);
                    if (permiso != null)
                    {
                        comboBox1.SelectedValue = permiso.IdEmpleado; 
                        dateTimePicker1.Value = permiso.FechaInicio;
                        dateTimePicker2.Value = permiso.FechaFin;
                        textBox1.Text = permiso.TipoPermiso; 
                    }
                    else
                    {
                        MessageBox.Show("Permiso no encontrado.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar datos del permiso: {responsePermisos.StatusCode}");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedId > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Permisos/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    Permisos permisoEditado = new Permisos
                    {
                        IdPermisos = selectedId,
                        IdEmpleado = (int)comboBox1.SelectedValue, 
                        FechaInicio = dateTimePicker1.Value,
                        FechaFin = dateTimePicker2.Value,
                        TipoPermiso = textBox1.Text
                    };

                    string jsonPermiso = JsonConvert.SerializeObject(permisoEditado);
                    StringContent content = new StringContent(jsonPermiso, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"Actualizar/{selectedId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Permiso editado correctamente.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Error al editar permiso: {response.StatusCode}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un permiso para editar.");
            }
        }
    }
}
