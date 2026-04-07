public class Barco
{
    // Nombre del barco.
    public string Nombre { get; }

    // Cuantas casillas ocupa.
    public int Tamanio { get; }

    // Impactos que lleva encima.
    public int Impactos { get; private set; }

    // Casillas donde esta colocado.
    public List<Casilla> Casillas { get; }

    public Barco(string nombre, int tamano, int impactos)
    {
        // Guardo lo basico del barco.
        Nombre = nombre;
        Tamanio = tamano;
        Impactos = impactos;
        Casillas = new List<Casilla>();
    }

    public void RecibirImpacto()
    {
        // Cada impacto suma uno.
        Impactos++;
    }

    public bool EstaHundido()
    {
        // Si los impactos llegan al tamano, esta hundido.
        return Impactos >= Tamanio;
    }
}
