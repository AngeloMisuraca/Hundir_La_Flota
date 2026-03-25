class Tablero
{
    bool todosHundidos;
    int barcosRestantes;
    List<Barco> barcos = new List<Barco>();
    Casilla[,] casillas = new Casilla[10, 10];

    public Tablero(bool todosHundidos, int barcosRestantes)
    {
        this.todosHundidos = todosHundidos;
        this.barcosRestantes = barcosRestantes;

        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                casillas[fila, columna] = new Casilla(fila, columna, false);
            }
        }
    }

    public bool FlotaHundida(Barco EstaHundido)
    {
        foreach (Barco barco in barcos)
        {
            if (barco != EstaHundido)
            {
                return false;
            }
        }
        return true;
    }

    public int BarcosRestantes(Barco EstaHundido)
    {
        int Contador = 0;

        foreach (Barco barco in barcos)
        {
            if (barco != EstaHundido)
            {
                Contador++;
            }
        }
        return Contador;
    }
    public Casilla ObtenerCasilla(int fila, int columna)
    {
        return casillas[fila, columna];
    }

    public bool PuedeColocar(Barco barco, int fila, int columna, bool esHorizontal)
    {
        for (int i = 0; i < barco.tamanio; i++)
        {
            if (esHorizontal)
            {
                columna = columna + i;
            }
            else
            {
                fila = fila + i;
            }   

            if (fila < 0 || fila >= 10 || columna < 0 || columna >= 10)
            {
                return false;
            }
                
            for (int despFila = -1; despFila <= 1; despFila++)
            {
                for (int despColumna = -1; despColumna <= 1; despColumna++)
                {
                    int nuevaFila = fila + despFila;
                    int nuevaColumna = columna + despColumna;
                    if (nuevaFila < 0 || nuevaFila >= 10 || nuevaColumna < 0 || nuevaColumna >= 10)
                    {
                        continue;
                    }
                        
                    if (casillas[nuevaFila, nuevaColumna].EstaVacia() != true)
                    {
                        return false;
                    }    
                }
            }
        }
        return true;
    }

    public bool ColocarBarco(Barco barco, int fila, int columna, bool esHorizontal)
    {
        for (int i = 0; i < barco.tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            if (esHorizontal)
                filaActual = fila + i;
            else
                columnaActual = columna + i;

            if (filaActual < 0 || filaActual >= 10 || columnaActual < 0 || columnaActual >= 10)
            {
                return false;
            }

            for (int despFila = -1; despFila <= 1; despFila++)
            {
                for (int despColumna = -1; despColumna <= 1; despColumna++)
                {
                    int nuevaFila = filaActual + despFila;
                    int nuevaColumna = columnaActual + despColumna;
                    if (nuevaFila < 0 || nuevaFila >= 10 || nuevaColumna < 0 || nuevaColumna >= 10)
                    {
                        continue;
                    }
                    if (!casillas[nuevaFila, nuevaColumna].EstaVacia())
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void Disparar(int fila, int columna)
    {
        
    }
}
