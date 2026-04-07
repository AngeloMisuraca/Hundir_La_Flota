class ArteAscii
{
    // Logo principal.
    public static string logo =
@" _   _ _   _ _   _ ____  ___ ____    _        _    _____ _     ___ _____  _    
| | | | | | | \ | |  _ \|_ _|  _ \  | |      / \  |  ___| |   / _ \_   _|/ \   
| |_| | | | |  \| | | | || || |_) | | |     / _ \ | |_  | |  | | | || | / _ \  
|  _  | |_| | |\  | |_| || ||  _ <  | |___ / ___ \|  _| | |__| |_| || |/ ___ \ 
|_| |_|\___/|_| \_|____/|___|_| \_\ |_____/_/   \_\_|   |_____\___/ |_/_/   \_\";

    // Esto sale al ganar.
    public static string victoria =
@"__   ___ ___ _____ ___  ___ ___    _    
\ \ / /_ _/ __|_   _/ _ \| _ \_ _|  / \   
 \ V / | | (__  | || (_) |   /| |  / _ \  
  \_/ |___\___| |_| \___/|_|_\___|/_/ \_\ ";

    // Esto sale al perder.
    public static string derrota =
@" ___  ___ ___ ___  ___ _____ _    
|   \| __| _ \ _ \/ _ \_   _/ \   
| |) | _||   /   / (_) || |/ _ \  
|___/|___|_|_\_|_\\___/ |_/_/ \_\ ";

    public void ArteTexto()
    {
        // Pinto el logo con el color del titulo.
        Console.ForegroundColor = Colores.titulo;
        Console.WriteLine(logo);
        Console.ResetColor();
    }
}
