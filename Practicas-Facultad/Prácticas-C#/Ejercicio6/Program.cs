using System;

public class Fruta
{
    private string color;
    private int peso;
    private bool esEstacional;

    public void setColor(string nuevoColor)
    {
        color = nuevoColor;
    }

    public void setPeso(int nuevoPeso)
    {
        peso = nuevoPeso;
    }

    public void setEstacional(bool nuevoEstacional)
    {
        esEstacional = nuevoEstacional;
    }

    public string getColor()
    {
        return color;
    }

    public int getPeso()
    {
        return peso;
    }

    public bool getEstacional()
    {
        return esEstacional;
    }

    public bool esComestible()
    {
        return (peso < 200 && esEstacional == true);
    }

    // Constructor 1: Sin valores iniciales (vacío)
    public Fruta()
    {
    }

    // Constructor 2: Con los tres valores al momento de instanciarse
    public Fruta(string colorInicial, int pesoInicial, bool estacionalInicial)
    {
        color = colorInicial;
        peso = pesoInicial;
        esEstacional = estacionalInicial;
    }
}

class Program
{
    static void Main()
    {
        // Opción 1: Usando el constructor vacío.
        // La fruta nace "en blanco" y usamos los setters para llenarla paso a paso.
        Fruta manzana = new Fruta();
        manzana.setColor("Rojo");
        manzana.setPeso(150);
        manzana.setEstacional(true);

        // Opción 2: Usando el constructor completo.
        // La fruta nace con absolutamente todos sus datos listos en una sola línea.
        Fruta sandia = new Fruta("Verde", 5000, true);

        // Comprobamos la lógica de tu método esComestible()
        Console.WriteLine($"¿La Manzana es comestible? {manzana.esComestible()}");
        // Dará True (pesa menos de 200 y es estacional)

        Console.WriteLine($"¿La sandía es comestible? {sandia.esComestible()}"); 
        // Dará False (pesa mucho más de 200)
    }
}