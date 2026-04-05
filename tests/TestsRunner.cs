static class TestsRunner
{
    public static void Ejecutar()
    {
        // Lanzamos todos los bloques de pruebas del proyecto.
        BarcoTests.Ejecutar();
        CpuTests.Ejecutar();
        TableroTests.Ejecutar();
        Console.WriteLine("Tests correctos.");
    }
}
