using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
/* 
-------------------------------------------------------------------------
Ejercicio 29:
Una fábrica automotriz es famosa por sus dos modelos de automóvil, "El deportivo" y "el familiero", 
el primero fabricado con piezas importadas y el segundo, con piezas nacionales. 
Ambos cuentan con aire acondicionado y reproductor mp3. 
El familiero cuenta con portaequipaje y un baúl espacioso, cosa que no ocurre con el otro automóvil. 

El texto exige estas piezas:
-----Aire Acondicionado: Nacional (frío/calor) y Chino (frío).
-----MP3: Nacional (miniplug) y Alemán (miniplug + bluetooth).
-----Vehículos: Deportivo (piezas importadas) y Familiero (piezas nacionales, portaequipaje, baúl).
*/

public interface ICalefaccionable
{
    string aireConCalor();
}

public abstract class aireAcondicionado
{
    public abstract string aireFrio();
}

public class aireNacional : aireAcondicionado, ICalefaccionable
{
    public override string aireFrio()
    {
        return "Aire Frio ON";
    }

    public string aireConCalor()
    {
        return "Aire Caliente ON";
    }
}

public class aireChino : aireAcondicionado
{
    public override string aireFrio()
    {
        return "Aire Frio ON";
    }
}
    
public interface IBluetooth
{
    public string bluetooth();
}

public abstract class mp3
{
    public abstract string miniPlug();
}

public class mp3Aleman : mp3, IBluetooth
{
    public string bluetooth()
    {
        return "Bluetooth encendido";
    }
    public override string miniPlug()
    {
        return "Miniplug Conectado";
    }
}

public class mp3Nacional : mp3
{
    public override string miniPlug()
    {
        return "Miniplug Conectado";
    }
}

public abstract class Automovil
{
    protected aireAcondicionado aire;

    protected mp3 reproductor;
    public abstract string Piezas();

    public abstract string Adicional();
}

public class autoDeportivo : Automovil
{
    public autoDeportivo()
    {
        aire = new aireChino();
        reproductor = new mp3Aleman();
    }
    public override string Piezas()
    {
        return "Este vehículo utiliza piezas importadas.";
    }

    public override string Adicional()
    {
        return "Este vehículo no tiene adicionales.";
    } 

    
}

public class autoFamiliero : Automovil
{
    public autoFamiliero()
    {
        aire = new aireNacional();
        reproductor = new mp3Nacional();
    }
    
    public override string Piezas()
    {
        return "Este vehículo utiliza piezas Nacionales.";
    }

    public override string Adicional()
    {
        return "Este vehículo tiene Portaequipaje y Baúl.";
    } 

}

class Program
{
    static void Main()
    {
        List<Automovil> listaAutomoviles = new List<Automovil>();
        for(int i = 0; i < 10; i++)
        {
            autoDeportivo nuevoAutoDeportivo = new autoDeportivo();
            listaAutomoviles.Add(nuevoAutoDeportivo);
            autoFamiliero nuevoAutoFamiliero = new autoFamiliero();
            listaAutomoviles.Add(nuevoAutoFamiliero);
        }
        foreach(Automovil automovilGuardado in listaAutomoviles)
        {
            Console.WriteLine($"Piezas: {automovilGuardado.Piezas()} | Adicionales: {automovilGuardado.Adicional()}");

        }
    }
}


