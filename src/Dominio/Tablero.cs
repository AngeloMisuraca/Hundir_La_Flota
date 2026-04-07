class Tablero
{
    // Aqui guardo el estado general del tablero.
    bool todosHundidos;
    int barcosRestantes;

    // Aqui van los barcos y las 100 casillas.
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
        // Devuelvo los barcos del tablero.
        return barcos;
    }

    public Tablero(bool todosHundidos, int barcosRestantes)
    {
        // Guardo el estado inicial.
        this.todosHundidos = todosHundidos;
        this.barcosRestantes = barcosRestantes;

        // Creo las 100 casillas del tablero.
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
        // Compruebo si solo quedaria ese barco.
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
        // Cuenta cuantos barcos quedan sin contar ese.
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
        // Devuelve la casilla que toque.
        return casillas[fila, columna];
    }

    public bool PuedeColocar(Barco barco, int fila, int columna, bool esHorizontal)
    {
        // Reviso todas las casillas que pisaria el barco.
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            // Ajusto fila y columna segun la direccion.
            if (esHorizontal)
            {
                columnaActual = columna + i;
            }
            else
            {
                filaActual = fila + i;
            }

            // Si se sale del tablero, no vale.
            if (filaActual < 0 || filaActual >= 10 || columnaActual < 0 || columnaActual >= 10)
            {
                return false;
            }

            // Tambien miro alrededor para que no toque otro barco.
            for (int despFila = -1; despFila <= 1; despFila++)
            {
                for (int despColumna = -1; despColumna <= 1; despColumna++)
                {
                    int nuevaFila = filaActual + despFila;
                    int nuevaColumna = columnaActual + despColumna;

                    // Si se sale, esa casilla no me importa.
                    if (nuevaFila < 0 || nuevaFila >= 10 || nuevaColumna < 0 || nuevaColumna >= 10)
                    {
                        continue;
                    }

                    // Si hay un barco cerca, esta posicion no sirve.
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
        // Si no se puede poner ahi, salgo.
        if (PuedeColocar(barco, fila, columna, esHorizontal) == false)
        {
            return false;
        }

        // Limpio las casillas anteriores por si el barco ya se uso antes.
        barco.Casillas.Clear();

        // Voy colocando cada parte del barco.
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            // Ajusto la casilla segun si va horizontal o vertical.
            if (esHorizontal)
            {
                columnaActual = columna + i;
            }
            else
            {
                filaActual = fila + i;
            }

            // Uno la casilla con el barco.
            casillas[filaActual, columnaActual].AsignarBarco(barco);
            barco.Casillas.Add(casillas[filaActual, columnaActual]);
        }

        // Si ese barco no estaba metido todavia, lo guardo.
        if (barcos.Contains(barco) == false)
        {
            barcos.Add(barco);
        }

        return true;
    }

    public ResultadoDisparo Disparar(int fila, int columna)
    {
        // Cojo la casilla a la que se dispara.
        Casilla casilla = casillas[fila, columna];

        // Si ahi ya se disparo antes, lo devuelvo tal cual.
        if (casilla.Disparada)
        {
            return ResultadoDisparo.YaDisparado;
        }

        // Marco el disparo.
        casilla.MarcarDisparo();

        // Si no habia barco, es agua.
        if (casilla.EstaVacia())
        {
            return ResultadoDisparo.Agua;
        }

        // Si habia barco, sumo el impacto.
        Barco barcoImpactado = casilla.Barco!;
        barcoImpactado.RecibirImpacto();

        // Si ya llego al limite, se hunde.
        if (barcoImpactado.EstaHundido())
        {
            barcosRestantes--;

            // Si no queda ninguno, la partida en ese tablero acaba.
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
        // Primero cuento cuantos barcos siguen vivos.
        int barcosRestantes = 0;

        foreach (BarcoGuardado barcoGuardado in estado.Barcos)
        {
            if (barcoGuardado.Impactos < barcoGuardado.Tamanio)
            {
                barcosRestantes++;
            }
        }

        // Con eso creo el tablero base.
        bool todosHundidos = barcosRestantes == 0;
        Tablero tablero = new Tablero(todosHundidos, barcosRestantes);

        // Vuelvo a montar los barcos en sus casillas.
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

        // Marco tambien las casillas que ya tenian disparo.
        foreach (CoordenadaGuardada coordenada in estado.CasillasDisparadas)
        {
            tablero.casillas[coordenada.Fila, coordenada.Columna].MarcarDisparo();
        }

        return tablero;
    }
}
