using AsuncionDesktop.Domain.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ActaIngreso
{
    public int Codigo { get; set; }
    public int Seguridad { get; set; }

    public string Provincia { get; set; }
    public string Canton { get; set; }
    public string Parroquia { get; set; }
    public string Zona { get; set; }

    public int Junta { get; set; }
    public string Sexo { get; set; }
    public int Dignidad { get; set; }
    public int Pagina { get; set; }

    [JsonProperty("numero_paginas")]
    public int Paginas { get; set; }

    public string Path { get; set; }
    public List<Pagina> paginas { get; set; }

    public int Estado { get; set; }
    public int Sufragantes { get; set; }
    public int Blancos { get; set; }
    public int Nulos { get; set; }

    public string TxIcr { get; set; }

    public ActaIngreso()
    {
        paginas = new List<Pagina>();
    }

    /// <summary>
    /// Crea un ActaIngreso copiando todo desde Acta,
    /// excepto Provincia, Canton, Parroquia y Zona,
    /// que se reciben como parámetros.
    /// </summary>
    public static ActaIngreso FromActa(
        Acta acta,
        string provincia,
        string canton,
        string parroquia,
        string zona)
    {
        return new ActaIngreso
        {
            Codigo = acta.Codigo,
            Seguridad = acta.Seguridad,

            Provincia = provincia,
            Canton = canton,
            Parroquia = parroquia,
            Zona = zona,

            Junta = acta.Junta,
            Sexo = acta.Sexo,
            Dignidad = acta.Dignidad,
            Pagina = acta.Pagina,
            Paginas = acta.Paginas,

            Path = acta.Path,
            paginas = acta.paginas, // MISMA referencia (ok para desktop)
            Estado = acta.Estado,
            Sufragantes = acta.Sufragantes,
            Blancos = acta.Blancos,
            Nulos = acta.Nulos,

            TxIcr = acta.TxIcr
        };
    }
}
