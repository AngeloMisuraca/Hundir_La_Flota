public class Barco
{
    public string nombre { get; set; }
    public int tamanio { get; set; }
    public int impactos { get; set; }
    public List<Casilla> Casillas = new List<Casilla>();
    public Barco(string nombre, int tamano, int impactos)
    {
        this.nombre = nombre;
        this.tamanio = tamano;
        this.impactos = impactos;
    }

    public static void RecibirImpacto()
    {
        int impacto = 0;
        impacto++;
    }

    public bool EstaHundido()
    {
        if (impactos >= tamanio)
        {
            bool EstaHundido = true;
            return true;
        }
        else
        {
            bool EstaHundido = false;
            return false;
        }
    }
}