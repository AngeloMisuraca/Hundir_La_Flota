static class CpuTests
{
    public static void Ejecutar()
    {
        // Ejecutamos todas las pruebas de la CPU.
        ElegirObjetivoNoRepiteCoordenadas();
    }

    static void ElegirObjetivoNoRepiteCoordenadas()
    {
        // La CPU debe poder recorrer las 100 casillas sin repetir ninguna.
        Cpu cpu = new Cpu("CPU", new Tablero(false, 5));
        HashSet<string> coordenadas = new HashSet<string>();

        for (int i = 0; i < 100; i++)
        {
            Casilla objetivo = cpu.ElegirObjetivo();
            string clave = objetivo.Fila + "-" + objetivo.Columna;
            bool agregada = coordenadas.Add(clave);
            TestHelper.DebeSerVerdadero(agregada, "La CPU no debe repetir objetivos.");
        }

        TestHelper.DebeSerIgual(100, coordenadas.Count, "La CPU debe cubrir las 100 casillas sin repetir.");
    }
}
