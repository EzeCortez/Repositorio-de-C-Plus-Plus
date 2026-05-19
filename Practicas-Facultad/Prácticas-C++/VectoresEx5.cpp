#include <iostream>
using namespace std;

int main() {

int n;
cout << "Ingresa N: ";
cin >> n;

int UNO[n];
int DOS[n];
int TRES[n];

for(int i = 0; i < n; i++){
    cout << "Ingrese " << n << " numeros para el vector UNO:" << endl;
    cin >> UNO[i];
}

for(int i = 0; i < n; i++){
    cout << "Ahora, ingrese " << n << " numeros para el vector DOS:" << endl;
    cin >> DOS[i];
}

for(int i = 0; i < n; i++){
    if(i % 2 == 0){
        TRES[i] = UNO[i];
    } else{        
        TRES[i] = DOS[i];
    }    
}

cout << "\nEl vector TRES resultante es:" << endl;

for(int i = 0; i < n; i++){
    cout << TRES[i] << " ";
}
cout << endl;

}