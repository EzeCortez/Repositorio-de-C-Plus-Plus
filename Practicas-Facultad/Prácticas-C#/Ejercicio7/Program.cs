using System;

public class Ninja
{
    private string arteMarcial, arma;
    private int fuerza, salto;


    public void setarteMarcial(string nuevoarteMarcial)
    {
        arteMarcial = nuevoarteMarcial;
    }

    public void setArma(string nuevoArma)
    {
        arma = nuevoArma;
    }

    public void setFuerza(int nuevaFuerza)
    {
        fuerza = nuevaFuerza;
    }
    public void setSalto(int nuevoSalto)
    {
        salto = nuevoSalto;
    }

    public string getarteMarcial()
    {
        return arteMarcial;
    }

    public string getArma()
    {
        return arma;
    }
    public int getFuerza()
    {
        return fuerza;
    }
    public int getSalto()
    {
        return salto;
    }
    public void Saltar(int multiplicador)
{
    Console.WriteLine($"Salto con multiplicador: {salto * multiplicador}");
}
    public void Ataque()
{
    Console.WriteLine($"Atacando con {arma} usando {arteMarcial}");
}
}

class Program
{
    static void Main()
    {
        Ninja miNinja1 = new Ninja();
        miNinja1.setarteMarcial("Taekwondo");
        miNinja1.setArma("Lunchaku");
        miNinja1.setFuerza(100);
        miNinja1.setSalto(10);
        miNinja1.Saltar(2);
        miNinja1.Ataque();

        Ninja miNinja2 = new Ninja();
        miNinja2.setarteMarcial("Karate");
        miNinja2.setArma("Katana");
        miNinja2.setFuerza(120);
        miNinja2.setSalto(2);
        miNinja2.Saltar(4);
        miNinja2.Ataque();

    }
}