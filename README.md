Ejercicio 1 – Corrección y análisis de calidad de código en C#

1. Descripción del código original

El código original llamado "BadCalcVeryBad" implementaba una calculadora en consola, pero tenía varios problemas de calidad:
- Usaba ArrayList sin tipo para el historial.
- Utilizaba instrucciones goto para controlar el flujo del programa.
- Tenía bloques catch vacíos que escondían los errores.
- Había código innecesario (archivos creados sin usarse, variables que no se utilizaban).
- La conversión de textos a números no estaba controlada y podía lanzar excepciones.

2. Cambios realizados en el código

A partir de ese código se hicieron las siguientes mejoras:

- Se creó una clase simple Globals con un List<string> para manejar el historial de operaciones en lugar de ArrayList, lo que hace el código más claro y con tipos definidos.
- Se eliminaron variables y código que no aportaban a la calculadora (por ejemplo Random, any y archivos temporales raros).
- Se reemplazaron los goto por un ciclo while con una variable booleana "salir", lo que hace el flujo del programa más fácil de entender.
- Se mejoró el manejo de errores:
  - En la división se valida si el divisor es cero y se lanza un mensaje de error claro.
  - Ya no se usan bloques catch vacíos, ahora se muestra el mensaje de la excepción al usuario.
- Se agregó el método LeerNumero para leer los datos desde consola usando double.TryParse, permitiendo tanto coma como punto y evitando que el programa se caiga por una conversión inválida.
- Se centralizó el registro de resultados en el método RegistrarYMostrarResultado, que:
  - Muestra el resultado en consola.
  - Guarda la operación en la lista de historial.
  - Mantiene el formato a|b|operación|resultado.
- Al final de la ejecución se guarda el historial en el archivo "history.txt" utilizando File.WriteAllLines.

3. Resultados del análisis en SonarQube

El proyecto corregido se analizó con SonarQube Community en la máquina local.  
En el panel del proyecto se observa que:

- La puerta de calidad aparece como aprobada.
- No se reportan bugs ni vulnerabilidades.
- La mantenibilidad tiene calificación A y no se muestran code smells relevantes.
- No hay código duplicado y la cobertura aparece en 0 % porque no se configuraron pruebas automatizadas.

Se tomó una captura de pantalla del panel de SonarQube donde se ven estas métricas como evidencia del análisis.

4. Conclusiones

Después de las correcciones, el programa sigue siendo una calculadora sencilla en consola, pero ahora:
- Es más fácil de leer y entender.
- Maneja mejor los errores de entrada del usuario.
- Evita malas prácticas como el uso de goto y los bloques catch vacíos.
- Utiliza estructuras más adecuadas como List<string>.

El análisis con SonarQube muestra que el código cumple con las reglas básicas de calidad configuradas para el proyecto, lo que demuestra la importancia de revisar y mejorar el código antes de su entrega.
