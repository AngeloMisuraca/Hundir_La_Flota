class Renderizador
{
    // Constantes para dibujar los marcos de la interfaz.
    const int anchoMarco = 75;
    const string bordeVertical = "\u2551";
    const string bordeSuperiorIzquierdo = "\u2554";
    const string bordeSuperiorDerecho = "\u2557";
    const string bordeInferiorIzquierdo = "\u255A";
    const string bordeInferiorDerecho = "\u255D";
    const string bordeSeparadorIzquierdo = "\u2560";
    const string bordeSeparadorDerecho = "\u2563";
    const char bordeHorizontal = '\u2550';

    public int MostrarBienvenida(bool hayPartidaGuardada)
    {
        // Limpiamos la pantalla y mostramos logo + menu.
        LimpiarPantalla();
        ArteAscii arteAscii = new ArteAscii();
        arteAscii.ArteTexto();
        Console.WriteLine();
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Nueva partida");
        // La opcion "Continuar" cambia segun exista o no una partida guardada.
        if (hayPartidaGuardada)
        {
            Console.WriteLine("2. Continuar");
        }
        else
        {
            Console.WriteLine("2. Continuar (sin partida guardada)");
        }
        Console.WriteLine("3. Records");
        Console.WriteLine("4. Opciones");
        Console.WriteLine("5. Salir");
        Console.Write("Elige una opcion: ");

        // Leemos la opcion del usuario.
        string opcionTexto = Console.ReadLine() ?? "1";
        int opcion = 1;
        int.TryParse(opcionTexto, out opcion);

        // Devolvemos la opcion elegida.
        switch (opcion)
        {
            case 1:
                Console.ForegroundColor = Colores.exito;
                Console.WriteLine("Iniciando nueva partida...");
                Console.ResetColor();
                return 1;
            case 2:
                if (hayPartidaGuardada == false)
                {
                    MostrarError("No hay ninguna partida guardada.");
                    return 0;
                }

                return 2;
            case 3:
                return 3;
            case 4:
                Console.WriteLine("Opciones no estan disponibles todavia.");
                return 4;
            case 5:
                return 5;
            default:
                MostrarError("Opcion no valida");
                return 1;
        }
    }

    public int MostrarMenuOpciones()
    {
        // Limpiamos la pantalla y mostramos un submenu con opciones sencillas.
        LimpiarPantalla();
        Console.WriteLine("Opciones:");
        Console.WriteLine("1. Eliminar todas las partidas");
        Console.WriteLine("2. Mostrar nombres de los barcos");
        Console.WriteLine("3. Volver al menu principal");
        Console.Write("Elige una opcion: ");

        // Leemos la opcion elegida por el usuario.
        string opcionTexto = Console.ReadLine() ?? "3";
        int opcion = 3;
        int.TryParse(opcionTexto, out opcion);
        return opcion;
    }

    public void MostrarNombresBarcos(List<Barco> barcos)
    {
        // Mostramos una lista simple con los nombres y tamanios de la flota del juego.
        LimpiarPantalla();
        Console.WriteLine("Nombres de los barcos:");
        Console.WriteLine();

        for (int i = 0; i < barcos.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + barcos[i].Nombre + " - Tamano: " + barcos[i].Tamanio);
        }

        Console.WriteLine();
        Console.WriteLine("Pulsa Enter para volver al menu de opciones...");
        Console.ReadLine();
    }

    public void MostrarTablerosBatalla(Jugador jugador, Tablero enemigo)
    {
        // Letras para etiquetar las filas del tablero.
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        Tablero propio = jugador.Tablero;
        int barcosHundidos = 5 - enemigo.BarcosRestantes;

        // Dibujamos el marco principal de la batalla.
        LimpiarPantalla();
        EscribirBordeSuperior();
        EscribirLineaCentrada("HUNDIR LA FLOTA");
        EscribirSeparador();
        EscribirLineaMarco("    TU TABLERO                          MAR ENEMIGO");
        EscribirCabeceraDoble();

        // Dibujamos ambos tableros fila a fila.
        for (int fila = 0; fila < 10; fila++)
        {
            Console.Write(bordeVertical);
            Console.Write($"  {letras[fila]}   ");

            for (int columna = 0; columna < 10; columna++)
            {
                ImprimirCasilla(propio.ObtenerCasilla(fila, columna), true);
            }

            Console.Write($"   {letras[fila]}   ");

            for (int columna = 0; columna < 10; columna++)
            {
                ImprimirCasilla(enemigo.ObtenerCasilla(fila, columna), false);
            }

            Console.WriteLine("  " + bordeVertical);
        }

        // Al final mostramos las estadisticas de la partida.
        EscribirSeparador();
        EscribirLineaMarco("Disparos: " + jugador.Disparos + "   Aciertos: " + jugador.Aciertos + "   Fallos: " + jugador.Fallos + "   Precision: " + jugador.Precision.ToString("0.0") + " %");
        EscribirLineaMarco("Barcos hundidos: " + barcosHundidos + " / 5       Barcos enemigos restantes: " + enemigo.BarcosRestantes);
        EscribirBordeInferior();
        Console.WriteLine("  [ S = barco propio   X = impacto   ~ = agua   . = vacio ]");
        Console.WriteLine();
    }

    public void MostrarTableroColocacion(Tablero tablero, Barco barco, int filaPreview = -1, int columnaPreview = -1, bool esHorizontalPreview = true, bool mostrarPreview = false, bool previewValida = false)
    {
        // Este metodo enseña el tablero mientras se colocan los barcos.
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        LimpiarPantalla();
        EscribirBordeSuperior();
        EscribirLineaCentrada("COLOCACION DE BARCOS");
        EscribirSeparador();
        EscribirLineaMarco("Coloca tu " + barco.Nombre + " (" + barco.Tamanio + " casillas)");
        EscribirLineaMarco("R = rotar   .   Enter = confirmar");
        EscribirSeparador();

        Console.Write(bordeVertical + "      ");
        for (int columna = 1; columna <= 10; columna++)
        {
            Console.Write($"{columna,3}");
        }
        Console.WriteLine(new string(' ', 39) + bordeVertical);

        // Dibujamos el tablero de colocacion.
        for (int fila = 0; fila < 10; fila++)
        {
            Console.Write($"{bordeVertical}  {letras[fila]}   ");

            for (int columna = 0; columna < 10; columna++)
            {
                bool esPreview = EsCasillaPreview(fila, columna, filaPreview, columnaPreview, barco.Tamanio, esHorizontalPreview, mostrarPreview);

                // Si esta casilla forma parte de la vista previa, dibujamos ?.
                if (esPreview)
                {
                    ImprimirPreview(previewValida);
                }
                else
                {
                    ImprimirCasilla(tablero.ObtenerCasilla(fila, columna), true);
                }
            }

            Console.WriteLine(new string(' ', 39) + bordeVertical);
        }

        EscribirBordeInferior();
        Console.WriteLine("Coloca tu " + barco.Nombre + " (" + barco.Tamanio + " casillas)");
        Console.WriteLine("Escribe una coordenada inicial como A1 y luego H o V.");
    }

    static void ImprimirCasilla(Casilla casilla, bool esPropio)
    {
        // Elegimos el simbolo segun el estado de la casilla.
        if (casilla.EsImpacto())
        {
            if (casilla.Barco != null && casilla.Barco.EstaHundido())
            {
                Console.ForegroundColor = Colores.hundido;
                Console.Write("  #");
            }
            else
            {
                Console.ForegroundColor = Colores.impacto;
                Console.Write("  X");
            }
        }
        else if (casilla.EsAgua())
        {
            Console.ForegroundColor = Colores.agua;
            Console.Write("  ~");
        }
        else if (!casilla.EstaVacia() && esPropio)
        {
            Console.ForegroundColor = Colores.barcoPropio;
            Console.Write("  S");
        }
        else
        {
            Console.ForegroundColor = Colores.casillaVacia;
            Console.Write("  .");
        }

        Console.ResetColor();
    }

    static void ImprimirPreview(bool previewValida)
    {
        // El preview se muestra en verde si es valido y en rojo si no lo es.
        if (previewValida)
        {
            Console.ForegroundColor = Colores.exito;
        }
        else
        {
            Console.ForegroundColor = Colores.error;
        }

        Console.Write("  ?");
        Console.ResetColor();
    }

    public (int fila, int columna, bool esHorizontal) PedirPosicion(Barco barco)
    {
        // Pedimos una coordenada y una orientacion para colocar un barco.
        Console.Write("Posicion: ");

        string posicion = Console.ReadLine() ?? "";

        // Repetimos hasta que el formato sea correcto.
        while (PosicionValida(posicion) == false)
        {
            MostrarError("Formato no valido. Ejemplo correcto: A1 H");
            Console.Write("Posicion: ");
            posicion = Console.ReadLine() ?? "";
        }

        Console.ForegroundColor = Colores.exito;
        Console.WriteLine("Posicion elegida: " + posicion.ToUpper());
        Console.ResetColor();

        // Convertimos el texto en datos utiles para el juego.
        string[] partes = posicion.Trim().ToUpper().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        (int fila, int columna) coordenada = ConvertirTextoACoordenada(partes[0]);
        bool esHorizontal = partes[1] == "H";
        return (coordenada.fila, coordenada.columna, esHorizontal);
    }

    public bool PedirConfirmacionColocacion()
    {
        // Pedimos una confirmacion clara para evitar confusiones.
        Console.Write("Confirmar colocacion? (S/N): ");
        string texto = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (texto == "S" || texto == "SI")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PedirGuardarPartida()
    {
        // Al acabar una ronda, preguntamos si se quiere guardar y salir al menu principal.
        Console.Write("Quieres guardar y salir al menu principal? (S/N): ");
        string texto = (Console.ReadLine() ?? "").Trim().ToUpper();

        // Repetimos hasta que el usuario escriba una respuesta valida.
        while (texto != "S" && texto != "SI" && texto != "N" && texto != "NO")
        {
            MostrarError("Respuesta no valida. Escribe S o N.");
            Console.Write("Quieres guardar y salir al menu principal? (S/N): ");
            texto = (Console.ReadLine() ?? "").Trim().ToUpper();
        }

        if (texto == "S" || texto == "SI")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public (int fila, int columna) PedirCoordenada()
    {
        // Pedimos una coordenada de disparo.
        Console.Write("Coordenada (ej. B7): ");
        string texto = Console.ReadLine() ?? "";

        // Repetimos hasta que el formato sea correcto.
        while (CoordenadaValida(texto) == false)
        {
            MostrarError("Coordenada no valida. Usa letras de A a J y numeros de 1 a 10.");
            Console.Write("Coordenada (ej. B7): ");
            texto = Console.ReadLine() ?? "";
        }

        Console.ForegroundColor = Colores.exito;
        Console.WriteLine("Coordenada aceptada: " + texto.ToUpper());
        Console.ResetColor();
        return ConvertirTextoACoordenada(texto);
    }

    public void MostrarResultadoDisparo(ResultadoDisparo resultado, int fila, int columna)
    {
        // Mostramos el resultado del disparo del jugador.
        string coordenada = ConvertirCoordenada(fila, columna);
        Console.Write("Tu disparo en " + coordenada + ": ");

        if (resultado == ResultadoDisparo.Hundido)
        {
            Console.ForegroundColor = Colores.hundido;
            Console.WriteLine("hundido");
        }
        else if (resultado == ResultadoDisparo.Impacto)
        {
            Console.ForegroundColor = Colores.impacto;
            Console.WriteLine("impacto");
        }
        else if (resultado == ResultadoDisparo.Agua)
        {
            Console.ForegroundColor = Colores.agua;
            Console.WriteLine("agua");
        }
        else if (resultado == ResultadoDisparo.YaDisparado)
        {
            Console.ForegroundColor = Colores.error;
            Console.WriteLine("ya habias disparado ahi");
        }

        Console.ResetColor();
        EsperarResultado();
    }

    public void MostrarResultadoCpu(ResultadoDisparo resultado, int fila, int columna)
    {
        // Mostramos el resultado del disparo de la CPU.
        string coordenada = ConvertirCoordenada(fila, columna);
        Console.Write("CPU dispara a " + coordenada + ": ");

        if (resultado == ResultadoDisparo.Impacto || resultado == ResultadoDisparo.Hundido)
        {
            Console.ForegroundColor = Colores.impacto;
            Console.WriteLine("impacto");
        }
        else
        {
            Console.ForegroundColor = Colores.agua;
            Console.WriteLine("agua");
        }

        Console.ResetColor();
        EsperarResultado();
    }

    public void MostrarDisparoCpu(ResultadoDisparo resultado, int fila, int columna)
    {
        MostrarResultadoCpu(resultado, fila, columna);
    }

    public void MostrarResultadoFinal(bool ganaJugador, Jugador jugador)
    {
        // Mostramos pantalla de victoria o derrota con estadisticas.
        Console.WriteLine();

        if (ganaJugador)
        {
            Console.ForegroundColor = Colores.exito;
            Console.WriteLine(ArteAscii.victoria);
            Console.WriteLine("Has ganado la partida.");
        }
        else
        {
            Console.ForegroundColor = Colores.error;
            Console.WriteLine(ArteAscii.derrota);
            Console.WriteLine("Has perdido la partida.");
        }

        Console.ResetColor();
        Console.WriteLine("Jugador: " + jugador.Nombre);
        Console.WriteLine("Disparos: " + jugador.Disparos);
        Console.WriteLine("Aciertos: " + jugador.Aciertos);
        Console.WriteLine("Fallos: " + jugador.Fallos);
        Console.WriteLine("Precision: " + jugador.Precision.ToString("0.0") + " %");
    }

    public void MostrarRecords(List<EntradaMarcador> entradas)
    {
        // Dibujamos la tabla del ranking.
        LimpiarPantalla();
        EscribirBordeSuperior();
        EscribirLineaCentrada("RECORDS");
        EscribirSeparador();

        // Si no hay registros, mostramos un mensaje simple.
        if (entradas.Count == 0)
        {
            EscribirLineaMarco("Todavia no hay partidas registradas.");
        }
        else
        {
            EscribirLineaMarco("Pos  Jugador        Disparos  Precision  Puntos    Fecha");
            EscribirSeparador();

            int limite = entradas.Count;

            if (limite > 10)
            {
                limite = 10;
            }

            // Si hay registros, mostramos hasta 10 lineas.
            for (int i = 0; i < limite; i++)
            {
                EntradaMarcador entrada = entradas[i];
                string linea =
                    (i + 1).ToString().PadLeft(2) + "   " +
                    entrada.nombreJugador.PadRight(12) + " " +
                    entrada.disparos.ToString().PadLeft(8) + " " +
                    (entrada.precision.ToString("0.0") + " %").PadLeft(10) + " " +
                    entrada.puntuacion.ToString("0.00").PadLeft(8) + " " +
                    entrada.fecha.ToString("dd/MM/yyyy");

                EscribirLineaMarco(linea);
            }
        }

        EscribirBordeInferior();
        Console.WriteLine("Pulsa Enter para volver al menu...");
        Console.ReadLine();
    }

    public void MostrarError(string mensaje)
    {
        // Los errores se muestran en rojo.
        Console.ForegroundColor = Colores.error;
        Console.WriteLine(mensaje);
        Console.ResetColor();
    }

    public void MostrarMensaje(string mensaje)
    {
        // Los mensajes informativos se muestran en verde para diferenciarlos de los errores.
        Console.ForegroundColor = Colores.exito;
        Console.WriteLine(mensaje);
        Console.ResetColor();
    }

    public void EsperarVolverAlMenu()
    {
        // Pausa simple para que el usuario lea la pantalla.
        Console.WriteLine("Pulsa Enter para volver al menu...");
        Console.ReadLine();
    }

    public void EsperarContinuar()
    {
        // Pausa simple para continuar el juego.
        Console.WriteLine("Pulsa Enter para continuar...");
        Console.ReadLine();
    }

    public void EsperarReintento()
    {
        // Pausa simple para recolocar o volver a probar.
        Console.WriteLine("Pulsa Enter para intentarlo otra vez...");
        Console.ReadLine();
    }

    bool CoordenadaValida(string texto)
    {
        // Validamos entradas como A1, B7 o J10.
        texto = texto.Trim().ToUpper();

        if (texto.Length < 2 || texto.Length > 3)
        {
            return false;
        }

        char fila = texto[0];
        if (fila < 'A' || fila > 'J')
        {
            return false;
        }

        string numeroTexto = texto.Substring(1);
        int columna = 0;

        if (int.TryParse(numeroTexto, out columna) == false)
        {
            return false;
        }

        return columna >= 1 && columna <= 10;
    }

    bool PosicionValida(string texto)
    {
        // Validamos entradas como A1 H o C5 V.
        string[] partes = texto.Trim().ToUpper().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length != 2)
        {
            return false;
        }

        if (CoordenadaValida(partes[0]) == false)
        {
            return false;
        }

        return partes[1] == "H" || partes[1] == "V";
    }

    string ConvertirCoordenada(int fila, int columna)
    {
        // Convierte una fila y columna numericas en texto tipo A1.
        char letra = (char)('A' + fila);
        return letra + (columna + 1).ToString();
    }

    (int fila, int columna) ConvertirTextoACoordenada(string texto)
    {
        // Convierte texto como B7 en indices internos del tablero.
        texto = texto.Trim().ToUpper();
        int fila = texto[0] - 'A';
        int columna = Convert.ToInt32(texto.Substring(1)) - 1;
        return (fila, columna);
    }

    void EscribirBordeSuperior()
    {
        // Dibuja el borde de arriba del marco.
        Console.WriteLine(bordeSuperiorIzquierdo + new string(bordeHorizontal, anchoMarco) + bordeSuperiorDerecho);
    }

    void EscribirBordeInferior()
    {
        // Dibuja el borde de abajo del marco.
        Console.WriteLine(bordeInferiorIzquierdo + new string(bordeHorizontal, anchoMarco) + bordeInferiorDerecho);
    }

    void EscribirSeparador()
    {
        // Dibuja una linea de separacion horizontal.
        Console.WriteLine(bordeSeparadorIzquierdo + new string(bordeHorizontal, anchoMarco) + bordeSeparadorDerecho);
    }

    void EscribirLineaCentrada(string texto)
    {
        // Escribe una linea centrada dentro del marco.
        int espaciosIzquierda = (anchoMarco - texto.Length) / 2;
        int espaciosDerecha = anchoMarco - texto.Length - espaciosIzquierda;

        Console.Write(bordeVertical);
        Console.Write(new string(' ', espaciosIzquierda));
        Console.ForegroundColor = Colores.titulo;
        Console.Write(texto);
        Console.ResetColor();
        Console.Write(new string(' ', espaciosDerecha));
        Console.WriteLine(bordeVertical);
    }

    void EscribirLineaMarco(string texto)
    {
        // Escribe una linea normal dentro del marco.
        if (texto.Length > anchoMarco)
        {
            texto = texto.Substring(0, anchoMarco);
        }

        Console.Write(bordeVertical);
        Console.Write(texto.PadRight(anchoMarco));
        Console.WriteLine(bordeVertical);
    }

    void EscribirCabeceraDoble()
    {
        // Cabecera con los numeros de columnas de los dos tableros.
        Console.Write(bordeVertical + "      ");
        for (int columna = 1; columna <= 10; columna++)
        {
            Console.Write($"{columna,3}");
        }

        Console.Write("       ");

        for (int columna = 1; columna <= 10; columna++)
        {
            Console.Write($"{columna,3}");
        }

        Console.WriteLine("  " + bordeVertical);
    }

    void LimpiarPantalla()
    {
        // Intentamos limpiar la consola, pero evitamos que el juego se rompa si falla.
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
        }
    }

    void EsperarResultado()
    {
        // Pausa de 2 segundos para leer el resultado del disparo.
        Thread.Sleep(1000);
    }

    bool EsCasillaPreview(int fila, int columna, int filaPreview, int columnaPreview, int tamanio, bool esHorizontalPreview, bool mostrarPreview)
    {
        // Comprueba si una casilla forma parte del preview del barco.
        if (mostrarPreview == false)
        {
            return false;
        }

        for (int i = 0; i < tamanio; i++)
        {
            int filaActual = filaPreview;
            int columnaActual = columnaPreview;

            if (esHorizontalPreview)
            {
                columnaActual = columnaPreview + i;
            }
            else
            {
                filaActual = filaPreview + i;
            }

            if (fila == filaActual && columna == columnaActual)
            {
                return true;
            }
        }

        return false;
    }
}
