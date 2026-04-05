class Program
{
    static void Main(string[] args)
    {
        // Si arrancamos el programa con "test", ejecutamos las pruebas.
        if (args.Length > 0 && args[0].ToLower() == "test")
        {
            TestsRunner.Ejecutar();
            return;
        }

        // Si no hay argumentos, iniciamos el juego normal.
        Juego juego = new Juego();
        juego.Iniciar();
    }
}
