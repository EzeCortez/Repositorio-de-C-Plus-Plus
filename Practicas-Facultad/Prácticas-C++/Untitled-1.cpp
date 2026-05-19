#include <iostream>
using namespace std;

struct Alumno {
    char legajo[8];
    char apellidoYNombre[25];
    int nota;
};

int main()
{
    int contador;


    Alumno unAlumno;
    Alumno vectorDeAlumnos[100];

    cout << "Ingrese legajo: ";
    cin >> unAlumno.legajo;

    cout << "Ingrese nombre y apellido: ";
    cin >> unAlumno.apellidoYNombre;
    return 0;
}