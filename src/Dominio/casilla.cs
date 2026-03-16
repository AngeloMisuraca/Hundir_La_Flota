class Casilla
{
    int fila;
    int columna;
    // Barco barco;
    bool disparada;

    public Casilla(int fila, int columna, bool disparada)
    {
        this.fila = fila;
        this.columna = columna;
        this.disparada = disparada;

    }

    public bool EstaVacia()
    {
        return false;
    }

    public bool EsImpacto()
    {
        return false;
    }

    public bool EsAgua()
    {
        return false;
    }
}