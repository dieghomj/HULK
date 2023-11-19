# HULK

Havana University Language Kompiler

## Correr el proyecto

Para correr el proyecto abra una terminal en la carpeta donde se encuentra el proyecto y escriba la siguiente línea: `sh run.sh` o `dotnet run -v q --project HULK`.

## Funcionalidades

### Keywords implementadas

#### Condiciones

```js
if ( <bool expression> ) <expression> else <expression>
```

Si la condición es verdadera realiza la primera expresión en caso contrario la segunda.


#### Declaracion de variables

```js
let <assignment expression>, ... in <expression>
```

El tipo de expresion luego de la keyword `let` debe ser de asignación, toda variable que haya sido asignado podrá ser usada luego de `in`.

#### Funciones

```js
function <function name>( <variable name>, ... ) => <expression>
```

Declara una función, luego para llamarla solo se debe usar la siguiente syntax `<function name>( <expression>, ... )` incluyendo dentro de los paréntesis la misma cantidad de expresiones que variables en la declaración.

#### Funciones predefinidas

`print( <expression> )`: Imprime en consola y devuelve la misma expresión.

`rnd()`: Devuelve un un número aleatorio entre 0 y 1 (inclusivo).

`sqrt( <expression> )`: Devuelve la raiz cuadrada de la expresión.

`exp( <expression> )`: Devuelve $e$ elevado a la expresión,

`sen( <expression> )`: Devuelve el seno de la expresión.

`cos( <expression> )`: Devuelve el coseno de la expresión.

`tan( <expression> )`: Devuelve la tangente de la expresión.

`cot( <expression> )`: Devuelve la cotangente de la expresión.

`log( <expression>, <expression>)`: Devuelve el logaritmo de la segunda expresión en base de la primera.

### Errores

Existen tres tipos de errores:

#### LEXICAL ERROR

Este tipo de errores son encontrados en el analisis lexico. Ejemplos:

Un cararcter desconocido:

```js
>>>1 + ■;
! LEXICAL ERROR: Bad character input: '■'
```

No terminar la expresion con `;`: 
```js
>>>1+2
! LEXICAL ERROR: Expected ';'
```

#### SYNTAX ERROR

Este tipo de errores son encontrados en el analisis sintactico.
Ejemplos:

Uso de una sintaxis incorrecta:

```js
>>>let a in 2;
! SYNTAX ERROR: Unexpected token <IdentifierToken> expected <EqualsToken>
```

#### SEMANTIC ERROR

Este tipo de errores son encontrados en el analisis semantico
Ejemplos:

Mal uso de un operador binario:

```js
>>>1 + true;
! SEMANTIC ERROR: Binary operator '+' is not defined for types 'Double' and 'Bool'
```

Uso de una variable sin declarar:

```js
>>>1 + X;
! SEMANTIC ERROR: Variable 'X' doesn't exist
```

### Comandos externos a **HULK**

`#showtree`:    Muestra el AST.

`#clear`:    Limpia la consola. _(**puede que en algunas consolas no funcione**)_
