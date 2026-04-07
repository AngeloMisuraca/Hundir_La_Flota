using System.Text.Json;

class Marcador
{
    // Aqui guardo el ranking y su ruta.
    List<EntradaMarcador> entradas;
    string carpetaDatos;
    string rutaMarcador;

    public Marcador()
    {
        // Nada mas crearlo, intento cargar lo que ya habia.
        entradas = new List<EntradaMarcador>();
        carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "datos");
        rutaMarcador = Path.Combine(carpetaDatos, "marcador.json");
        Cargar();
    }

    public List<EntradaMarcador> ObtenerEntradas()
    {
        // Devuelvo el ranking actual.
        return entradas;
    }

    public void AgregarEntrada(EntradaMarcador entrada)
    {
        // Meto la partida, ordeno y dejo solo las 10 mejores.
        entradas.Add(entrada);
        OrdenarEntradas();
        LimitarTop10();
        Guardar();
    }

    public void Guardar()
    {
        // Si hace falta, creo la carpeta.
        if (Directory.Exists(carpetaDatos) == false)
        {
            Directory.CreateDirectory(carpetaDatos);
        }

        // Guardo el ranking en JSON.
        JsonSerializerOptions opciones = new JsonSerializerOptions();
        opciones.WriteIndented = true;

        string json = JsonSerializer.Serialize(entradas, opciones);
        File.WriteAllText(rutaMarcador, json);
    }

    public void Cargar()
    {
        // Limpio antes de volver a cargar.
        entradas.Clear();

        // Si el archivo no existe, se queda vacio.
        if (File.Exists(rutaMarcador) == false)
        {
            return;
        }

        try
        {
            string json = File.ReadAllText(rutaMarcador);
            List<EntradaMarcador>? entradasCargadas = JsonSerializer.Deserialize<List<EntradaMarcador>>(json);

            if (entradasCargadas != null)
            {
                entradas = entradasCargadas;
                OrdenarEntradas();
                LimitarTop10();
            }
        }
        catch
        {
            // Si el JSON falla, empiezo con el ranking vacio.
            entradas = new List<EntradaMarcador>();
        }
    }

    void OrdenarEntradas()
    {
        // Ordeno por puntuacion de mayor a menor.
        entradas = entradas
            .OrderByDescending(entrada => entrada.puntuacion)
            .ThenBy(entrada => entrada.fecha)
            .ToList();
    }

    void LimitarTop10()
    {
        // Si hay mas de 10, me quedo con los 10 mejores.
        if (entradas.Count > 10)
        {
            entradas = entradas.Take(10).ToList();
        }
    }
}
