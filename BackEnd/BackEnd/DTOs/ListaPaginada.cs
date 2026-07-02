using System;
using System.Collections.Generic;

namespace BackEnd.DTOs
{
    public class ListaPaginada<T>
    {
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas => RegistrosPorPagina > 0 ? (int)Math.Ceiling(TotalRegistros / (double)RegistrosPorPagina) : 0;
        public List<T> Elementos { get; set; } = new List<T>(); //T representa Type, tomara valor cuando reciba un tipo o una clase
        //Esto nos servira para usar este DTO en varios Endpoints
    }
}
