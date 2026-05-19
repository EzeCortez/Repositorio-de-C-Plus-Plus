using System;
/*
El laboratorio Kokumo Technologies está desarrollando el prototipo de un robot explorador cuyo sistema de tracción 
puede ser personalizado para que se adapte mejor al terreno. El robot, llamado KT-2020, tiene las siguientes 
características:

Número de serie: KT-2020-P
Potencia de tracción base (PTB): 10 hp
Tracción: cualquiera de las dos opciones desarrolladas.

Los sistemas de tracción disponibles son:
Rueda de caucho: ideal para entornos urbanos, su uso le resta 1 hp al PTB y permite el rodado de hasta 100 km; 
cuando se gasta, debe reemplazarse.
Oruga: para todo tipo de terreno, le permite avanzar hasta 400 km antes de requerir reemplazo y resta 3 hp al PTB. 
Incorpora sensores Meke-M0 que le permiten conocer la temperatura.

Analizar, diseñar, diagramar las relaciones e implementar el código. 

Crear instancias y asignarle al robot los distintos sistemas de tracción, mostrando por pantalla: 
Número de serie, potencia final, tipo de tracción, cuánto puede avanzar y datos adicionales
*/
public abstract class Traccion
{
    public abstract int Hp();
    public abstract int DistanciaMaxima();
    public abstract string datosAdicionales();
}

public class RuedaCaucho : Traccion
{
    
    public override int Hp()
    {
        return 1; // La rueda de caucho resta 1 HP
    }

    
    public override int DistanciaMaxima()
    {
        return 100; // La rueda de caucho avanza hasta 100 km
    }

    public override string datosAdicionales()
    {
        return ("Nada");
    }
}

public class RuedaOruga : Traccion
{
    public override int Hp()
    {
        return 3; 
    }
    public override int DistanciaMaxima()
    {
        return 400; 
    }
    public override string datosAdicionales()
    {
        return ("Sensores de temperatura Meke-M0");
    }
}

public class Robot
{
    private string numeroSerie = "KT-2020-P";
    private int potenciaBase = 10;
    private Traccion piezaEquipada;
    
    public void equiparPieza(Traccion nuevaPieza)
    {
        piezaEquipada = nuevaPieza;
        Console.WriteLine($"Pieza {piezaEquipada} equipada con éxito."); 
    }

    public void mostrarInfo()
    {
        Console.WriteLine($"El robot actual es el {numeroSerie} ,la Potencia actual es de {potenciaBase - piezaEquipada.Hp()} y la distancia recorrida sería de {piezaEquipada.DistanciaMaxima()}, addons adicionales: {piezaEquipada.datosAdicionales()}.");
    }
}

class Program
{
    static void Main()
    {
        Robot miRobot = new Robot();
        RuedaCaucho miRuedaCaucho = new RuedaCaucho();
        RuedaOruga miRuedaOruga = new RuedaOruga();
        

        miRobot.equiparPieza(miRuedaCaucho);
        miRobot.mostrarInfo();
        miRobot.equiparPieza(miRuedaOruga);
        miRobot.mostrarInfo();

    }
}