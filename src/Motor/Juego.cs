class Juego
{
    // Estas son las fases por las que pasa la partida.
    enum FaseJuego
    {
        Colocacion,
        Batalla,
        Terminado
    }

    // Aqui tengo lo importante de la partida.
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
        // Arranco todo lo basico del juego.
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
        // Esto se queda en el menu hasta que se elija salir.
        bool salirDelPrograma = false;

        while (salirDelPrograma == false)
        {
            int opcion = 0;

            // Sigo mostrando el menu hasta que la opcion sea valida.
            while (opcion != 1 && opcion != 2 && opcion != 5)
            {
                opcion = renderizador.MostrarBienvenida(gestorGuardado.ExistePartidaGuardada());

                // Si toca records, los enseño.
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

            // Si se elige continuar, intento recuperar la partida.
            if (opcion == 2)
            {
                bool partidaCargada = CargarPartida();

                if (partidaCargada == false)
                {
                    renderizador.MostrarError("No hay ninguna partida guardada.");
                    renderizador.EsperarVolverAlMenu();
                    continue;
                }

                // Aviso de que la partida se ha cargado bien.
                renderizador.MostrarMensaje("Cargando partida guardada...");
            }
            else
            {
                // Si no, arranco una partida nueva.
                Colocacion();
            }

            // Cuando ya esta todo listo, empieza la partida.
            bool partidaTerminada = Batalla();

            // Solo enseño el final si la partida ha acabado de verdad.
            if (partidaTerminada)
            {
                Terminado();
            }
        }
    }

    void ReiniciarEstadoPartida()
    {
        // Dejo todo limpio para empezar otra vez.
        jugador = new Jugador(configJuego.NombreJugador, new Tablero(false, 5));
        cpu = new Cpu("CPU", new Tablero(false, 5));
        faseActual = FaseJuego.Colocacion;
        turnoJugador = true;
        flotaJugador = new List<Barco>();
        flotaCpu = new List<Barco>();
    }

    public void Colocacion()
    {
        // Antes de colocar nada, reseteo la partida.
        ReiniciarEstadoPartida();

        // Creo una flota para cada lado.
        flotaJugador = Flota.CrearFlota();
        flotaCpu = Flota.CrearFlota();

        // El jugador coloca la suya y la CPU pone la suya sola.
        ColocarFlotaJugador();
        cpu.ColocarFlotaAleatoria(flotaCpu);

        // Cuando termina esto, ya pasamos a batalla.
        faseActual = FaseJuego.Batalla;
    }

    void MostrarOpciones()
    {
        // Este menu sigue hasta que se vuelva al principal.
        bool volverAlMenuPrincipal = false;

        while (volverAlMenuPrincipal == false)
        {
            int opcion = renderizador.MostrarMenuOpciones();

            if (opcion == 1)
            {
                // Si habia partida guardada, la borro y aviso.
                gestorGuardado.EliminarTodasLasPartidas();
                renderizador.MostrarMensaje("Todas las partidas guardadas han sido eliminadas.");
                renderizador.EsperarVolverAlMenu();
            }
            else if (opcion == 2)
            {
                // Aqui enseño los barcos que usa el juego.
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
        // Si no toca batalla todavia, salgo.
        if (faseActual != FaseJuego.Batalla)
        {
            return false;
        }

        // Esto sigue hasta que alguien se queda sin barcos.
        while (jugador.Tablero.TodosHundidos == false && cpu.Tablero.TodosHundidos == false)
        {
            // En cada turno vuelvo a pintar el tablero actualizado.
            renderizador.MostrarTablerosBatalla(jugador, cpu.Tablero);

            if (turnoJugador)
            {
                // Si es tu turno, pido el disparo.
                bool turnoTerminado = false;

                while (turnoTerminado == false)
                {
                    // Disparo al tablero enemigo.
                    (int fila, int columna) disparoJugador = renderizador.PedirCoordenada();
                    ResultadoDisparo resultadoJugador = cpu.Tablero.Disparar(disparoJugador.fila, disparoJugador.columna);

                    // Si esa casilla ya estaba usada, aviso y vuelvo a pedir.
                    if (resultadoJugador == ResultadoDisparo.YaDisparado)
                    {
                        renderizador.MostrarError("Ya habias disparado en esa coordenada.");
                    }
                    else
                    {
                        // Si el disparo vale, actualizo datos y paso turno.
                        jugador.RegistrarDisparo(resultadoJugador);
                        renderizador.MostrarResultadoDisparo(resultadoJugador, disparoJugador.fila, disparoJugador.columna);
                        turnoTerminado = true;
                        turnoJugador = false;
                    }
                }
            }

            // Si aqui ya termino la partida, la CPU no juega.
            if (cpu.Tablero.TodosHundidos)
            {
                break;
            }

            // Despues del turno de la CPU pregunto si se quiere guardar.
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
        // Si la partida no ha terminado, aqui no hago nada.
        if (faseActual != FaseJuego.Terminado)
        {
            return;
        }

        // Si la CPU se queda sin barcos, gana el jugador.
        bool ganaJugador = cpu.Tablero.TodosHundidos;
        renderizador.MostrarResultadoFinal(ganaJugador, jugador);

        // Si ganas, guardo tu partida en el ranking.
        if (ganaJugador)
        {
            GuardarEnMarcador();
        }

        // Cuando acaba, borro la partida guardada.
        gestorGuardado.EliminarGuardado();
    }

    void ColocarFlotaJugador()
    {
        // Voy barco por barco para colocarlos.
        foreach (Barco barco in flotaJugador)
        {
            bool colocado = false;

            while (colocado == false)
            {
                // Primero pido la posicion.
                renderizador.MostrarTableroColocacion(jugador.Tablero, barco);
                (int fila, int columna, bool esHorizontal) posicion = renderizador.PedirPosicion(barco);
                bool posicionValida = jugador.Tablero.PuedeColocar(barco, posicion.fila, posicion.columna, posicion.esHorizontal);

                // Luego enseño la vista previa con ?.
                renderizador.MostrarTableroColocacion(jugador.Tablero, barco, posicion.fila, posicion.columna, posicion.esHorizontal, true, posicionValida);

                // Si no vale, aviso y vuelvo a pedir.
                if (posicionValida == false)
                {
                    renderizador.MostrarError("Ese barco no cabe ahi o toca a otro barco.");
                    renderizador.EsperarReintento();
                    continue;
                }

                // Si vale, dejo confirmar o cambiarla.
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
        // Monto el estado y lo mando a guardar.
        EstadoPartida estado = CrearEstadoPartida();
        gestorGuardado.Guardar(estado);
    }

    bool PreguntarSiQuiereGuardarYSeguir()
    {
        // Al final de la ronda pregunto si se quiere guardar.
        bool quiereGuardar = renderizador.PedirGuardarPartida();

        if (quiereGuardar)
        {
            GuardarPartida();
            renderizador.MostrarMensaje("Partida guardada correctamente.");
            return false;
        }
        else
        {
            // Si no quiere guardar, sigo la partida normal.
            return true;
        }
    }

    EstadoPartida CrearEstadoPartida()
    {
        // Paso la partida actual a un objeto que se pueda guardar.
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
        // Aqui guardo solo lo basico del jugador.
        JugadorGuardado jugadorGuardado = new JugadorGuardado();
        jugadorGuardado.Nombre = jugadorActual.Nombre;
        jugadorGuardado.Disparos = jugadorActual.Disparos;
        jugadorGuardado.Aciertos = jugadorActual.Aciertos;
        jugadorGuardado.Fallos = jugadorActual.Fallos;
        return jugadorGuardado;
    }

    TableroGuardado CrearTableroGuardado(Tablero tablero)
    {
        // Preparo el tablero para guardarlo en el JSON.
        TableroGuardado tableroGuardado = new TableroGuardado();

        // Guardo los datos de cada barco.
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

        // Tambien guardo las casillas donde ya se disparo.
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
        // Intento leer la partida guardada.
        EstadoPartida? estado = gestorGuardado.Cargar();

        if (estado == null)
        {
            return false;
        }

        // Vuelvo a montar los dos tableros desde el JSON.
        Tablero tableroJugador = Tablero.CrearDesdeEstado(estado.TableroJugador);
        Tablero tableroCpu = Tablero.CrearDesdeEstado(estado.TableroCpu);

        // Aqui reconstruyo jugador, CPU y lo demas.
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
        // Paso el texto guardado a la fase real.
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
        // Saco la puntuacion final.
        double puntuacion = jugador.Precision * (100.0 / Math.Max(jugador.Disparos, 1));

        // Creo la entrada y la meto en el ranking.
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

