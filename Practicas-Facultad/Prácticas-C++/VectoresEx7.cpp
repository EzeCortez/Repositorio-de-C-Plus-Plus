#include <iostream>
using namespace std;

int main() {

int n;
cout << "Ingresa N: ";
cin >> n;

int dato[n]; // El vector que contiene todos los números

cout << "Ingrese " << n << " numeros:" << endl;
for(int i = 0; i < n; i++){
    cin >> dato[i];
}
int mayor;
int segundoMayor;

if(dato[1] < dato[0]){
    mayor = dato[0];
    segundoMayor = dato[1];
}   else{
    mayor = dato[1];
    segundoMayor = dato[0];
}

for(int i = 2; i < n; i++){
    if(dato[i] > mayor){
        segundoMayor = mayor;
        mayor = dato[i];     
    }   else{
        if(dato[i] > segundoMayor){
            segundoMayor = dato[i];
        }
    }
}





}