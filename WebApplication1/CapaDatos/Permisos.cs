﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Permisos
    {
        
        public int IdPermisos { get; set; } 

        public int IdEmpleado { get; set; } 
        
        public DateTime FechaInicio { get; set; }
        
        public DateTime FechaFin { get; set; }
        
        public string TipoPermiso { get; set; }
    }
}
