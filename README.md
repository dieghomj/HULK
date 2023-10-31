# HULK

Havana University Language Kompiler

## Correr el proyecto

Para correr el proyecto abra una terminal en la carpeta donde se encuentra el proyecto y escriba la siguiente linea:
    `dotnet run -v q --project HULK`

## Funcionalidades

### Keywords implementadas

`if ( <bool expression> ) <expression> else <expression>`: Si la condicion es verdadera realiza la primera expresion en caso contrario la segunda.

`let <assignment expression>, ... in <expression>`: El tipo de expresion luego de la keyword `let` debe ser de asignacion, toda variable que haya sido asignado podra ser usada luego de `in`.

`function <function name>( <variable name>, ... ) <expression>`: Declara una funcion, luego para llamarla solo se debe usar la siguiente syntax `<function name>( <expression>, ... )` incluyendo dentro de los parentesis la misma cantidad de expresiones que variables en la declaracion. 

#### Funciones predefinidas

`print( <expression> )`: Imprime en consola y devuelve la misma expresion.

`rnd()`: Devuevle un un numero aleatorio entre 0 y 1 (inclusivo).

`sqrt( <expression> )`: Devuelve la raiz cuadrada de la expresion.

`exp( <expression> )`: Devuelve $e$ elevado a la expresion,

`sen( <expression> )`: Devuelve el seno de la expresion.

`cos( <expression> )`: Devuelve el coseno de la expresion.

`tan( <expression> )`: Devuelve la tangente de la expresion.

`cot( <expression> )`: Devuelve la cotangente de la expresion.

`log( <expression>, <expression>)`: Devuelve el logaritmo de la segunda expresion en base de la primera.

### Comandos externos a **HULK**

`#showtree`:    Muestra el AST.

`#clear`:    Limpia la consola. _(**puede que en algunas consolas no funcione**)_
