class Flota
{
    public static List<Barco> CrearFlota()
    {
        // Aqui monto la flota completa.
        List<Barco> barcos = new List<Barco>();

        // Creo un barco de cada tipo.
        barcos.Add(new Barco("Portaaviones", 5, 0));
        barcos.Add(new Barco("Acorazado", 4, 0));
        barcos.Add(new Barco("Destructor", 3, 0));
        barcos.Add(new Barco("Submarino", 3, 0));
        barcos.Add(new Barco("Patrullera", 2, 0));

        return barcos;
    }
}
