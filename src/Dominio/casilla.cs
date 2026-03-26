public class Casilla
{
    public int fila { get; set; }
    public int columna { get; set; }
    public bool disparada { get; set; }
    public List<Barco> barcos { get; set; }
    public Barco barco { get; set; }

    public Casilla(int fila, int columna, bool disparada)
    {
        this.fila = fila;
        this.columna = columna;
        this.disparada = disparada;
        this.barcos = new List<Barco>();
    }

    public bool EstaVacia()
    {
        if (barcos.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EsImpacto()
    {
        if (disparada == true && barcos.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EsAgua()
    {
        if (disparada == true && barcos.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}