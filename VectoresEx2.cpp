#include <iostream>
using namespace std;

int main() {
    int n;
    cout << "Ingrese el valor menor a 30" << endl;
    cin >> n;
    int vec[n];

    // --- PASO 1: Llenar el vector ---
    cout << "Ingrese " << n << " numeros:" << endl;
    for (int i = 0; i < n; i++) {
        cin >> vec[i];
    }

    // --- PASO 2: Procesar el vector (DESPUÉS de llenarlo) ---
    if (vec[n - 1] < 10) {
        cout << "\nImprimiendo negativos:" << endl;
        for (int i = 0; i < n; i++) {
            if (vec[i] < 0) {
                cout << vec[i] << endl;
            }
        }
    } else {
        cout << "\nImprimiendo los demas:" << endl;
        for (int i = 0; i < n; i++) {
            // Corregimos para que incluya al cero
            if (vec[i] >= 0) {
                cout << vec[i] << endl;
            }
        }
    }

    return 0;
}