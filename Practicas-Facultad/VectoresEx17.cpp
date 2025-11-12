#include <iostream>
using namespace std;

int main() {

    // --- Preparación ---
    int m;
    cout << "Ingrese la cantidad de FILAS (M): ";
    cin >> m;

    int n;
    cout << "Ingrese la cantidad de COLUMNAS (N): ";
    cin >> n;

    int MATRIZA[m][n];

    // --- Carga de la Matriz ---
    cout << "Ingrese los " << m * n << " elementos de la matriz por filas:" << endl;
    for (int i = 0; i < m; i++) {
        // CORREGIDO: El bucle interno debe ir hasta 'n' (columnas)
        for (int j = 0; j < n; j++) { 
            cout << "Elemento [" << i << "][" << j << "]: ";
            cin >> MATRIZA[i][j];
        }
    }

    // --- a) Imprimir por Columnas ---
    cout << "\n--- Matriz impresa por columnas ---" << endl;
    // CORREGIDO: El bucle externo va hasta 'n' (columnas)
    for (int j = 0; j < n; j++) {
        // CORREGIDO: El bucle interno va hasta 'm' (filas)
        for (int i = 0; i < m; i++) {
            cout << MATRIZA[i][j] << "\t"; // Agregué un tabulador para formato
        }
        cout << endl;
    }

    // --- b) Calcular el Promedio ---
    double suma = 0;
    for (int i = 0; i < m; i++) {
        for (int j = 0; j < n; j++) {
            // CORREGIDO: 'i' debe ser minúscula
            suma += MATRIZA[i][j];        
        }
    }
    double promedio = suma / (m * n);
    cout << "\nEl promedio de todos los elementos es: " << promedio << endl;


    // --- c) Suma de Columnas ---
    cout << "\n--- Suma por Columnas ---" << endl;
    int VECSUMCOL[n];
    for (int j = 0; j < n; j++) {
        int sumaColumna = 0; 
        for (int i = 0; i < m; i++) {
            sumaColumna += MATRIZA[i][j];        
        }
        VECSUMCOL[j] = sumaColumna;
        // CORREGIDO: Eliminada la línea 'k++' que estaba aquí
    }
    // Agregado: Bucle para imprimir el resultado de la suma de columnas
    for (int j = 0; j < n; j++) {
        cout << "Columna " << j << ": " << VECSUMCOL[j] << endl;
    }

    // --- d) Máximo de Filas ---
    int VECMAXFIL[m];
    for(int i = 0; i < m; i++){
        int wawa = MATRIZA[i][0];
        for(int j = 1; j < n; j++){
            if(MATRIZA[i][j] > wawa){
                wawa = MATRIZA[i][j];
            }
        }
        VECMAXFIL[i] = wawa;
    }

    // Agregado: Impresión de los máximos por fila
    cout << "\n--- Maximo por Fila ---" << endl;
    for (int i = 0; i < m; i++) {
        cout << "Fila " << i << ": " << VECMAXFIL[i] << endl;
    }

    return 0;
}