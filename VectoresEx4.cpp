#include <iostream>
using namespace std;

int main() {
    int n;
    cout << "Ingresa N: ";
    cin >> n;
    int vec[n];
    

    for(int i = 0; i < n; i++){
        cin >> vec[i];
    }

    int suma = 0;
    for(int i = 0; i < n; i++){        
        suma = suma + vec[i];
    }

    if (suma > 0){
        cout << "\nLa suma es positiva. Imprimiendo elementos de indice impar:" << endl;
        for (int i = 0; i < n; i++) {            
            if (i % 2 != 0) {
                cout << vec[i] << endl;
            }                
        }
    } else{
        cout << "\nLa suma no es positiva. Imprimiendo elementos de indice par:" << endl;
        for (int i = 0; i < n; i++) {            
            if (i % 2 == 0) {
                cout << vec[i] << endl;
            }
        }
    }




}