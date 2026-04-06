class Juego
{
    // Fases internas del juego.
    enum FaseJuego
    {
        Colocacion,
        Batalla,
        Terminado
    }

    // Objetos principales del juego.
    Jugador jugador;
    Cpu cpu;
    Renderizador renderizador;
    GestorGuardado gestorGuardado;
    Marcador marcador;
    ConfigJuego configJuego;
    FaseJuego faseActual;
    bool turnoJugador;
    List<Barco> flotaJugador;
    List<Barco> flotaCpu;

    public Juego()
    {
        // Creamos los objetos base del juego.
        renderizador = new Renderizador();
        gestorGuardado = new GestorGuardado();
        marcador = new Marcador();
        configJuego = ConfigJuego.Cargar();
        jugador = new Jugador(configJuego.NombreJugador, new Tablero(false, 5));
        cpu = new Cpu("CPU", new Tablero(false, 5));
        faseActual = FaseJuego.Colocacion;
        turnoJugador = true;
        flotaJugador = new List<Barco>();
        flotaCpu = new List<Barco>();
    }

    public void Iniciar()
    {
        // Repetimos el menu principal hasta que el usuario elija salir del programa.
        bool salirDelPrograma = false;

        while (salirDelPrograma == false)
        {
            int opcion = 0;

            // Repetimos el menu hasta elegir una opcion valida de juego o salir.
            while (opcion != 1 && opcion != 2 && opcion != 5)
            {
                opcion = renderizador.MostrarBienvenida(gestorGuardado.ExistePartidaGuardada());

                // Mostramos records si el usuario elige esa opcion.
                if (opcion == 3)
                {
                    marcador.Cargar();
                    renderizador.MostrarRecords(marcador.ObtenerEntradas());
                }
                else if (opcion == 4)
                {
                    MostrarOpciones();
                }
                else if (opcion == 5)
                {
                    salirDelPrograma = true;
                }
            }

            if (salirDelPrograma)
            {
                return;
            }

            // Si se elige continuar, intentamos cargar una partida guardada.
            if (opcion == 2)
            {
                bool partidaCargada = CargarPartida();

                if (partidaCargada == false)
                {
                    renderizador.MostrarError("No hay ninguna partida guardada.");
                    renderizador.EsperarVolverAlMenu();
                    continue;
                }

                // Avisamos de que la partida guardada se ha cargado y vamos a volver al tablero.
                renderizador.MostrarMensaje("Cargando partida guardada...");
            }
            else
            {
                // Si no, empezamos una partida nueva.
                Colocacion();
            }

            // Una vez preparada la partida, jugamos.
            bool partidaTerminada = Batalla();

            // Solo mostramos el final si realmente se ha terminado la partida.
            if (partidaTerminada)
            {
                Terminado();
            }
        }
    }

    void ReiniciarEstadoPartida()
    {
        // Preparamos todos los objetos para empezar una partida nueva desde cero.
        jugador = new Jugador(configJuego.NombreJugador, new Tablero(false, 5));
        cpu = new Cpu("CPU", new Tablero(false, 5));
        faseActual = FaseJuego.Colocacion;
        turnoJugador = true;
        flotaJugador = new List<Barco>();
        flotaCpu = new List<Barco>();
    }

    public void Colocacion()
    {
        // Antes de colocar barcos, dejamos la partida en un estado limpio.
        ReiniciarEstadoPartida();

        // Creamos una flota para el jugador y otra para la CPU.
        flotaJugador = Flota.CrearFlota();
        flotaCpu = Flota.CrearFlota();

        // El jugador coloca su flota y la CPU coloca la suya automaticamente.
        ColocarFlotaJugador();
        cpu.ColocarFlotaAleatoria(flotaCpu);

        // Al terminar, pasamos a la fase de batalla y guardamos.
        faseActual = FaseJuego.Batalla;
    }

    void MostrarOpciones()
    {
        // Repetimos el submenu hasta que el usuario quiera volver al menu principal.
        bool volverAlMenuPrincipal = false;

        while (volverAlMenuPrincipal == false)
        {
            int opcion = renderizador.MostrarMenuOpciones();

            if (opcion == 1)
            {
                // Si existe una partida guardada, la eliminamos y avisamos al usuario.
                gestorGuardado.EliminarTodasLasPartidas();
                renderizador.MostrarMensaje("Todas las partidas guardadas han sido eliminadas.");
                renderizador.EsperarVolverAlMenu();
            }
            else if (opcion == 2)
            {
                // Mostramos los nombres de los barcos definidos para el juego.
                List<Barco> barcos = Flota.CrearFlota();
                renderizador.MostrarNombresBarcos(barcos);
            }
            else if (opcion == 3)
            {
                volverAlMenuPrincipal = true;
            }
            else
            {
                renderizador.MostrarError("Opcion no valida.");
                renderizador.EsperarVolverAlMenu();
            }
        }
    }

    public bool Batalla()
    {
        // Si no estamos en la fase correcta, no hacemos nada.
        if (faseActual != FaseJuego.Batalla)
        {
            return false;
        }

        // El bucle termina cuando uno de los dos tableros se queda sin barcos.
        while (jugador.Tablero.TodosHundidos == false && cpu.Tablero.TodosHundidos == false)
        {
            // En cada vuelta mostramos el tablero actual para que siempre se vea el estado real de la partida.
            renderizador.MostrarTablerosBatalla(jugador, cpu.Tablero);

            if (turnoJugador)
            {
                // Si es el turno del jugador, pedimos su disparo.
                bool turnoTerminado = false;

                while (turnoTerminado == false)
                {
                    // Disparamos contra el tablero enemigo.
                    (int fila, int columna) disparoJugador = renderizador.PedirCoordenada();
                    ResultadoDisparo resultadoJugador = cpu.Tablero.Disparar(disparoJugador.fila, disparoJugador.columna);

                    // Si la casilla ya habia sido usada, avisamos y repetimos.
                    if (resultadoJugador == ResultadoDisparo.YaDisparado)
                    {
                        renderizador.MostrarError("Ya habias disparado en esa coordenada.");
                    }
                    else
                    {
                        // Si el disparo es valido, actualizamos estadisticas y cambiamos de turno.
                        jugador.RegistrarDisparo(resultadoJugador);
                        renderizador.MostrarResultadoDisparo(resultadoJugador, disparoJugador.fila, disparoJugador.columna);
                        turnoTerminado = true;
                        turnoJugador = false;
                    }
                }
            }

            // Si despues del disparo del jugador la partida termina, salimos sin turno de CPU.
            if (cpu.Tablero.TodosHundidos)
            {
                break;
            }

            // Cuando ambos han disparado, termina la ronda y entonces preguntamos si se quiere guardar.
            Casilla objetivoCpu = cpu.ElegirObjetivo();
            ResultadoDisparo resultadoCpu = jugador.Tablero.Disparar(objetivoCpu.Fila, objetivoCpu.Columna);
            cpu.RegistrarDisparo(resultadoCpu);
            renderizador.MostrarDisparoCpu(resultadoCpu, objetivoCpu.Fila, objetivoCpu.Columna);
            turnoJugador = true;

            bool quiereSeguirJugando = PreguntarSiQuiereGuardarYSeguir();

            if (quiereSeguirJugando == false)
            {
                renderizador.MostrarMensaje("Saliendo al menu principal...");
                renderizador.EsperarVolverAlMenu();
                faseActual = FaseJuego.Colocacion;
                return false;
            }

            renderizador.EsperarContinuar();
        }

        faseActual = FaseJuego.Terminado;
        return true;
    }

    public void Terminado()
    {
        // Solo mostramos el resultado final si de verdad la partida ha terminado.
        if (faseActual != FaseJuego.Terminado)
        {
            return;
        }

        // Ganamos si la CPU se ha quedado sin barcos.
        bool ganaJugador = cpu.Tablero.TodosHundidos;
        renderizador.MostrarResultadoFinal(ganaJugador, jugador);

        // Si el jugador gana, guardamos su resultado en el ranking.
        if (ganaJugador)
        {
            GuardarEnMarcador();
        }

        // Al terminar la partida, eliminamos el guardado.
        gestorGuardado.EliminarGuardado();
    }

    void ColocarFlotaJugador()
    {
        // Recorremos todos los barcos del jugador para colocarlos uno a uno.
        foreach (Barco barco in flotaJugador)
        {
            bool colocado = false;

            while (colocado == false)
            {
                // Primero pedimos una posicion.
                renderizador.MostrarTableroColocacion(jugador.Tablero, barco);
                (int fila, int columna, bool esHorizontal) posicion = renderizador.PedirPosicion(barco);
                bool posicionValida = jugador.Tablero.PuedeColocar(barco, posicion.fila, posicion.columna, posicion.esHorizontal);

                // Luego mostramos una vista previa con ?.
                renderizador.MostrarTableroColocacion(jugador.Tablero, barco, posicion.fila, posicion.columna, posicion.esHorizontal, true, posicionValida);

                // Si no es valida, avisamos y volvemos a empezar.
                if (posicionValida == false)
                {
                    renderizador.MostrarError("Ese barco no cabe ahi o toca a otro barco.");
                    renderizador.EsperarReintento();
                    continue;
                }

                // Si es valida, permitimos confirmar o recolocar.
                if (renderizador.PedirConfirmacionColocacion())
                {
                    jugador.Tablero.ColocarBarco(barco, posicion.fila, posicion.columna, posicion.esHorizontal);
                    colocado = true;
                }
                else
                {
                    renderizador.MostrarError("Colocacion cancelada. Elige otra posicion.");
                }
            }
        }
    }

    void GuardarPartida()
    {
        // Creamos un estado serializable y lo enviamos al gestor de guardado.
        EstadoPartida estado = CrearEstadoPartida();
        gestorGuardado.Guardar(estado);
    }

    bool PreguntarSiQuiereGuardarYSeguir()
    {
        // Al terminar una ronda completa, preguntamos si quiere guardar y volver al menu.
        bool quiereGuardar = renderizador.PedirGuardarPartida();

        if (quiereGuardar)
        {
            GuardarPartida();
            renderizador.MostrarMensaje("Partida guardada correctamente.");
            return false;
        }
        else
        {
            // Si no quiere guardar, seguimos la partida exactamente igual y sin guardar.
            return true;
        }
    }

    EstadoPartida CrearEstadoPartida()
    {
        // Convertimos el estado actual del juego en un objeto facil de guardar.
        EstadoPartida estado = new EstadoPartida();
        estado.Jugador = CrearJugadorGuardado(jugador);
        estado.Cpu = CrearJugadorGuardado(cpu);
        estado.TableroJugador = CrearTableroGuardado(jugador.Tablero);
        estado.TableroCpu = CrearTableroGuardado(cpu.Tablero);
        estado.TurnoJugador = turnoJugador;
        estado.FaseActual = faseActual.ToString();
        estado.Configuracion = configJuego;
        estado.ObjetivosCpu = cpu.ObtenerObjetivosGuardados();
        return estado;
    }

    JugadorGuardado CrearJugadorGuardado(Jugador jugadorActual)
    {
        // Copiamos solo los datos simples del jugador.
        JugadorGuardado jugadorGuardado = new JugadorGuardado();
        jugadorGuardado.Nombre = jugadorActual.Nombre;
        jugadorGuardado.Disparos = jugadorActual.Disparos;
        jugadorGuardado.Aciertos = jugadorActual.Aciertos;
        jugadorGuardado.Fallos = jugadorActual.Fallos;
        return jugadorGuardado;
    }

    TableroGuardado CrearTableroGuardado(Tablero tablero)
    {
        // Creamos un tablero guardado con barcos y disparos.
        TableroGuardado tableroGuardado = new TableroGuardado();

        // Guardamos la informacion de cada barco.
        foreach (Barco barco in tablero.ObtenerBarcos())
        {
            BarcoGuardado barcoGuardado = new BarcoGuardado();
            barcoGuardado.Nombre = barco.Nombre;
            barcoGuardado.Tamanio = barco.Tamanio;
            barcoGuardado.Impactos = barco.Impactos;

            foreach (Casilla casilla in barco.Casillas)
            {
                barcoGuardado.Casillas.Add(new CoordenadaGuardada(casilla.Fila, casilla.Columna));
            }

            tableroGuardado.Barcos.Add(barcoGuardado);
        }

        // Guardamos tambien las casillas ya disparadas.
        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                Casilla casilla = tablero.ObtenerCasilla(fila, columna);

                if (casilla.Disparada)
                {
                    tableroGuardado.CasillasDisparadas.Add(new CoordenadaGuardada(fila, columna));
                }
            }
        }

        return tableroGuardado;
    }

    bool CargarPartida()
    {
        // Pedimos al gestor el estado guardado.
        EstadoPartida? estado = gestorGuardado.Cargar();

        if (estado == null)
        {
            return false;
        }

        // Reconstruimos ambos tableros a partir del JSON.
        Tablero tableroJugador = Tablero.CrearDesdeEstado(estado.TableroJugador);
        Tablero tableroCpu = Tablero.CrearDesdeEstado(estado.TableroCpu);

        // Reconstruimos jugador, CPU y datos auxiliares.
        jugador = new Jugador(estado.Jugador.Nombre, estado.Jugador.Disparos, estado.Jugador.Aciertos, estado.Jugador.Fallos, 0, tableroJugador);
        cpu = new Cpu(estado.Cpu.Nombre, estado.Cpu.Disparos, estado.Cpu.Aciertos, estado.Cpu.Fallos, 0, tableroCpu);
        cpu.CargarObjetivos(estado.ObjetivosCpu);
        configJuego = estado.Configuracion;

        flotaJugador = tableroJugador.ObtenerBarcos();
        flotaCpu = tableroCpu.ObtenerBarcos();
        turnoJugador = estado.TurnoJugador;
        faseActual = ConvertirFase(estado.FaseActual);
        return true;
    }

    FaseJuego ConvertirFase(string faseTexto)
    {
        // Convertimos el texto guardado en la fase real del juego.
        if (faseTexto == "Colocacion")
        {
            return FaseJuego.Colocacion;
        }
        else if (faseTexto == "Batalla")
        {
            return FaseJuego.Batalla;
        }
        else
        {
            return FaseJuego.Terminado;
        }
    }

    void GuardarEnMarcador()
    {
        // Calculamos la puntuacion final usando la formula del proyecto.
        double puntuacion = jugador.Precision * (100.0 / Math.Max(jugador.Disparos, 1));

        // Creamos la entrada y la guardamos en el ranking.
        EntradaMarcador entrada = new EntradaMarcador(
            jugador.Nombre,
            jugador.Disparos,
            jugador.Aciertos,
            jugador.Fallos,
            jugador.Precision,
            puntuacion,
            DateTime.Now
        );

        marcador.AgregarEntrada(entrada);
    }
}

