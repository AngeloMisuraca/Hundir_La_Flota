class Cpu : Jugador
{
    public Cpu(string nombre, int disparos, int aciertos, int fallos, double precision, int tablero) : base(nombre, disparos, aciertos, fallos, precision, tablero)
    {

    }

    public void ColocarFlotaAleatoria(List<Barco> barcos)
    {

    }

    public bool PuedeColocar()
    {
        return false;
    }

    public Casilla ElegirObjetivo()
    {
        return null;
    }
}

using System;
using System.Collections.Generic;
using HundirLaFlota.Dominio;

namespace HundirLaFlota.Motor
{
    public class Cpu : Jugador
    {
        private List<(int fila, int columna)> objetivos;
        private Random random;

        public Cpu(string nombre) : base(nombre)
        {
            random = new Random();
            objetivos = new List<(int, int)>();

            // Generar todas las coordenadas
            for (int fila = 0; fila < 10; fila++)
            {
                for (int columna = 0; columna < 10; columna++)
                {
                    objetivos.Add((fila, columna));
                }
            }

            BarajarObjetivos();
        }

        private void BarajarObjetivos()
        {
            for (int i = objetivos.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                var temp = objetivos[i];
                objetivos[i] = objetivos[j];
                objetivos[j] = temp;
            }
        }

        public (int fila, int columna) ElegirObjetivo()
        {
            var objetivo = objetivos[0];
            objetivos.RemoveAt(0);
            return objetivo;
        }

        public void ColocarFlotaAleatoria(List<Barco> barcos)
        {
            foreach (var barco in barcos)
            {
                bool colocado = false;

                while (!colocado)
                {
                    int fila = random.Next(0, 10);
                    int columna = random.Next(0, 10);
                    bool horizontal = random.Next(0, 2) == 0;

                    if (Tablero.PuedeColocar(barco, fila, columna, horizontal))
                    {
                        Tablero.ColocarBarco(barco, fila, columna, horizontal);
                        colocado = true;
                    }
                }
            }
        }
    }
}

//test