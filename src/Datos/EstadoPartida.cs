class EstadoPartida
{
    // Aqui guardo todo lo necesario para reconstruir la partida.
    public JugadorGuardado Jugador { get; set; }
    public JugadorGuardado Cpu { get; set; }
    public TableroGuardado TableroJugador { get; set; }
    public TableroGuardado TableroCpu { get; set; }
    public bool TurnoJugador { get; set; }
    public string FaseActual { get; set; }
    public ConfigJuego Configuracion { get; set; }
    public List<CoordenadaGuardada> ObjetivosCpu { get; set; }

    public EstadoPartida()
    {
        // Dejo todo inicializado para que el JSON no de problemas.
        Jugador = new JugadorGuardado();
        Cpu = new JugadorGuardado();
        TableroJugador = new TableroGuardado();
        TableroCpu = new TableroGuardado();
        TurnoJugador = true;
        FaseActual = "";
        Configuracion = new ConfigJuego();
        ObjetivosCpu = new List<CoordenadaGuardada>();
    }
}

class JugadorGuardado
{
    // Aqui solo guardo los datos simples del jugador.
    public string Nombre { get; set; }
    public int Disparos { get; set; }
    public int Aciertos { get; set; }
    public int Fallos { get; set; }

    public JugadorGuardado()
    {
        Nombre = "";
    }
}

class TableroGuardado
{
    // Aqui van los barcos y las casillas ya disparadas.
    public List<BarcoGuardado> Barcos { get; set; }
    public List<CoordenadaGuardada> CasillasDisparadas { get; set; }

    public TableroGuardado()
    {
        Barcos = new List<BarcoGuardado>();
        CasillasDisparadas = new List<CoordenadaGuardada>();
    }
}

class BarcoGuardado
{
    // Esto representa un barco dentro del JSON.
    public string Nombre { get; set; }
    public int Tamanio { get; set; }
    public int Impactos { get; set; }
    public List<CoordenadaGuardada> Casillas { get; set; }

    public BarcoGuardado()
    {
        Nombre = "";
        Casillas = new List<CoordenadaGuardada>();
    }
}

class CoordenadaGuardada
{
    // Esta clase solo guarda fila y columna.
    public int Fila { get; set; }
    public int Columna { get; set; }

    public CoordenadaGuardada()
    {
    }

    public CoordenadaGuardada(int fila, int columna)
    {
        Fila = fila;
        Columna = columna;
    }
}
