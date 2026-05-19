#include <iostream>
using namespace std;

int main() {

int n;
cout << "Ingresa N: ";
cin >> n;

int gg[n]; // El vector que contiene todos los números

cout << "Ingrese " << n << " numeros:" << endl;
for(int i = 0; i < n; i++){
    cin >> gg[i];
}

int contador = 0;

for(int i = n - 1; i >= 0; i--){
    contador++;
    if(contador % 10 == 0){
        cout << gg[i] << endl;
    }   else{
        cout << gg[i] << " ";
    }
}





}