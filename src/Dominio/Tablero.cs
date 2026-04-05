class Tablero
{
    // Estado general del tablero.
    bool todosHundidos;
    int barcosRestantes;

    // Lista de barcos colocados y matriz de casillas de 10 x 10.
    List<Barco> barcos = new List<Barco>();
    Casilla[,] casillas = new Casilla[10, 10];

    public bool TodosHundidos
    {
        get
        {
            return todosHundidos;
        }
    }

    public int BarcosRestantes
    {
        get
        {
            return barcosRestantes;
        }
    }

    public List<Barco> ObtenerBarcos()
    {
        // Devuelve la lista de barcos del tablero.
        return barcos;
    }

    public Tablero(bool todosHundidos, int barcosRestantes)
    {
        // Guardamos el estado recibido.
        this.todosHundidos = todosHundidos;
        this.barcosRestantes = barcosRestantes;

        // Creamos las 100 casillas del tablero.
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
        // Este metodo comprueba si todos los barcos menos uno ya estan hundidos.
        foreach (Barco barco in barcos)
        {
            if (barco != EstaHundido)
            {
                return false;
            }
        }
        return true;
    }

    public int ContarBarcosRestantes(Barco EstaHundido)
    {
        // Cuenta cuantos barcos quedan sin considerar el barco recibido.
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
        // Devuelve una casilla concreta del tablero.
        return casillas[fila, columna];
    }

    public bool PuedeColocar(Barco barco, int fila, int columna, bool esHorizontal)
    {
        // Recorremos todas las casillas que ocuparia el barco.
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            // Calculamos la posicion real segun la orientacion.
            if (esHorizontal)
            {
                columnaActual = columna + i;
            }
            else
            {
                filaActual = fila + i;
            }

            // Si alguna parte se sale del tablero, no se puede colocar.
            if (filaActual < 0 || filaActual >= 10 || columnaActual < 0 || columnaActual >= 10)
            {
                return false;
            }

            // Revisamos alrededor de cada casilla para aplicar la regla de adyacencia.
            for (int despFila = -1; despFila <= 1; despFila++)
            {
                for (int despColumna = -1; despColumna <= 1; despColumna++)
                {
                    int nuevaFila = filaActual + despFila;
                    int nuevaColumna = columnaActual + despColumna;
                    // Si la coordenada vecina se sale del tablero, la ignoramos.
                    if (nuevaFila < 0 || nuevaFila >= 10 || nuevaColumna < 0 || nuevaColumna >= 10)
                    {
                        continue;
                    }

                    // Si hay un barco cerca, no se puede colocar.
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
        // Si la posicion no es valida, salimos.
        if (PuedeColocar(barco, fila, columna, esHorizontal) == false)
        {
            return false;
        }

        // Limpiamos las casillas anteriores del barco por si se reutiliza.
        barco.Casillas.Clear();

        // Colocamos cada parte del barco dentro del tablero.
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            // Calculamos la casilla exacta segun la orientacion.
            if (esHorizontal)
            {
                columnaActual = columna + i;
            }
            else
            {
                filaActual = fila + i;
            }

            // Asociamos la casilla con el barco.
            casillas[filaActual, columnaActual].AsignarBarco(barco);
            barco.Casillas.Add(casillas[filaActual, columnaActual]);
        }

        // Añadimos el barco a la lista si todavia no estaba guardado.
        if (barcos.Contains(barco) == false)
        {
            barcos.Add(barco);
        }

        return true;
    }

    public ResultadoDisparo Disparar(int fila, int columna)
    {
        // Obtenemos la casilla a la que se quiere disparar.
        Casilla casilla = casillas[fila, columna];

        // Si ya se habia disparado ahi, devolvemos ese resultado.
        if (casilla.Disparada)
        {
            return ResultadoDisparo.YaDisparado;
        }

        // Marcamos el disparo.
        casilla.MarcarDisparo();

        // Si la casilla esta vacia, el disparo cae al agua.
        if (casilla.EstaVacia())
        {
            return ResultadoDisparo.Agua;
        }

        // Si hay barco, sumamos un impacto.
        Barco barcoImpactado = casilla.Barco!;
        barcoImpactado.RecibirImpacto();

        // Si el barco ya no aguanta mas impactos, queda hundido.
        if (barcoImpactado.EstaHundido())
        {
            barcosRestantes--;

            // Si no quedan barcos, toda la flota esta hundida.
            if (barcosRestantes <= 0)
            {
                barcosRestantes = 0;
                todosHundidos = true;
            }

            return ResultadoDisparo.Hundido;
        }
        else
        {
            return ResultadoDisparo.Impacto;
        }
    }

    public static Tablero CrearDesdeEstado(TableroGuardado estado)
    {
        // Primero calculamos cuantos barcos quedan vivos.
        int barcosRestantes = 0;

        foreach (BarcoGuardado barcoGuardado in estado.Barcos)
        {
            if (barcoGuardado.Impactos < barcoGuardado.Tamanio)
            {
                barcosRestantes++;
            }
        }

        // Con esa informacion creamos el tablero base.
        bool todosHundidos = barcosRestantes == 0;
        Tablero tablero = new Tablero(todosHundidos, barcosRestantes);

        // Reconstruimos cada barco y lo volvemos a colocar en sus casillas.
        foreach (BarcoGuardado barcoGuardado in estado.Barcos)
        {
            Barco barco = new Barco(barcoGuardado.Nombre, barcoGuardado.Tamanio, barcoGuardado.Impactos);

            foreach (CoordenadaGuardada coordenada in barcoGuardado.Casillas)
            {
                Casilla casilla = tablero.casillas[coordenada.Fila, coordenada.Columna];
                casilla.AsignarBarco(barco);
                barco.Casillas.Add(casilla);
            }

            tablero.barcos.Add(barco);
        }

        // Marcamos las casillas que ya habian sido disparadas.
        foreach (CoordenadaGuardada coordenada in estado.CasillasDisparadas)
        {
            tablero.casillas[coordenada.Fila, coordenada.Columna].MarcarDisparo();
        }

        return tablero;
    }
}
