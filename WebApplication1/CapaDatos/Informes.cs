using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Informes
    {
       
        public int IdInformes { get; set; }

        
        public int IdEmpleado { get; set; }

        
        public DateTime FechaGeneracion { get; set; }

      
        public string ContenidoInforme { get; set; }
    }
}
