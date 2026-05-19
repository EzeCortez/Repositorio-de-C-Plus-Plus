#include <iostream>
using namespace std;

int main() {
    int n;
    cout << "Ingrese N: ";
    cin >> n;

    long long fac[n];
    int vec[n];

    cout << "Ingrese " << n << " numeros:" << endl;
    for (int i = 0; i < n; i++) {
        cin >> vec[i];
    }

    
    for (int i = 0; i < n; i++) {
        long long resultado_factorial = 1;
        int numero_a_calcular = vec[i];

        for (int j = 1; j <= numero_a_calcular; j++) {
            resultado_factorial = resultado_factorial * j;
        }

        fac[i] = resultado_factorial;
    }

    cout << "\nNumero\tFactorial" << endl;
    for (int i = 0; i < n; i++) {
        cout << vec[i] << "\t" << fac[i] << endl;
    }
    
    return 0;
}
