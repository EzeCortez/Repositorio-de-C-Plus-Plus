#include <iostream>
using namespace std;

int main() {

int n;
cout << "Ingresa N: ";
cin >> n;

int valor[n]; // El vector que contiene todos los números

cout << "Ingrese " << n << " numeros:" << endl;
for(int i = 0; i < n; i++){
    cin >> valor[i];
}

int maximo = valor[0];

for(int i = 0; i < n; i++){
    if(valor[i] > maximo){
        maximo = valor[i];
    }
    
}
cout << "El valor mas alto es: " << maximo;

cout << "Se encontro en la(s) siguiente(s) posicion(es):" << endl;
for(int i = 0; i < n; i++){
    if(valor[i] == maximo){
        cout << "Indice: " << i << endl;
    }
}

}