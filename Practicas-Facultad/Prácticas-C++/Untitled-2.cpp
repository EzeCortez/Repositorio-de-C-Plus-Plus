#include <iostream>
using namespace std;

int elevarCuadrado(int &alias) {
    alias = alias * alias;
    return alias;
}

void saludo() {
    cout << "Re jodido esto jsjsss " << endl;
}

int main() {
    int numerousuario;
    numerousuario = 2;
    cout << numerousuario << endl;

    cout << elevarCuadrado(numerousuario) << endl;

    cout << "El cuadrado de 2 es: " << numerousuario << endl;

    saludo();

    return 0;
}
