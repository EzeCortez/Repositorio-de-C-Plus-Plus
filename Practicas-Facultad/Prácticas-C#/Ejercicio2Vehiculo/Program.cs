using System;

public class Vehiculo
{
    public string Marca;
    public string Modelo;
    private string Patente;
}

class Program
{
    static void Main()
    {
        Vehiculo vehiculo1 = new Vehiculo();
        vehiculo1.Marca1 = "Toyota";
        vehiculo1.Modelo1 = "Yaris";
        vehiculo1.Patente1 = "123";

        Vehiculo vehiculo2 = new Vehiculo();
        vehiculo2.Marca2 = "Fiat";
        vehiculo2.Modelo2 = "Palio";
        vehiculo2.Patente2 = "321";

        Console.WriteLine("Primer auto: " + vehiculo1.Marca1 + " " + vehiculo1.Modelo1 + " " + vehiculo1.Patente1);
        Console.WriteLine("Primer auto: " + vehiculo2.Marca2 + " " + vehiculo2.Modelo2 + " " + vehiculo2.Patente2);
    }
}