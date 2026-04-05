static class BarcoTests
{
    public static void Ejecutar()
    {
        // Ejecutamos todas las pruebas de la clase Barco.
        RecibirImpactoIncrementaImpactos();
        EstaHundidoDevuelveTrueCuandoLlegaAlTamanio();
    }

    static void RecibirImpactoIncrementaImpactos()
    {
        // Comprobamos que cada impacto suma 1.
        Barco barco = new Barco("Destructor", 3, 0);
        barco.RecibirImpacto();
        TestHelper.DebeSerIgual(1, barco.Impactos, "RecibirImpacto debe sumar un impacto.");
    }

    static void EstaHundidoDevuelveTrueCuandoLlegaAlTamanio()
    {
        // Comprobamos que el barco queda hundido al recibir todos sus impactos.
        Barco barco = new Barco("Patrullera", 2, 0);
        barco.RecibirImpacto();
        barco.RecibirImpacto();
        TestHelper.DebeSerVerdadero(barco.EstaHundido(), "El barco debe quedar hundido al recibir todos sus impactos.");
    }
}
