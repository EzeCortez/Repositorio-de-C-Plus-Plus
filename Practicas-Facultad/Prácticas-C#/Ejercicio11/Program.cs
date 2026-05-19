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
public class Visitante : Persona
{
    private int dni;
    public void setDni(int nuevoDni)
    {
        dni = nuevoDni;
    }    
    public int getDni()
    {
        return dni;
    }
}
public class Guardia : Persona
{
    
    public override string Presentarse()
    {
    // Usamos los getters porque los atributos originales son privados
    return $"Hola, mi nombre es {getNombre()} {getApellido()} y soy el guardia.";
    }
    public string controlarDocumento(int dni)
    {
        return $"Adelante persona con dni {dni}";
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
        miVisitante.setNombre("Julian");
        miVisitante.setApellido("Peralta");  
        miVisitante.setDni(42659953);

        Guardia miGuardia = new Guardia();
        miGuardia.setNombre("Gordo");
        miGuardia.setApellido("Dan");

        Console.WriteLine($"{miGuardia.controlarDocumento(miVisitante.getDni())} {miGuardia.Presentarse()}");

        
    }
}

