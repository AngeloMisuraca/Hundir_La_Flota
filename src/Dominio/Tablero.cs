class Tablero
{
    bool todosHundidos;
    int barcosRestantes;

    public Tablero(bool todosHundidos, int barcosRestantes)
    {
        this.todosHundidos = todosHundidos;
        this.barcosRestantes = barcosRestantes;
    }

    public Casilla ObtenerCasilla(int fila, int columna)
    {
        return null;
    }

    public bool PuedeColocar(Barco barco, int fila, int columna, bool esHorizontal)
    {
        return false;
    }

    public void ColocarBarco(Barco barco, int fila, int columna, bool esHorizontal)
    {

    }

    public void Disparar(int fila, int columna)
    {

    }
}