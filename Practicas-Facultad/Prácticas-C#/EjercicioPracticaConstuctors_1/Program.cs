using System.Dynamic;
/*

public class Frutata
{
    private string color;
    private double peso;
    private bool esEstacional;

    public Frutata()
    {
        
    }

    public Frutata(string color, double peso, bool esEstacional)
    {
        this.color = color;
        this.peso = peso;
        this.esEstacional = esEstacional;
    }
    public string GetColor()
    {
        return color;
    }
    public double GetPeso()
    {
        return peso;
    }
    public bool GetEstacional()
    {
        return esEstacional;
    }
    public void SetColor(string nuevoColor)
    {
        color = nuevoColor;
    }
    public void SetPeso(double nuevoPeso)
    {
        peso = nuevoPeso;
    }
    public void SetEstacional(bool nuevoEstacional)
    {
        esEstacional = nuevoEstacional;
    }

    public bool EsComestible()
    {
        return (peso < 200 && esEstacional == true);
    }
}
*/

public class VagonPasajeros
{
    private double largo;
    private double anchoUtil;

    public VagonPasajeros()
    {
        
    }
    public VagonPasajeros(double largo, double anchoUtil)
    {
        this.largo = largo;
        this.anchoUtil = anchoUtil;
    }
    public double GetLargo()
    {
        return largo;
    }
    public double GetanchoUtil()
    {
        return anchoUtil;
    }
    public void SetLargo(double nuevoLargo)
    {
        largo = nuevoLargo;
    }
    public void setanchoUtil(double nuevoanchoUtil)
    {
        anchoUtil = nuevoanchoUtil;
    }
    public double CalcularPasajeros()
    {
        if (anchoUtil <= 2.5)
        {
            return largo * 8;
        }
        else
        {
            return largo * 10;
        }
    }

    public class VagonCarga
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
        public double CalcularPesoMaximo()
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
            this.pesoMaximoArrastre = pesoMaximoArrastre;
            this.velocidadMaxima = velocidadMaxima;
        }
        public double CalcularArrastreUtil()
        {
            return pesoMaximoArrastre - peso;
        }
    }
    public class Formacion
    {
        private List<VagonPasajeros> vagonesPasajeros;
        private List<Locomotora> locomotoras;
        private List<VagonCarga> vagonesCarga;

        public Formacion()
        {
            vagonesPasajeros = new List<VagonPasajeros>();
            locomotoras = new List<Locomotora>();
            vagonesCarga = new List<VagonCarga>();
        }
            public double CalcularTotalPasajeros()
        {
            double total = 0;
            foreach (VagonPasajeros totalTmp in vagonesPasajeros)
            {
                total += totalTmp.CalcularPasajeros();
            }
            return total;
        }

        
    }

}