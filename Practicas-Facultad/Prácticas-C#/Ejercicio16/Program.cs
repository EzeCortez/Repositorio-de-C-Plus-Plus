using System;
using System.Collections.Generic;
/*
Una empresa de logística realiza envíos con dos tipos de vehículos:
* Camioneta: capacidad para llevar cómodas, heladeras y lavarropas. Capacidad máxima: 10 elementos.
* Auto: capacidad para llevar televisores, bicicletas y cajas. Capacidad máxima: 5 elementos.

Todos los elementos tienen descripción, dimensiones y un ID. Además, cada uno tiene datos propios 
(ej: las heladeras si tienen freezer, los televisores si son LED, etc.).

Los vehículos deben tener los métodos:
1. Cargar(objeto): para ir sumando carga.
2. ListarItems(): para mostrar por pantalla todo lo que llevan adentro.
*/

public abstract class Vehiculo
{
    public abstract string itemsSoportados();
    public abstract int elementosMax();

    private List<Item> cargaActual;
    public Vehiculo()
    {
        cargaActual = new List<Item>();
    }
    public void Cargar(Item unItem)
    {
    if (cargaActual.Count < elementosMax()) 
    {
        cargaActual.Add(unItem); 
        Console.WriteLine("Ítem cargado con éxito.");
    }
    else 
    {
        Console.WriteLine("¡Error! El vehículo está lleno.");
    }
    }   
    public void ListarItems()
    {
        if (cargaActual.Count <= 0)
        {
            Console.WriteLine("No hay nada cargado");
        }
        else
        {
            Console.WriteLine($"Los items cargados son:");
            Dictionary<string, int> contador = new Dictionary<string, int>();
            foreach (Item itemGuardado in cargaActual)
            {
                string nombreItem = itemGuardado.Descripción();                 
                if (contador.ContainsKey(nombreItem)){
                    contador[nombreItem] = contador[nombreItem] + 1;
                }
                else 
                {      
                    contador[nombreItem] = 1;
                }
            }
            foreach (var renglon in contador)
            {
                // Imprimimos la clave (el nombre) y el valor (la cantidad)
                Console.WriteLine($"- {renglon.Key}: {renglon.Value} unidades");
            }

        }
    }   
    
}
public abstract class Item
{
    public abstract string Descripción();
    public abstract Double Dimensiones();
    public abstract int ID();
    public abstract string Adicional();
}

public class Auto : Vehiculo
{
    public override int elementosMax()
    {
        return 5; 
    }

    public override string itemsSoportados()
    {
        return ("Televisores, bicicletas plegables y cajas pequeñas.");
    }
}

public class Camioneta : Vehiculo
{
    public override int elementosMax()
    {
        return 10; 
    }

    public override string itemsSoportados()
    {
        return ("Cómodas, heladeras y lavarropas.");
    }
}

public class Heladera : Item
{
    public override string Descripción()
    {
        return ("Heladera");
    }

    public override Double Dimensiones()
    {
        return 5.0;
    }
    public override int ID()
    {
        return 1;
    }
    public override string Adicional()
    {
        return ("Voltaje: 220v - Tiene Freezer: Sí");
    }
}

public class Televisor : Item
{
    public override string Descripción()
    {
        return ("Televisor");
    }

    public override Double Dimensiones()
    {
        return 2.0;
    }
    public override int ID()
    {
        return 2;
    }
    public override string Adicional()
    {
        return ("55' 4K Samsung OLED");
    }
}


class Program
{
    static void Main()
    {
        Camioneta miCamioneta = new Camioneta();
        Heladera miHeladera = new Heladera();
        Auto miAuto = new Auto();
        Televisor miTv = new Televisor();

        miCamioneta.Cargar(miHeladera);
        miCamioneta.Cargar(miHeladera);
        miCamioneta.Cargar(miHeladera);
        miCamioneta.ListarItems();

        miAuto.Cargar(miTv);
        miAuto.Cargar(miTv);
        miAuto.Cargar(miTv);
        miAuto.ListarItems();



    }
}