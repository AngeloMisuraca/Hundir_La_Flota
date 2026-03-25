class Program
{
    static void Main(string[] args)
    {
        Tablero propio = new Tablero(false, 5);
        Tablero enemigo = new Tablero(false, 5);

        Barco barco1 = new Barco("Acorazado", 4, 0);
        Barco barco2 = new Barco("Destructor", 3, 0);
        // Barco barco3 = new Barco("Acorazado", 1, 0);
        // Barco barco4 = new Barco("Destructor", 2, 0);

        propio.ColocarBarco(barco1, 1, 1, true);
        propio.ColocarBarco(barco2, 4, 2, false);

        // enemigo.ColocarBarco(barco3, 2, 2, true);
        // enemigo.ColocarBarco(barco4, 5, 5, false);

        Renderizador.MostrarTablerosBatalla(propio, enemigo);
    }
}