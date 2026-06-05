using System.Dynamic;
using System.Collections.Generic;
using System.ComponentModel;

public abstract class Vagon
{
    public abstract double CalcularPesoMaximo();

}
public class VagonPasajeros : Vagon
{
    private double largo, anchoUtil;

    public double GetLargo()
    {
        return largo;
    }

    public double GetAnchoUtil()
    {
        return anchoUtil;
    }
    public void SetLargo(double nuevoLargo)
    {
        largo = nuevoLargo;
    }
    public void SetAnchoUtil(double nuevoanchoUtil)
    {
        anchoUtil = nuevoanchoUtil;
    }

    public VagonPasajeros(){
        
    }

    public VagonPasajeros(double largo, double anchoUtil)
    {
        this.anchoUtil = anchoUtil;
        this.largo = largo;
    }

    public double CalcularPasajeros()
    {
        if(anchoUtil <= 2.5)
        {
            return largo * 8;
        }
        else
        {
            return largo * 10;
        }
        
        
        }

    public override double CalcularPesoMaximo()
    {
        return CalcularPasajeros() * 80;
    }
}

public class VagonCarga : Vagon
{
    private double cargaMaxima;

    public double GetCargaMaxima()
    {
        return cargaMaxima;
    }
    public void SetCargaMaxima(double nuevaCargaMaxima)
    {
        cargaMaxima = nuevaCargaMaxima;
    }
    public VagonCarga()
    {
        
    }
    public VagonCarga(double cargaMaxima)
    {
        this.cargaMaxima = cargaMaxima;
    }
    public override double CalcularPesoMaximo()
    {
        return cargaMaxima + 160;
    }
}

public class Locomotora
{
    private double peso, pesoMaximoArrastre, velocidadMaxima;

    public double GetPeso()
    {
        return peso;
    }

    public double GetPesoMaximoArrastre()
    {
        return pesoMaximoArrastre;
    }
    public double GetVelocidadMaxima()
    {
        return velocidadMaxima;
    }

    public void SetPeso(double nuevoPeso)
    {
        peso = nuevoPeso;
    }
    public void SetPesoMaximoArrastre(double nuevoPesoMaximoArrastre)
    {
        
        pesoMaximoArrastre = nuevoPesoMaximoArrastre;
    }
    public void SetVelocidadMaxima(double nuevaVelocidadMaxima)
    {
        velocidadMaxima = nuevaVelocidadMaxima;
    }
    public Locomotora()
    {
        
    }
    public Locomotora(double peso, double pesoMaximoArrastre, double velocidadMaxima)
    {
        this.peso = peso;
        this.velocidadMaxima = velocidadMaxima;
        this.pesoMaximoArrastre = pesoMaximoArrastre;
    }
    public double CalcularArrastreUtil()
    {
        return pesoMaximoArrastre - peso;
    }

}

public class Formacion
{
    private List<Vagon> vagones;
    private List<Locomotora> locomotoras;
    
    public Formacion()
    {
        vagones = new List<Vagon>();
        locomotoras = new List<Locomotora>();
    }
    public int ContarVagonesLivianos()
    {
    int contador = 0;
        foreach(Vagon tmp in vagones)
        {
            if(tmp.CalcularPesoMaximo() < 2500)
            {
                contador++;
            }
        }
        return contador;
    }
    public double CalcularVelocidadMaxima()
    {
        double velocidadMinima = 99999;

        foreach(Locomotora tmp in locomotoras)
        {
            if(tmp.GetVelocidadMaxima() < velocidadMinima)
            {
                velocidadMinima = tmp.GetVelocidadMaxima();
            }
        }
        return velocidadMinima;
    }
    public bool EsEficiente()
    {
        foreach(Locomotora tmp in locomotoras)
        {
            if(tmp.GetPesoMaximoArrastre() < tmp.GetPeso() * 5)
            {
                return false;
            }
        }
        return true;
    }
    private double CalcularPesoTotalVagones()
    {
        double sumaPesoTotal = 0;
        foreach(Vagon tmp in vagones){
            sumaPesoTotal += tmp.CalcularPesoMaximo();
        }
                return sumaPesoTotal;
    }
    private double CalcularArrastreTotalLocomotoras()
    {
        double sumaArrastreTotal = 0;
        foreach(Locomotora tmp in locomotoras)
        {
            sumaArrastreTotal += tmp.CalcularArrastreUtil();        
        }
        return sumaArrastreTotal;

    }
    public bool PuedeMoverse()
    {
        return CalcularPesoTotalVagones() <= CalcularArrastreTotalLocomotoras();
    }
    public double CalcularEmpujeFaltante()
    {
        if(PuedeMoverse() == true)
        {
            return 0;
        }
        else
        {
            return CalcularPesoTotalVagones() - CalcularArrastreTotalLocomotoras();
        }        
    }
    public Vagon ObtenerVagonMasPesado()
    {
        Vagon vagonGanador = null;
        double pesoMasAlto = 0;

        foreach(Vagon tmp in vagones)
        {
            if(tmp.CalcularPesoMaximo() > pesoMasAlto)
            {
                vagonGanador = tmp;
                pesoMasAlto = tmp.CalcularPesoMaximo();
            }
        }
        return vagonGanador;
    }
    private double CalcularPesoTotalLocomotoras()
    {
        double peso = 0;
        foreach(Locomotora tmp in locomotoras)
        {
            peso += tmp.GetPeso();
        }
        return peso;
    }
    public bool EsCompleja()
    {
        
        
    }
}
public class Deposito
{
    private List<Formacion> formaciones;
    private List<Locomotora> locomotoras;
    public Deposito()
    {
        formaciones = new List<Formacion>();
        locomotoras = new List<Locomotora>();
    }
    public List<Vagon> ObtenerVagonesMasPesados()
    {
        List<Vagon> resultado;
        resultado = new List<Vagon>();
        foreach(Formacion tmp in formaciones)
        {
            resultado.Add(tmp.ObtenerVagonMasPesado());
        }
        return resultado;
    }
}
