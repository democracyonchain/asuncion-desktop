using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class OcrTextProcessorService
    {
        /// <summary>
        /// Convierte el resultado OCR en un número entero válido.
        /// </summary>
        /// <param name="ocrResult">Texto extraído del OCR</param>
        /// <returns>Un número entero válido</returns>
        public int ConvertirTextoANumero(string ocrResult)
        {
            if (string.IsNullOrWhiteSpace(ocrResult))
                return 0;

            // 1️⃣ Eliminar caracteres no numéricos ni letras
            string cleanedText = Regex.Replace(ocrResult, @"[^a-zA-Z0-9\s]", "");

            // 2️⃣ Extraer todos los números del texto
            var numerosEncontrados = Regex.Matches(cleanedText, @"\d+")
                              .Cast<Match>()  // Convertir MatchCollection a IEnumerable<Match>
                              .Select(m => m.Value)
                              .ToList();


            // 3️⃣ Intentar convertir texto a número si no hay dígitos en la cadena
            if (numerosEncontrados.Count == 0)
            {
                int numeroTexto = ConvertirTextoNumerico(cleanedText);
                return numeroTexto;
            }

            // 4️⃣ Si hay más de un número separado, unirlos
            string numeroConcatenado = string.Join("", numerosEncontrados);

            // 5️⃣ Convertir a int y devolver
            if (int.TryParse(numeroConcatenado, out int resultado))
                return resultado;

            return 0; // Si todo falla, devolver 0
        }

        /// <summary>
        /// Convierte números escritos en palabras a su representación numérica.
        /// </summary>
        private int ConvertirTextoNumerico(string texto)
        {
            // Diccionario con números en texto y su valor
            var numerosTexto = new Dictionary<string, int>
            {
                { "uno", 1 }, { "dos", 2 }, { "tres", 3 }, { "cuatro", 4 }, { "cinco", 5 },
                { "seis", 6 }, { "siete", 7 }, { "ocho", 8 }, { "nueve", 9 }, { "diez", 10 },
                { "once", 11 }, { "doce", 12 }, { "trece", 13 }, { "catorce", 14 }, { "quince", 15 },
                { "dieciseis", 16 }, { "diecisiete", 17 }, { "dieciocho", 18 }, { "diecinueve", 19 },
                { "veinte", 20 }, { "treinta", 30 }, { "cuarenta", 40 }, { "cincuenta", 50 },
                { "sesenta", 60 }, { "setenta", 70 }, { "ochenta", 80 }, { "noventa", 90 },
                { "cien", 100 }, { "ciento", 100 }, { "doscientos", 200 }, { "trescientos", 300 },
                { "cuatrocientos", 400 }, { "quinientos", 500 }, { "seiscientos", 600 },
                { "setecientos", 700 }, { "ochocientos", 800 }, { "novecientos", 900 }
            };

            string[] palabras = texto.ToLower().Split(' ');
            int numero = 0;
            int acumulado = 0;

            foreach (var palabra in palabras)
            {
                if (numerosTexto.ContainsKey(palabra))
                {
                    int valor = numerosTexto[palabra];

                    if (valor == 100 && acumulado > 0)
                    {
                        acumulado *= valor;
                    }
                    else if (valor > 100)
                    {
                        acumulado += valor;
                        numero += acumulado;
                        acumulado = 0;
                    }
                    else
                    {
                        acumulado += valor;
                    }
                }
            }

            return numero + acumulado;
        }
    }
}
