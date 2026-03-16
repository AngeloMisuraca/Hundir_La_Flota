class Barco
{
    string nombre;
    int tamano;
    int impactos;
    List<Casilla> casillas;

    public Barco(string nombre, int tamano, int impactos, List<Casilla> casillas)
    {
        this.nombre = nombre;
        this.tamano = tamano;
        this.impactos = impactos;
        this.casillas = casillas;
    }

    public void RecibirImpacto()
    {

    }

    public bool EstaHundido()
    {
        return false;
    }
}