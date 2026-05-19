using System;


public class Persona
{
    public string nombre;
    public string apellido;
}

class Program
{
    static void Main()
    {
        Persona persona1 = new Persona();
        persona1.nombre = "DJ";
        persona1.apellido = "SypaT";

        Persona persona2 = new Persona();
        persona2.nombre = "Rodrats";
        persona2.apellido = "123";



        Console.WriteLine("Primera persona: " + persona1.nombre + " " + persona1.apellido + " ");
        Console.WriteLine("Segunda Persona: " + persona2.nombre + " " + persona2.apellido + " ");

    }
}