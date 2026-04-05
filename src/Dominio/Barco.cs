public class Barco
{
    // Nombre del barco, por ejemplo "Destructor".
    public string Nombre { get; }

    // Numero de casillas que ocupa el barco.
    public int Tamanio { get; }

    // Impactos recibidos hasta ahora.
    public int Impactos { get; private set; }

    // Casillas del tablero que ocupa este barco.
    public List<Casilla> Casillas { get; }

    public Barco(string nombre, int tamano, int impactos)
    {
        // Guardamos los datos basicos del barco.
        Nombre = nombre;
        Tamanio = tamano;
        Impactos = impactos;
        Casillas = new List<Casilla>();
    }

    public void RecibirImpacto()
    {
        // Cada disparo acertado suma un impacto.
        Impactos++;
    }

    public bool EstaHundido()
    {
        // Un barco esta hundido cuando recibe tantos impactos como casillas ocupa.
        return Impactos >= Tamanio;
    }
}
