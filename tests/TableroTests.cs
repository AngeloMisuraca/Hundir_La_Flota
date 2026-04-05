static class TableroTests
{
    public static void Ejecutar()
    {
        // Ejecutamos las pruebas principales del tablero.
        PuedeColocarImpideAdyacenciaDiagonal();
        DispararDevuelveAguaImpactoHundidoYYaDisparado();
    }

    static void PuedeColocarImpideAdyacenciaDiagonal()
    {
        // Verificamos la regla que impide que dos barcos se toquen en diagonal.
        Tablero tablero = new Tablero(false, 5);
        Barco barco1 = new Barco("Patrullera", 2, 0);
        Barco barco2 = new Barco("Submarino", 3, 0);

        tablero.ColocarBarco(barco1, 0, 0, true);
        bool puedeColocar = tablero.PuedeColocar(barco2, 1, 2, false);

        TestHelper.DebeSerVerdadero(puedeColocar == false, "No se debe poder colocar un barco tocando en diagonal.");
    }

    static void DispararDevuelveAguaImpactoHundidoYYaDisparado()
    {
        // Verificamos los cuatro resultados posibles al disparar.
        Tablero tablero = new Tablero(false, 1);
        Barco barco = new Barco("Patrullera", 2, 0);
        tablero.ColocarBarco(barco, 0, 0, true);

        ResultadoDisparo agua = tablero.Disparar(5, 5);
        ResultadoDisparo impacto = tablero.Disparar(0, 0);
        ResultadoDisparo hundido = tablero.Disparar(0, 1);
        ResultadoDisparo yaDisparado = tablero.Disparar(0, 1);

        TestHelper.DebeSerIgual(ResultadoDisparo.Agua, agua, "El disparo en vacio debe devolver Agua.");
        TestHelper.DebeSerIgual(ResultadoDisparo.Impacto, impacto, "El primer impacto debe devolver Impacto.");
        TestHelper.DebeSerIgual(ResultadoDisparo.Hundido, hundido, "El ultimo impacto debe devolver Hundido.");
        TestHelper.DebeSerIgual(ResultadoDisparo.YaDisparado, yaDisparado, "Repetir disparo debe devolver YaDisparado.");
    }
}
