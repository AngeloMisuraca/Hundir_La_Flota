public class Casilla
{
    // Esta es la posicion de la casilla.
    public int Fila { get; }
    public int Columna { get; }

    // Esto dice si aqui ya se disparo.
    public bool Disparada { get; private set; }

    // Si hay barco, queda guardado aqui.
    public Barco? Barco { get; private set; }

    public Casilla(int fila, int columna, bool disparada)
    {
        // Guardo la posicion y si estaba disparada o no.
        Fila = fila;
        Columna = columna;
        Disparada = disparada;
    }

    public bool EstaVacia()
    {
        // Si no hay barco, esta vacia.
        if (Barco == null)
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
        // Solo es impacto si ya se disparo y habia barco.
        if (Disparada == true && Barco != null)
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
        // Solo es agua si ya se disparo y no habia barco.
        if (Disparada == true && Barco == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AsignarBarco(Barco barco)
    {
        // Le asigno el barco a esta casilla.
        Barco = barco;
    }

    public void MarcarDisparo()
    {
        // Marco que aqui ya se disparo.
        Disparada = true;
    }
}
