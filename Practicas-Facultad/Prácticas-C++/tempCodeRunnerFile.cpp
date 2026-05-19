int main() {

int resultado = 1;
int numero;
cin >> numero;


for(int i = 1; i <= numero; i++){
    resultado = resultado * i;
    cout << resultado << endl;
}

return 0;
