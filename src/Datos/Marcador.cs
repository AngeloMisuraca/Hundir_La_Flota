using System.Text.Json;

class Marcador
{
    // Guardamos la lista de partidas ganadas y la ruta del archivo del ranking.
    List<EntradaMarcador> entradas;
    string carpetaDatos;
    string rutaMarcador;

    public Marcador()
    {
        // Al crear el marcador, intentamos cargar las entradas ya guardadas.
        entradas = new List<EntradaMarcador>();
        carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "datos");
        rutaMarcador = Path.Combine(carpetaDatos, "marcador.json");
        Cargar();
    }

    public List<EntradaMarcador> ObtenerEntradas()
    {
        // Devolvemos la lista actual del ranking.
        return entradas;
    }

    public void AgregarEntrada(EntradaMarcador entrada)
    {
        // Añadimos la nueva partida, ordenamos y dejamos solo el top 10.
        entradas.Add(entrada);
        OrdenarEntradas();
        LimitarTop10();
        Guardar();
    }

    public void Guardar()
    {
        // Creamos la carpeta si hace falta.
        if (Directory.Exists(carpetaDatos) == false)
        {
            Directory.CreateDirectory(carpetaDatos);
        }

        // Guardamos el ranking en formato JSON.
        JsonSerializerOptions opciones = new JsonSerializerOptions();
        opciones.WriteIndented = true;

        string json = JsonSerializer.Serialize(entradas, opciones);
        File.WriteAllText(rutaMarcador, json);
    }

    public void Cargar()
    {
        // Limpiamos la lista antes de volver a cargar.
        entradas.Clear();

        // Si el archivo no existe, dejamos el marcador vacio.
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
            // Si el JSON esta roto, empezamos con el marcador vacio.
            entradas = new List<EntradaMarcador>();
        }
    }

    void OrdenarEntradas()
    {
        // El ranking se ordena por puntuacion de mayor a menor.
        entradas = entradas
            .OrderByDescending(entrada => entrada.puntuacion)
            .ThenBy(entrada => entrada.fecha)
            .ToList();
    }

    void LimitarTop10()
    {
        // Si hay mas de 10 resultados, nos quedamos solo con los 10 mejores.
        if (entradas.Count > 10)
        {
            entradas = entradas.Take(10).ToList();
        }
    }
}
