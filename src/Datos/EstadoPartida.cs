class EstadoPartida
{
    Jugador jugador;
    Cpu cpu;
    Tablero tableroJugador;
    Tablero tableroCpu;
    bool turnoJugador;

    public EstadoPartida(Jugador jugador, Cpu cpu, Tablero tableroJugador, Tablero tableroCpu, bool turnoJugador)
    {
        this.jugador = jugador;
        this.cpu = cpu;
        this.tableroJugador = tableroJugador;
        this.tableroCpu = tableroCpu;
        this.turnoJugador = turnoJugador;
    }
}