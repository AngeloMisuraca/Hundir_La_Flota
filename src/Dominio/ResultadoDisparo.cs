class ResultadoDisparo
{
    public bool impacto;
    public bool hundido;
    public int fila;
    int columna;

    public ResultadoDisparo(bool impacto, bool hundido, int fila, int columna)
    {
        this.impacto = impacto;
        this.hundido = hundido;
        this.fila = fila;
        this.columna = columna;
    }
}