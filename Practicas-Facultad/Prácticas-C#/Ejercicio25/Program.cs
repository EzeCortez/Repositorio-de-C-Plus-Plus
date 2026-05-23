using System;
using System.Collections.Generic;

public interface IMolinete
{
    int obtenerIngresos();
    int obtenerEgresos();

}

public class Rotativity : IMolinete
{
    private int[] GetUsage()
    {
        return new int[] {50, 45};
    }

    public int obtenerIngresos()
    {
        int[] datosLeidos = GetUsage();        
        int losIngresos = datosLeidos[0];
        return losIngresos; 
    }

    public int obtenerEgresos()
    {
        return GetUsage()[1];
    }
}
