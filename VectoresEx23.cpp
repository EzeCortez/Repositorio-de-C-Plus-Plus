#include <iostream>
using namespace std;

int main() {
    int n;
    cout << "Ingrese la cantidad de numeros en el vector: ";
    cin >> n;
    int vec[n];


    cout << "Ingrese los " << n << " numeros:" << endl;
    for (int i = 0; i < n; i++) {
        cin >> vec[i];
    }

    int dato;
    cout << "\nQue numero desea buscar? ";
    cin >> dato;

    bool encontrado = false;
    int posicion[n];
    int cantEncontrados = 0;
    int primeraPosicion = -1;
    int ultimaPosicion = -1;
    for (int i = 0; i < n; i++) {
        if(vec[i] == dato){
            encontrado = true;
            posicion[cantEncontrados] = i;
            cantEncontrados ++;
            ultimaPosicion = i;
            if(primeraPosicion == -1){
                primeraPosicion = i; 
            }            
        }        
    }

    if(encontrado == true){
            cout << "Se encontraron " << cantEncontrados << "resultados, en las posiciones ";
            for(int i = 0; i < cantEncontrados; i++){
                cout << posicion[i] << ", ";
            }
            cout << "La primera posición encontrada fue " << primeraPosicion;
            cout << "La ultima posición encontrada fue " << ultimaPosicion;
        } else{
            cout << "No se encontro nada" << endl;
        }




}