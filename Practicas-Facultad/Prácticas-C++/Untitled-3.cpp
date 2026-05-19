#include <iostream>
using namespace std;

int calcular(int a, int b) {
    return a * b;
}

void operacion(int x, int &y) {
    y = calcular(x, y);
    x = x + 1;
}

int main() {
    int a = 5, b = 3;
    operacion(a, b);
    cout << a << " " << b << endl;
    return 0;
}