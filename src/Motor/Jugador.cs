class Jugador
{
    // Datos basicos y estadisticas del jugador.
    string nombre;
    Tablero tablero;
    int disparos;
    int aciertos;
    int fallos;
    double precision;

    public string Nombre
    {
        get
        {
            return nombre;
        }
    }

    public Tablero Tablero
    {
        get
        {
            return tablero;
        }
    }

    public int Disparos
    {
        get
        {
            return disparos;
        }
    }

    public int Aciertos
    {
        get
        {
            return aciertos;
        }
    }

    public int Fallos
    {
        get
        {
            return fallos;
        }
    }

    public double Precision
    {
        get
        {
            return precision;
        }
    }

    public Jugador(string nombre, int disparos, int aciertos, int fallos, double precision, Tablero tablero)
    {
        // Guardamos todos los datos recibidos.
        this.nombre = nombre;
        this.disparos = disparos;
        this.aciertos = aciertos;
        this.fallos = fallos;
        this.tablero = tablero;
        ActualizarPrecision();
    }

    public Jugador(string nombre, Tablero tablero) : this(nombre, 0, 0, 0, 0, tablero)
    {
        // Este constructor crea un jugador nuevo con estadisticas a cero.
    }

    public void RegistrarDisparo(ResultadoDisparo resultado)
    {
        // Si el disparo no cuenta porque estaba repetido, no tocamos estadisticas.
        if (resultado == ResultadoDisparo.YaDisparado)
        {
            return;
        }

        // Cada disparo valido suma al total.
        disparos++;

        // Sumamos aciertos o fallos segun el resultado.
        if (resultado == ResultadoDisparo.Impacto || resultado == ResultadoDisparo.Hundido)
        {
            aciertos++;
        }
        else if (resultado == ResultadoDisparo.Agua)
        {
            fallos++;
        }

        ActualizarPrecision();
    }

    void ActualizarPrecision()
    {
        // La precision es el porcentaje de aciertos sobre el total de disparos.
        if (disparos == 0)
        {
            precision = 0;
        }
        else
        {
            precision = (double)aciertos * 100 / disparos;
        }
    }
}
