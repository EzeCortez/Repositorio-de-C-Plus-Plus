using System;

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
    public abstract int itemsSoportados();
    public abstract int elementosMax();
    
}
public abstract class Item
{
    public abstract string Descripción();
    public abstract Double Dimensiones();
    public abstract int ID();
    public abstract string Adicional();
    public abstract int Cantidad();

}

