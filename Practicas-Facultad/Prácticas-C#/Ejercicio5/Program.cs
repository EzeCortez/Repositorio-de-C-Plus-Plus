using System;

public class Cine
{
    private string? Pelicula, Horario;

    public void SetPelicula(string nuevaPelicula)
    {
        Pelicula = nuevaPelicula;
    }

    public void SetHorario(string nuevoHorario)
    {
        Horario = nuevoHorario;
    }

    public string GetPelicula()
    {
        return Pelicula;
    }
    public string GetHorario()
    {
        return Horario;
    }

    public string ObtenerCartelera()
    {
        return $"{Pelicula} - Horario: {Horario}";
    }
}

class Program
{
    static void Main()
    {
        Cine Cine1 = new Cine();
        Cine1.SetPelicula("Avatar");
        Cine1.SetHorario("19.30");

        Cine Cine2 = new Cine();
        Cine2.SetPelicula("Transformer 3");
        Cine2.SetHorario("20.00");

        Console.WriteLine(Cine1.ObtenerCartelera());
        Console.WriteLine(Cine2.ObtenerCartelera());

        Cine1.SetPelicula("El señor de los Anillos");

        Console.WriteLine(Cine1.ObtenerCartelera());

    }
}