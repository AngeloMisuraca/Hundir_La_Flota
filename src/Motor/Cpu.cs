class Cpu : Jugador
{
    // Lista de objetivos pendientes y generador de numeros aleatorios.
    List<Casilla> objetivos;
    Random random;

    public Cpu(string nombre, int disparos, int aciertos, int fallos, double precision, Tablero tablero) : base(nombre, disparos, aciertos, fallos, precision, tablero)
    {
        // Preparamos la CPU completa desde cero.
        random = new Random();
        objetivos = new List<Casilla>();
        PrepararObjetivos();
    }

    public Cpu(string nombre, Tablero tablero) : base(nombre, tablero)
    {
        // Version simplificada para una CPU nueva.
        random = new Random();
        objetivos = new List<Casilla>();
        PrepararObjetivos();
    }

    public void ColocarFlotaAleatoria(List<Barco> barcos)
    {
        // La CPU va colocando uno a uno todos sus barcos.
        foreach (Barco barco in barcos)
        {
            bool colocado = false;

            while (colocado == false)
            {
                // Elegimos una posicion y orientacion al azar.
                int fila = random.Next(0, 10);
                int columna = random.Next(0, 10);
                bool esHorizontal = random.Next(0, 2) == 0;

                // Solo colocamos si la posicion cumple las reglas.
                if (Tablero.PuedeColocar(barco, fila, columna, esHorizontal))
                {
                    Tablero.ColocarBarco(barco, fila, columna, esHorizontal);
                    colocado = true;
                }
            }
        }
    }

    public bool PuedeColocar()
    {
        // Devuelve true si aun quedan objetivos disponibles.
        return objetivos.Count > 0;
    }

    public Casilla ElegirObjetivo()
    {
        // Si ya no hay objetivos, lanzamos un error.
        if (objetivos.Count == 0)
        {
            throw new InvalidOperationException("La CPU ya no tiene objetivos disponibles.");
        }

        // La CPU toma siempre el primer objetivo de la lista y lo elimina.
        Casilla objetivo = objetivos[0];
        objetivos.RemoveAt(0);
        return objetivo;
    }

    public List<CoordenadaGuardada> ObtenerObjetivosGuardados()
    {
        // Convertimos la lista de objetivos en coordenadas simples para poder guardarlas.
        List<CoordenadaGuardada> objetivosGuardados = new List<CoordenadaGuardada>();

        foreach (Casilla objetivo in objetivos)
        {
            objetivosGuardados.Add(new CoordenadaGuardada(objetivo.Fila, objetivo.Columna));
        }

        return objetivosGuardados;
    }

    public void CargarObjetivos(List<CoordenadaGuardada> objetivosGuardados)
    {
        // Reconstruimos la lista de objetivos al cargar una partida.
        objetivos.Clear();

        foreach (CoordenadaGuardada coordenada in objetivosGuardados)
        {
            objetivos.Add(new Casilla(coordenada.Fila, coordenada.Columna, false));
        }
    }

    void PrepararObjetivos()
    {
        // Primero llenamos la lista con las 100 casillas del tablero.
        objetivos.Clear();

        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                objetivos.Add(new Casilla(fila, columna, false));
            }
        }

        // Luego la mezclamos con Fisher-Yates para que no siga un patron fijo.
        for (int i = objetivos.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Casilla temporal = objetivos[i];
            objetivos[i] = objetivos[j];
            objetivos[j] = temporal;
        }
    }
}
