using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CAPAVISTA
{
    public partial class EditarNominas : Form
    {
        public int selectedId = 0;

        public EditarNominas(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadNominaData();
        }

        private async void LoadNominaData()
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

                HttpResponseMessage responseNominas = await client.GetAsync("Nominas/Listar");
                if (responseNominas.IsSuccessStatusCode)
                {
                    string jsonResponse = await responseNominas.Content.ReadAsStringAsync();
                    var nominas = JsonConvert.DeserializeObject<List<Nominas>>(jsonResponse);

                    var idsNominas = nominas.Select(n => n.IdNomina).ToList();
                    if (!idsNominas.Contains(selectedId))
                    {
                        MessageBox.Show($"Nómina con ID {selectedId} no encontrada. IDs disponibles: {string.Join(", ", idsNominas)}");
                        return;
                    }

                    Nominas nomina = nominas.Find(n => n.IdNomina == selectedId);
                    if (nomina != null)
                    {
                        comboBox1.SelectedValue = nomina.IdEmpleado; 
                        dateTimePicker1.Value = nomina.FechaPago;
                        numericUpDown1.Value = nomina.MontoPago;
                    }
                    else
                    {
                        MessageBox.Show("Nómina no encontrada.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar datos de la nómina: {responseNominas.StatusCode}");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedId > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Nominas/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    Nominas nominaEditada = new Nominas
                    {
                        IdNomina = selectedId,
                        IdEmpleado = (int)comboBox1.SelectedValue,
                        FechaPago = dateTimePicker1.Value,
                        MontoPago = numericUpDown1.Value 
                    };

                    string jsonNomina = JsonConvert.SerializeObject(nominaEditada);
                    StringContent content = new StringContent(jsonNomina, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"Actualizar/{selectedId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Nómina editada correctamente.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Error al editar nómina: {response.StatusCode}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una nómina para editar.");
            }
        }

        private void EditarNominas_Load(object sender, EventArgs e)
        {

        }
    }
}
