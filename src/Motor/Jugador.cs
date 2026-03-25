class Jugador
{
    string nombre;
    int tablero; // no se si es int
    int disparos;
    int aciertos;
    int fallos;
    double precision;

    public Jugador(string nombre, int disparos, int aciertos, int fallos, double precision, int tablero)
    {
        this.nombre = nombre;
        this.disparos = disparos;
        this.aciertos = aciertos;
        this.fallos = fallos;
        this.precision = precision;
        this.tablero = tablero;
    }

    public void RegistrarDisparo(bool resultado)
    {

    }
}