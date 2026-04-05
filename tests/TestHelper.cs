static class TestHelper
{
    // Esta ayuda comprueba una condicion booleana.
    public static void DebeSerVerdadero(bool condicion, string mensaje)
    {
        if (condicion == false)
        {
            throw new Exception(mensaje);
        }
    }

    // Esta ayuda compara dos valores y falla si no son iguales.
    public static void DebeSerIgual<T>(T esperado, T actual, string mensaje)
    {
        if (Equals(esperado, actual) == false)
        {
            throw new Exception(mensaje + " Esperado: " + esperado + " Actual: " + actual);
        }
    }
}
