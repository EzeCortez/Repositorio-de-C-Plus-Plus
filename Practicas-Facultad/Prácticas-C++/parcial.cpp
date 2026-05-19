#include <iostream>
using namespace std;

int main(){
    int a = 5;
    int b = 1;

    while(a > b){
        a--;
        while(b < a){
            b++;
            cout << a << "-" << b << endl;
        }
    }

    return 0;
}