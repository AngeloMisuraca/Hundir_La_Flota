using System.Text.Json;

class GestorGuardado
{
    // Aqui guardo las rutas del archivo de partida.
    string carpetaDatos;
    string rutaGuardado;

    public GestorGuardado()
    {
        carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "datos");
        rutaGuardado = Path.Combine(carpetaDatos, "partida.json");
    }

    public void Guardar(EstadoPartida estado)
    {
        // Si la carpeta no existe, la creo.
        if (Directory.Exists(carpetaDatos) == false)
        {
            Directory.CreateDirectory(carpetaDatos);
        }

        // Paso el estado a JSON y lo guardo.
        JsonSerializerOptions opciones = new JsonSerializerOptions();
        opciones.WriteIndented = true;

        string json = JsonSerializer.Serialize(estado, opciones);
        File.WriteAllText(rutaGuardado, json);
    }

    public EstadoPartida? Cargar()
    {
        // Si no hay archivo, devuelvo null.
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
            // Si el archivo falla, devuelvo null y ya esta.
            return null;
        }
    }

    public void EliminarGuardado()
    {
        // Borro el archivo si existe.
        if (File.Exists(rutaGuardado))
        {
            File.Delete(rutaGuardado);
        }
    }

    public void EliminarTodasLasPartidas()
    {
        // Aqui solo hay una partida guardada, asi que borrar todo es borrar ese archivo.
        EliminarGuardado();
    }

    public bool ExistePartidaGuardada()
    {
        // Devuelve true si el guardado existe.
        return File.Exists(rutaGuardado);
    }
}
