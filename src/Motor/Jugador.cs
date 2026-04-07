class Jugador
{
    // Aqui guardo los datos y estadisticas del jugador.
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
        // Guardo los datos que me llegan.
        this.nombre = nombre;
        this.disparos = disparos;
        this.aciertos = aciertos;
        this.fallos = fallos;
        this.tablero = tablero;
        ActualizarPrecision();
    }

    public Jugador(string nombre, Tablero tablero) : this(nombre, 0, 0, 0, 0, tablero)
    {
        // Este constructor deja al jugador nuevo desde cero.
    }

    public void RegistrarDisparo(ResultadoDisparo resultado)
    {
        // Si el disparo estaba repetido, no cuento nada.
        if (resultado == ResultadoDisparo.YaDisparado)
        {
            return;
        }

        // Cada disparo valido suma uno.
        disparos++;

        // Aqui sumo acierto o fallo segun toque.
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
        // La precision sale de los aciertos sobre los disparos.
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
