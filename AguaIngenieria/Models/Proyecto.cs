using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AguaIngenieria.Models
{
    public class Proyecto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string URL { get; set; }
        public DateTime FechaSubida{ get; set; }
        public int IdAdmin { get; set; }
    }
}