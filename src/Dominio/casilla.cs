public class Casilla
{
    // Posicion fija de la casilla dentro del tablero.
    public int Fila { get; }
    public int Columna { get; }

    // Indica si ya se disparo a esta casilla.
    public bool Disparada { get; private set; }

    // Guarda el barco que ocupa esta casilla. Si es null, esta vacia.
    public Barco? Barco { get; private set; }

    public Casilla(int fila, int columna, bool disparada)
    {
        // Guardamos la posicion y el estado inicial.
        Fila = fila;
        Columna = columna;
        Disparada = disparada;
    }

    public bool EstaVacia()
    {
        // Devuelve true cuando no hay ningun barco en esta casilla.
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
        // Hay impacto si se ha disparado y habia un barco en esa casilla.
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
        // Hay agua si se ha disparado y la casilla estaba vacia.
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
        // Asociamos esta casilla a un barco.
        Barco = barco;
    }

    public void MarcarDisparo()
    {
        // Marcamos que esta casilla ya ha sido atacada.
        Disparada = true;
    }
}
