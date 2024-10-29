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
    public partial class EditarInformes : Form
    {
        public int selectedId = 0;

        public EditarInformes(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadInformeData();
        }

        private async void LoadInformeData()
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

                HttpResponseMessage responseInformes = await client.GetAsync("Informes/Listar");
                if (responseInformes.IsSuccessStatusCode)
                {
                    string jsonResponse = await responseInformes.Content.ReadAsStringAsync();
                    var informes = JsonConvert.DeserializeObject<List<Informes>>(jsonResponse);

                    var idsInformes = informes.Select(i => i.IdInformes).ToList();
                    if (!idsInformes.Contains(selectedId))
                    {
                        MessageBox.Show($"Informe con ID {selectedId} no encontrado. IDs disponibles: {string.Join(", ", idsInformes)}");
                        return;
                    }

                    Informes informe = informes.Find(i => i.IdInformes == selectedId);
                    if (informe != null)
                    {
                        comboBox1.SelectedValue = informe.IdEmpleado; 
                        dateTimePicker1.Value = informe.FechaGeneracion;
                        textBox1.Text = informe.ContenidoInforme; 
                    }
                    else
                    {
                        MessageBox.Show("Informe no encontrado.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar datos del informe: {responseInformes.StatusCode}");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedId > 0) 
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Informes/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    Informes informeEditado = new Informes
                    {
                        IdInformes = selectedId,
                        IdEmpleado = (int)comboBox1.SelectedValue, 
                        FechaGeneracion = dateTimePicker1.Value,
                        ContenidoInforme = textBox1.Text 
                    };
                    string jsonInforme = JsonConvert.SerializeObject(informeEditado);
                    StringContent content = new StringContent(jsonInforme, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"Actualizar/{selectedId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Informe editado correctamente.");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show($"Error al editar informe: {response.StatusCode}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un informe para editar.");
            }
        }
    }
}
