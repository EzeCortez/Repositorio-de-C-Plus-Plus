using System;

public class Articulo
{
    private string Marca, Modelo;
    public void SetMarca(string nuevaMarca){
        Marca = nuevaMarca;
    }
    public void SetModelo(string nuevoModelo)
    {
        Modelo = nuevoModelo;
    }
}
class Program
{
    static void Main()
    {
        Articulo miArticulo = new Articulo();
        miArticulo.SetMarca("Logitech");
        miArticulo.SetModelo("MX Master 3");
        Console.WriteLine(miArticulo.SetMarca);
    }
}