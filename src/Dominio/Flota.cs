class Flota
{
    public List<Barco> barcos;
    public Flota(List<Barco> barcos)
    {
        this.barcos = new List<Barco>();
    }

    public void fabricaBarcos(Barco barco)
    {
        string[] nombres = { "🚢 Portaaviones", "🛳️ Acorazado", "⛴️ Destructor", "🚤 submarino", "⛵ Patrullera" };
        int[] tamaños = { 5, 4, 3, 3, 2 };

        for (int i = 0; i < 5; i++)
        {
            barcos.Add(new Barco(nombres[i], tamaños[i], impactos: 0));
        }
    }
}