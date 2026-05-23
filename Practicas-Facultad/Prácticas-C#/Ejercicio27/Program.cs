using System;
using System.Collections.Generic;
/* 
-------------------------------------------------------------------------
Ejercicio 27: Crear un singleton con un atributo del tipo:
-----1. Entero 
-----2. Cadena de caracteres 
-----3. Colección de objetos (cualquiera) 

Desde el main, asignarle valores a los atributos.
Una vez cargados, recuperarlos y mostrar en pantalla los valores.

-------------------------------------------------------------------------
Ejercicio 28: Crear un singleton [...]:
-----1. Desde el main, invocar al singleton y asignarle valor al atributo. 
-----2. Crear una instancia de SingletonCaller para ejecutar la función calling. 
-----3. Concluir que el singleton siempre opera sobre la misma instancia
-------------------------------------------------------------------------
*/

public class Singleton
{
    // 1. La "caja vacía" (Variable)
    private static Singleton instanciaUnica; 
    
    // 2. Los atributos que guardarán los datos
    private int numero;
    private string texto;
    private List<string> lista;

    // 3. El Constructor (Lleva el nombre de la clase)
    private Singleton() 
    {
        // Obligatorio instanciar la lista para que no dé error al usar .Add()
        lista = new List<string>(); 
    }

    // 4. El método que crea y devuelve (El que ya armaste perfecto)
    public static Singleton ObtenerInstancia()
    {
        if (instanciaUnica == null)
        {
            instanciaUnica = new Singleton();            
        }
        return instanciaUnica;
    }
    
    // ... Acá abajo irían tus métodos Get y Set para numero, texto y lista ...
    public int GetNumero()
    {
        return numero;
    }

    public void SetNumero(int valor)
    {
        numero = valor;
    }

    public string getTexto()
    {
        return texto;
    }

    public void setTexto(string valor)
    {
        
        texto = valor;
    }


    public void agregarElemento(string item)
    {
        lista.Add(item);
    }

    public List<string> getLista()
    {
        return lista;
    }
}

class SingletonCaller
{
    public void calling()
    {
        Singleton Instancia = Singleton.ObtenerInstancia();
        Console.WriteLine($"Valor recuperado desde SingletonCaller: {Instancia.GetNumero()}");
    
    }
}

class Program
{
    static void Main()
    {
        Singleton miInstancia = Singleton.ObtenerInstancia();
        
        miInstancia.SetNumero(15);
        miInstancia.setTexto("Hola");
        miInstancia.agregarElemento("Primer Elemento");

        Console.WriteLine($"Los valores son: Numero: {miInstancia.GetNumero()}, texto: {miInstancia.getTexto()} y lista: {miInstancia.getLista()[0]}");


        SingletonCaller Caller = new SingletonCaller();
        Caller.calling();
    }
}