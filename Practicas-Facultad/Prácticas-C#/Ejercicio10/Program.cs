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

    public virtual string Presentarse()
{
    return $"Hola, mi nombre es {getNombre()} {getApellido()} y soy el guardia.";
}
}
public class Guardia : Persona
{
    public override string Presentarse()
{
    // Usamos los getters porque los atributos originales son privados
    return $"Hola, mi nombre es {getNombre()} {getApellido()} y soy el guardia.";
}
}

class Program
{
    static void Main()
    {
        Persona miPersona = new Persona();
        miPersona.setNombre("Armando");
        miPersona.setApellido("Barreda");  

        Guardia miGuardia = new Guardia();
        miGuardia.setNombre("Gordo");
        miGuardia.setApellido("Dan");

        Console.WriteLine(miPersona.Presentarse());
        Console.WriteLine(miGuardia.Presentarse());
        
    }
}

