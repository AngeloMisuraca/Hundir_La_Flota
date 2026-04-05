class EstadoPartida
{
    // Aqui guardamos todo lo necesario para reconstruir una partida.
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
        // Creamos objetos vacios para que la clase sea facil de serializar y deserializar.
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
    // Este DTO guarda solo los datos simples del jugador.
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
    // Aqui se guardan los barcos del tablero y las casillas ya disparadas.
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
    // Esta clase representa un barco guardado en el archivo JSON.
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
    // Una coordenada sencilla para guardar fila y columna.
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
