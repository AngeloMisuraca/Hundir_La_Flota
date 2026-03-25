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