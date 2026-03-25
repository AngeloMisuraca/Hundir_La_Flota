class Renderizador
{
    public void MostrarBienvenida()
    {
        System.Console.WriteLine("Menú:");
        System.Console.WriteLine("1. Nueva partida");
        System.Console.WriteLine("2. Continuar");
        System.Console.WriteLine("3. Récords");
        System.Console.WriteLine("4. Opciones");
        System.Console.WriteLine("5. Salir");
        int opcion = Convert.ToInt32(Console.ReadLine());

        switch (opcion)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                System.Console.WriteLine("Opcion no válida");
                break;
        }
    }

    public static void MostrarTablerosBatalla(Tablero propio, Tablero enemigo)
    {
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        Console.Write("      ");
        for (int columna = 1; columna <= 10; columna++)
            Console.Write($"{columna,3}");
        Console.Write("          ");
        for (int columna = 1; columna <= 10; columna++)
            Console.Write($"{columna,3}");
        Console.WriteLine();

        for (int i = 0; i < 10; i++)
        {
            Console.Write($"  {letras[i]}   ");
            for (int columna = 0; columna < 10; columna++)
                ImprimirCasilla(propio.ObtenerCasilla(i, columna), true);

            Console.Write($"      {letras[i]}   ");
            for (int columna = 0; columna < 10; columna++)
                ImprimirCasilla(enemigo.ObtenerCasilla(i, columna), false);

            Console.WriteLine();
        }
    }

    static void ImprimirCasilla(Casilla casilla, bool esPropio)
    {
        if (casilla.EsImpacto())
        {
            if (casilla.barcos.Count > 0 && casilla.EstaVacia())
            {
                Console.Write("  #");
            }
            else
            {
                Console.Write("  X");
            }
        }
        else if (casilla.EsAgua())
        {
            Console.Write("  ~");
        }
        else if (!casilla.EstaVacia() && esPropio)
        {
            Console.Write("  S");
        }
        else
        {
            Console.Write("  .");
        }
    }

    public void PedirPosicion(Barco barco)
    {

    }

    public void PedirCoordenada()
    {

    }

    public void MostrarResultadoCpu(bool resultado, int fila, int columna)
    {

    }

    public void MostrarDisparoCpu(bool resultado, int fila, int columna)
    {

    }

    public void MostrarResultadoFinal(bool ganaJugador, Jugador jugador)
    {

    }

    public void MostrarError(string mensaje)
    {

    }
}