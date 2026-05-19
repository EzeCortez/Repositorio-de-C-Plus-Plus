#include <iostream>
using namespace std;

int main() {

int m;
cout << "Ingresa M: ";
cin >> m;

int n;
cout << "Ingresa N: ";
cin >> n;

int A[m], B[n], TOTAL[m + n];

for (int i = 0; i < m; i++){
    cin >> A[i];
}

for (int i = 0; i < n; i++){
    cin >> B[i];
}

int i = 0, j = 0, k = 0;

while(i < m && j < n){
    if(A[i] < B[j]){
        TOTAL[k] = A[i];
        i++;
        k++;
    } else{
        TOTAL[k] = B[j];
        j++;
        k++;
    }
}

while(i < m){
    TOTAL[k] = A[i];
    i++;
    k++;    
}

cout << "\nEl vector TOTAL resultante es:" << endl;
    for (int idx = 0; idx < m + n; idx++) {
        cout << TOTAL[idx] << " ";
    }
    cout << endl;

    return 0;
}