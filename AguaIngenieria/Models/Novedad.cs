using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AguaIngenieria.Models
{
    public class Novedad
    {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Fuente { get; set; }
            public DateTime Fecha { get; set; }
            public string Resumen { get; set; }
            public string UrlNoticia { get; set; }
            public string ImagenPath { get; set; }
            public int? IdAdmin { get; set; }
    }
}