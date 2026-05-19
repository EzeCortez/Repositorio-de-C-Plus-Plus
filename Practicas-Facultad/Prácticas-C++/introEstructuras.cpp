#include <iostream>
using namespace std;


// defino mi estructura 
struct Cliente
{
    int subscriberId;
    string email;
    string name;
    int fechaNacimiento; //YYYYMMDD
};

void mostrarCliente(Cliente clien){
    cout << "Cliente nro:" << cli.subscriberId << clienteAMostrar.subscriberId << endl;
    cout << "email" << clien.email << clienteAMostrar.email << endl;
    cout << "nombre" << clien.name << clienteAMostrar.name << endl;
}

void mostrarClientes(Clinete clientesAMostrar[], int n){
    for (int i = 0; i < n; i++)
    {

        mostrarCliente(clientesAMostrar[i]);
    }
}

int main(){
    
    Cliente cli;
    Cliente misClientes[10];

    cout << "ingrese fecha de nacimiento: " << endl; 
    cin >> cli.fechaNacimiento = 20001103;

    cli.subscriberId = 123123;
    cli.email = "ezenicortez@gmail.com";
    cli.name = "John Doe";

    mostrarCliente(cli);






    return 0;
}
