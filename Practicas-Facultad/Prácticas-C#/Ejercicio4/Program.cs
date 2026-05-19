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
        Cine nuevoCine = new Cine();
        nuevoCine.SetPelicula("Avatar");
        nuevoCine.SetHorario("13.30");
        Console.WriteLine(nuevoCine.ObtenerCartelera());
    }
}
