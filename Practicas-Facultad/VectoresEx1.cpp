#include <iostream>
using namespace std;


int main(){
    int n;
    cin >> n;
    int vec[n]; 

for (int i = 0; i < n; i++) {
    vec[i] = (i + 1)*2;
    cout << vec[i] << endl;
}

}