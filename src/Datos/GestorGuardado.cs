using System.Text.Json;

class GestorGuardado
{
    // Estas rutas apuntan al archivo donde se guarda la partida.
    string carpetaDatos;
    string rutaGuardado;

    public GestorGuardado()
    {
        carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "datos");
        rutaGuardado = Path.Combine(carpetaDatos, "partida.json");
    }

    public void Guardar(EstadoPartida estado)
    {
        // Creamos la carpeta si todavia no existe.
        if (Directory.Exists(carpetaDatos) == false)
        {
            Directory.CreateDirectory(carpetaDatos);
        }

        // Convertimos el estado a JSON y lo escribimos en disco.
        JsonSerializerOptions opciones = new JsonSerializerOptions();
        opciones.WriteIndented = true;

        string json = JsonSerializer.Serialize(estado, opciones);
        File.WriteAllText(rutaGuardado, json);
    }

    public EstadoPartida? Cargar()
    {
        // Si no hay partida guardada, devolvemos null.
        if (ExistePartidaGuardada() == false)
        {
            return null;
        }

        try
        {
            string json = File.ReadAllText(rutaGuardado);
            EstadoPartida? estado = JsonSerializer.Deserialize<EstadoPartida>(json);
            return estado;
        }
        catch
        {
            // Si el archivo falla o esta corrupto, devolvemos null para evitar que el juego se rompa.
            return null;
        }
    }

    public void EliminarGuardado()
    {
        // Borramos el archivo solo si existe.
        if (File.Exists(rutaGuardado))
        {
            File.Delete(rutaGuardado);
        }
    }

    public void EliminarTodasLasPartidas()
    {
        // En este proyecto solo usamos un archivo de partida, asi que eliminar todas
        // las partidas significa borrar ese archivo si existe.
        EliminarGuardado();
    }

    public bool ExistePartidaGuardada()
    {
        // Devuelve true si el archivo de guardado esta en disco.
        return File.Exists(rutaGuardado);
    }
}
