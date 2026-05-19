using System;

public class Persona
{
    private string nombre, apellido;


    public void setNombre(string nuevoNombre)
    {
        nombre = nuevoNombre;
    }
    public void setApellido(string nuevoApeliido)
    {
        apellido = nuevoApeliido;
    }


    public string getNombre()
    {
        return nombre;
    }

    public string getApellido()
    {
        
        return apellido;
    }
}
public class Visitante
{
    private string nombre, apellido;


    public void setNombre(string nuevoNombre)
    {
        nombre = nuevoNombre;
    }
    public void setApellido(string nuevoApeliido)
    {
        apellido = nuevoApeliido;
    }


    public string getNombre()
    {
        return nombre;
    }

    public string getApellido()
    {
        
        return apellido;
    }
}
public class Guardia
{
    private string nombre, apellido;


    public void setNombre(string nuevoNombre)
    {
        nombre = nuevoNombre;
    }
    public void setApellido(string nuevoApeliido)
    {
        apellido = nuevoApeliido;
    }


    public string getNombre()
    {
        return nombre;
    }

    public string getApellido()
    {
        
        return apellido;
    }
}

class Program
{
    static void Main()
    {
        Persona miPersona = new Persona();
        miPersona.setNombre("Armando");
        miPersona.setApellido("Barreda");

        Visitante miVisitante = new Visitante();
        miVisitante.setNombre("Kakaroto");
        miVisitante.setApellido("Goku");


        Guardia miGuardia = new Guardia();
        miGuardia.setNombre("Gordo");
        miGuardia.setApellido("Dan");


        Console.WriteLine($"Actualmente en el museo se encuentran: {miGuardia.getNombre()} {miGuardia.getApellido()} , {miPersona.getNombre()} {miPersona.getApellido()} y {miVisitante.getNombre()} {miVisitante.getApellido()}");
        }
}