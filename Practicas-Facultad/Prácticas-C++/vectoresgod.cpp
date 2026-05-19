#include <iostream>
using namespace std;


// defino mi estructura 
struct Legajo{
    int numero_legajo;
    int anio_inscripcion;
};

struct Alumno
{
    string apellido;
    string nombre;
    Legajo legajo;
};



int busquedaPorLegajo(Alumno[], int, int);



int main(){
    
    int legajoUser;
    Alumno alumnoUser;


    Legajo legajos[5] = {
            {1001, 2020},
            {1002, 2021},
            {1003, 2022},
            {1004, 2023},
            {1005, 2024}
    };
    string nombres[5]  = {
        "Lucas", 
        "Martina", 
        "Santiago", 
        "Valentina", 
        "Mateo"
    };
    string apellidos[5] = {
        "Gomez", 
        "Lopez", 
        "Fernandez", 
        "Diaz", 
        "Ruiz"
    };


    Alumno misAlumnos[5];

    for(int i = 0; i < 5; i++){
        misAlumnos[i].nombre = nombres[i];
        misAlumnos[i].apellido = apellidos[i];
        misAlumnos[i].legajo = legajos[i];
    }

    cout << "ingrese legajo del alumno: ";
    cin >> legajoUser;

    alumnoUser = misAlumnos[busquedaPorLegajo(misAlumnos, 5, legajoUser)];


    return 0;

}

int busquedaBinaria(Alumno arr[], int n, int x) {

    // Cuando arranco evalúo todo el vector de 0 a n - 1
    int inicio = 0;
    int fin = n -1;
    while (fin >= inicio) {
        int mitad = inicio + (fin - inicio) / 2;
        
        // Si el elemento es el del medio, devolvemos la posicion
        if (arr[mitad].legajo.numero_legajo == x)
            return mitad ;

        // Si el elemento es menor entonces solo puede estar en la primer mitad
        if (arr[mitad].legajo.numero_legajo > x) {
            fin = mitad - 1; //Cambio el limite superior
            } else {
            inicio = mitad + 1; // Cambio el limite inferior
        }
    }
    // Si llegamos hasta aca es que el elemento no estaba
    return -1;
} 