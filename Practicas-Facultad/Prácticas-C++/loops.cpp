

                                 // Ejercicio 1: Dados dos valores enteros A y B, pedir al usuario que ingrese valores, e informar la suma, la resta y el producto.
   /* 
   int a;
   int b;

   cout << "Ingrese el primer valor: " << endl;
   cin >> a;1
   cout << "Ingrese el segundo valor: " << endl;
   cin >> b;
   cout << "El valor es: " << endl;
   
   a = a + b;

   cout << a; */




                                 // Ejercicio 2: A partir de un valor entero ingresado por teclado, se pide informar:
                                    //a) La quinta parte de dicho valor
                                    //b) El resto de la división por 5
                                    //c) La séptima parte del resultado del punto a)

   /* int a;
   float b;
   cout << "Ingrese el valor a descomponer " << endl;
   cin >> a;
   cout << "Primero, la quinta parte de " << a << " es: ";
   b = a / 5;
   cout << b << endl;
   cout << "Segundo, el resto de la division por 5 es: ";
   b = a % 5;
   cout << b << endl;
   cout << "Tercero, la septimaparte del resultado del punto A es: ";
   b = a / 7;
   cout << b << endl;
   return 0; */
   



                                 //Ejercicio 3: Dada una terna de números naturales que representan al día, al mes y al año de una 
                                 //determinada fecha informarla como un solo número natural de 8 dígitos con la forma (AAAAMMDD).
   /* int dia;
   int mes;
   int año;
   int fecha;
   cout << "Ingrese la fecha en formato AAAAMMDD: " << endl;
   cin >> fecha;
   cout << "El numero quedo asi: " << fecha << endl;
   dia = fecha % 100;
   cout << "El dia es: " << dia << endl;
   mes = (fecha % 10000) / 100;
   cout << "El mes es: " << mes << endl;
   año = (fecha % 100000000) / 10000;
   cout << "El año es: " << año << endl;
   return 0; */



/* /*Ejercicio 1
Dados dos valores enteros A y B, informar la suma, la resta y el producto */

  /*  int a;
   int b;

   cout << "Ingresa dos valores, primero A: "<< endl;
   cin >> a;
   cout << "Ahora B: " << endl;
   cin >> b;

   cout << "La suma es: " << a + b << endl;
   cout << "La resta es: " << a - b << endl;
   cout << "El producto es: " << a * b << endl;


   return 0; */



/* Ejercicio 2
Solicitar al usuario que ingrese 3 notas y calcular el promedio e informarlo por pantalla */

/* int nota1;
int nota2;
int nota3;

cout << "Favor ingrese la nota del parcial 1: " << endl;
cin >> nota1;
cout << "Favor ingrese la nota del parcial 2: " << endl;
cin >> nota2;
cout << "Favor ingrese la nota del final: " << endl;
cin >> nota3;

cout << "El promedio es: " << (nota1 + nota2 + nota3) / 3 << endl;
return 0; */


/*Ejercicio 3
Dada la longitud de un lado de un cuadrado calcular e informar por pantalla:
a) El perímetro del cuadrado
b) El área del cuadrado */

/* int lado1 = 10;
int lado2 = 10;
int lado3 = 10;
int lado4 = 10;

cout << "El perimetro del cuadrado es " << lado1 + lado2 + lado3 + lado4 << endl;
return 0; */


/* Ejercicio 4
Dado el radio de un círculo calcular e informar:
a) El área del círculo (se obtiene multiplicando el radio al cuadrado por el número Pi)
b) El perímetro del círculo (se obtiene multiplicando el diámetro por el número Pi) */

/* float pi = 3.14;
float radio;
float cir;
float dia;
float area;
float perim;

cout << "Ingrese el radio del circulo: " << endl;
cin >> radio;
cir = 2 * pi * radio;
dia = cir / pi;
area = pi * (radio * radio);
perim = pi * dia;

cout << "La circunferencia es: " << cir << endl
<< "El diametro es: " << dia << endl
<< "El area es: " << area << endl 
<< "El perimetro es " << perim << endl;
return 0; */

/* Ejercicio 5
Dados una terna de números naturales que representan al día, al mes y al año de una
determinada fecha informarla como un solo número natural de 8 dígitos con la forma
(AAAAMMDD). */










/* Ejercicio 6
Dada un número entero de la forma (AAAAMMDD), que representa una fecha valida
mostrar el día, mes y año que representa. */





/* Ejercicio 7
A partir de un valor entero ingresado por teclado, se pide informar:
a) La quinta parte de dicho valor
b) El resto de la división por 5
c) La séptima parte del resultado del punto a) */







/* Ejercicio 8
Dada una cantidad de segundos informar por pantalla el mismo valor pero en horas,
ejemplo: 4860000 milisegundos son 1,35 horas */


/* double mili;
double horas;

cout << "Ingrese los MS que su servidor tiene actualmente: " << endl;
cin >> mili;

horas = mili / 10;
 

cout << "Eso representa " << horas << " cantidad de horas" << endl;
return 0; */



/* Ejercicio 9
Similar al anterior, dada una cantidad de horas informar por pantalla cuántos milisegundos
son. */








/* 1. Realizar un programa que informe si un número es POSITIVO o NEGATIVO. */

/* int a;


cout << "Ingrese un numero " << endl;
cin >> a;

if (a > 0)
   cout << "POSITIVO" << endl;

   else {

      
      cout << "NEGATIVO" << endl;
   }
return 0; */



/* 2. Realizar un programa que informe si un número es PAR o IMPAR. Precondición:No
se ingresa Cero. */


/* int a;
cout << "Ingrese un numero" << endl;
cin >> a;

if (a % 2 == 0 && a > 0){
   cout << "PAR" << endl;
}   else {
      if(a < 0){
         cout << "Negativo, no se puede" << endl;
      } else {
            cout << "INPAR" << endl;
      }
      
   
return 0;
} */




/* 3. Realizar un programa que informe el precio a abonar total de una cuenta con dos
productos. Si la suma de ambos productos es mayor a $ 10.000, el producto de
menor valor debe tener un 30% de descuento. */

/* int main() {
    int a;
    int b;

    cout << "Ingrese A" << endl;
    cin >> a;
    cout << "Ingrese B" << endl;
    cin >> b;

    if (a + b > 10000 && a + b > 0) { 
        if (a > b) { 
            b = b * 0.70;
            cout << "Descuento en B, total = " << a + b << endl;
        } else { 
            a = a * 0.70;
            cout << "Descuento en A, total = " << a + b << endl;
        }
    } else if (a + b < 0) { 
        cout << "NEGATIVO, ERROR" << endl;
    } else { 
        cout << "No hay descuento, total = " << a + b << endl;
    }

    return 0; 
} */




/* 4. Número de la suerte: 
Realizar un algoritmo que evalúe si un número ingresado por el usuario es número de la suerte. 
Para ser número de la suerte, el número debe ser 
positivo, impar, y múltiplo de 3. 
También si el número es 20 o 80 es número de lasuerte. 
Se solicita que esta lógica se encuentre en un solo condicional. */


/*  int main () {

   int a;
   cout << "Ingrese el numero:" << endl;
   cin >> a;

   if (a > 0 && a % 2 != 0 && a % 3 == 0 || a == 20 || a == 80) {
      cout << "SI es suerte" << endl;
   } else{
      cout << "No es suerte" << endl;
   }
return 0;
} */





/* 5. Realizar un programa que solicite dos números al usuario. Luego que le consulte
qué operación desea realizar sobre esos números, pudiendo ser: 1: Suma, 2: Resta, 3: Multiplicación y 4: División. En función de la selección del usuario, se debe
devolver el resultado. Si el usuario elige otra opción, se debe dar el mensaje de que
la opción no es válida. */


/* main (){
   int a;
   int b;
   int option;

   cout << "Primer valor" << endl;
   cin >> a;
   cout << "Segundo valor" << endl;
   cin >> b;

   cout << "Que operacion desea realizar? \n 1: Suma \n 2: Resta \n 3: Multiplicacion \n 4: Division"<< endl;

   cin >> option;
   switch (option){
      case 1:
         cout << "La suma da = " << a + b << endl;
         break;
      case 2:
         cout << "La suma da = " << a - b << endl;
         break;
      case 3:
         cout << "La suma da = " << a * b << endl;
         break;
      case 4:
         cout << "La suma da = " << a / b << endl;
         break;
      default:
         if (a < 0 || b < 0){
           cout << "sin negativos wacho" << endl;
         } else {            
            cout << "Opcion incorrecta" << endl;
         }
   }




   return 0;
} */


/* Consigna: Escribir una función llamada esPar que reciba un número entero, y devuelva 1 si el número es par y 0 si es impar.

ATENCIÓN: No solicitar valores por consola, ni mostrar por pantalla, ni escribir la función main. */


#include <iostream>
using namespace std;

int main() {
    int numero;
    
    
    cout << "Ingresa un numero (0 o negativo para salir): ";
    cin >> numero;
    
    
    while (numero > 0) {
        
        bool esPrimo = true;
        if (numero <= 1) {
            esPrimo = false; 
        } else {
            for (int i = 2; i <= numero / 2; i++) {
                if (numero % i == 0) { 
                    esPrimo = false;
                    break;
                }
            }
        }
        
        
        if (esPrimo) {
            cout << numero << " es primo" << endl;
        } else {
            cout << numero << " no es primo" << endl;
        }
        
        
        cout << "Ingresa otro numero (0 o negativo para salir): ";
        cin >> numero;
    }
    
    cout << "Programa terminado." << endl;
    return 0;
}





