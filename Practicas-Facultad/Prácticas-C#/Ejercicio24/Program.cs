using System;
using System.Collections.Generic;


public interface IMolinete
{
    int obtenerIngresos();
    int obtenerEgresos();

}

public class Giramax : IMolinete
{
    public int obtenerIngresos()
    {
        return 1;
    }
    public int obtenerEgresos()
    {
        return 1;
    }
    
}

public class Molinit : IMolinete
{
    public int obtenerIngresos()
    {
        return 1;
    }
    public int obtenerEgresos()
    {
        return 1;
    }
    
}

class Program
{
    static void Main()
    {
        List<IMolinete> listaMolinetes = new List<IMolinete>();
        Giramax primerGiramax = new Giramax();
        listaMolinetes.Add(primerGiramax);
        Molinit primerMolinit = new Molinit();
        listaMolinetes.Add(primerMolinit);
        Dictionary<string, int> contador = new Dictionary<string, int>();
        foreach(IMolinete molineteGuardado in listaMolinetes)
        {
            Console.WriteLine($"GIRAMAX:\n  Los Ingresos son: {primerGiramax.obtenerIngresos()}");
            Console.WriteLine($"    Y los Egresos son: {primerGiramax.obtenerEgresos()}");
        }
    }
    
}

