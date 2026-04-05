using System.Text.Json;

class ConfigJuego
{
    // Estas propiedades guardan las opciones basicas del jugador.
    public bool MostrarColores { get; set; }
    public string NombreJugador { get; set; }

    public ConfigJuego()
    {
        // Valores por defecto.
        MostrarColores = true;
        NombreJugador = "Jugador";
    }

    public static ConfigJuego Cargar()
    {
        // Calculamos la carpeta y el archivo donde se guarda la configuracion.
        string carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "datos");
        string rutaConfig = Path.Combine(carpetaDatos, "config.json");

        // Si no existe el archivo, creamos uno nuevo con la configuracion por defecto.
        if (File.Exists(rutaConfig) == false)
        {
            ConfigJuego configPorDefecto = new ConfigJuego();
            configPorDefecto.Guardar();
            return configPorDefecto;
        }

        try
        {
            string json = File.ReadAllText(rutaConfig);
            ConfigJuego? config = JsonSerializer.Deserialize<ConfigJuego>(json);

            if (config == null)
            {
                return new ConfigJuego();
            }

            return config;
        }
        catch
        {
            // Si el archivo esta roto o no se puede leer, usamos la configuracion por defecto.
            return new ConfigJuego();
        }
    }

    public void Guardar()
    {
        // Volvemos a calcular la ruta del archivo de configuracion.
        string carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "datos");
        string rutaConfig = Path.Combine(carpetaDatos, "config.json");

        // Creamos la carpeta si todavia no existe.
        if (Directory.Exists(carpetaDatos) == false)
        {
            Directory.CreateDirectory(carpetaDatos);
        }

        // Guardamos la configuracion en JSON con formato legible.
        JsonSerializerOptions opciones = new JsonSerializerOptions();
        opciones.WriteIndented = true;

        string json = JsonSerializer.Serialize(this, opciones);
        File.WriteAllText(rutaConfig, json);
    }
}
