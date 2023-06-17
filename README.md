# AmyContraObelix

Programar en Unity un videojuego en 3D llamado AmyContraObelix, completando el proyecto
base entregado según se detalla a continuación.
Se deben programar las mecánicas de control básico del personaje principal, usando la clase
CharacterController, de la cámara, y las interacciones que se describen a continuación.

## Movimiento del personaje principal
Programaremos el movimiento del personaje principal implementando una forma de control en 3ª
persona bastante común en los videojuegos.
Crearemos un script PlayerControl partiendo de un ejemplo de script de control que figura en la
documentación de Unity en la siguiente dirección:
https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
Con este script tendremos un control básico el movimiento de Amy en 8 direcciones, cuatro
correspondientes a los ejes X y Z y otras cuatro correspondientes a las direcciones intermedias.
También dispondremos del control de salto. Por ahora, nos conformaremos con este
comportamiento.

## Movimiento de la cámara
Crear, usando el paquete Cinemachine, un objeto de tipo VirtualCamera y configurarlo para que
siga a Amy. De esta manera conseguimos una cámara que reacciona a los movimientos del
personaje. Podemos configurar el comportamiento de la cámara en aspectos como la velocidad a la
que se acerca al personaje, el margen en el apuntado y el margen en el que el apuntado es
progresivo frente a cuando es inmediato.

## Animación de Amy
Crear un animador que permita que Amy ejecute las animaciones idle, walking y jumping. Desde el
script de control de Amy ordenar las transiciones adecuadas.

## Control combinado del personaje y la cámara
Para conseguir un comportamiento más flexible podemos sofisticar el control del personaje
mediante PlayerControl y seguir dejando que la cámara lo siga, o podemos usar un control de
cámara más sofisticado y aprovecharlo para controlar al personaje a partir de ella. Usaremos esta
última opción. La idea es que la cámara pueda girar en torno al personaje, controlada por el ratón, y
que el personaje se mueva en la dirección hacia dónde apunta la cámara.
Podemos lograr esto usando la cámara FreeLookCamera de Cinemachine y haciendo algunos
ajustes en el script PlayerControl.
Recién creada esta cámara permite el movimiento en torno al personaje al que sigue, tanto girando
horizontalmente en torno a él, como moviéndose verticalmente entre los límites marcados por los
rig inferior y el superior.
En PlayerControl obtendremos una referencia a la cámara y modificaremos el vector move del
script, para que su movimiento sea con referencia a la cámara.

## Control del choque de la cámara con los muros
Añadir el componente CinemachineCollider. Este componente sirve para evitar que la cámara
atraviese muros y otros objetos y nos encontremos mirando hacia el personaje con un muro en
medio. El ajuste fundamental de este componente es la máscara de capas con las que se detecta la
colisión y las etiquetas que se pueden excluir de la colisión.

## Movimiento de la doble puerta
Este obstáculo está formado por dos puertas situadas a ambos lados de un hueco que atraviesa el
muro. El movimiento de las puertas debe ser cinemático, sin ser resultado de la aplicación de
fuerzas. Ambas puertas se mueven de forma independiente una de la otra, aunque el movimiento de
ambas tiene un control común que las para o pone en marcha según se explica a continuación.
Las dos puertas permanecerán paradas siempre que Amy no esté en contacto con la zona de
detección incluida en el objeto PlayerDetector1. Esta zona de detección consiste en un
BoxCollider establecido en modo Trigger. Las puertas, por lo tanto, comenzarán el juego paradas
y permanecerán así hasta que Amy ingrese en la zona de detección o siempre que la abandone. En
este último caso, cada puerta quedará en la posición que ocupe en ese momento. A la hora de
reiniciar el movimiento deberán continuar desde la posición en que hayan quedado paradas.
El movimiento de ambas puertas debe hacerse de modo que se acelere al partir de uno de sus
extremos, sea máximo en el centro del recorrido y se desacelere a partir de ahí hasta pararse en el
otro extremo. Este comportamiento puede conseguirse usando las funciones Mathf.PingPong() y
Mathf.SmoothStep() de Unity para calcular las posiciones que ocupará la puerta en cada frame.
El movimiento de la puerta HorizontalDoor debe ser en su eje Z (y, por lo tanto, horizontal). Se
debe mover 2 metros en cada sentido desde la posición que ocupa inicialmente, que será la central
de su recorrido. En efectuar el movimiento de vaivén completo debe invertir 2.5 segundos.
El movimiento de la puerta VerticalDoor debe ser en su eje Y (y, por lo tanto, vertical). Se debe
mover 2 metros hacia arriba desde la posición que ocupa inicialmente, siendo ésta la posición
inferior de su recorrido. En efectuar el movimiento completo de subida y bajada debe invertir 1.8
segundos.

## Movimiento de la doble puerta rotatoria
Este obstáculo está formado por dos puertas situadas una a continuación de la otra en un hueco del
muro. El movimiento de las puertas debe ser cinemático, sin ser resultado de la aplicación de
fuerzas. Ambas puertas giran en sentido contrario una de la otra, y ambas comparten un control
común que las para o pone en marcha según se explica a continuación.
Las dos puertas permanecerán paradas siempre que Amy no esté en contacto con la zona de
detección incluida en el objeto PlayerDetector2. Esta zona de detección consiste en un
BoxCollider establecido en modo Trigger. Las puertas, por lo tanto, comenzarán el juego paradas
y permanecerán así hasta que Amy ingrese en la zona de detección o siempre que la abandone. En
este último caso, cada puerta quedará en la posición que ocupe en ese momento. A la hora de
reiniciar el movimiento deberán continuar desde la posición en que hayan quedado paradas.
Ambas puertas se moverán a una velocidad angular de 90 grados/s. La puerta de la izquierda
(mirado desde el lado del muro en que Amy comienza el juego) debe moverse en el sentido de las
agujas del reloj (mirado desde arriba) y la puerta de la derecha en sentido antihorario.

## Menhires
Hay dos menhires en la escena que Amy debe poder empujar. Para empujarlos, Amy debe
comprobar si tiene enfrente de si, a un máximo de 0.5 metros un objeto con la tag “Pushable”. La
detección debe hacerse a la altura aproximada de los hombros de Amy. Si es así debe empujar ese
objeto, para lo cual lo desplazará en la misma dirección y distancia que se desplaza la propia Amy,
teniendo en cuenta que se debe eliminar la componente vertical del movimiento del personaje. Para
mover el menhir, se debe crear un componente Pushable que se incorporará a los menhires. Este
componente tendrá un método Push(). Este método recibirá un Vector3 como parámetro, y
desplazará el menhir en la dirección y distancia especificados por éste, comportándose en este
aspecto igual que el método Move de CharacterController.
El menhir debe mantener su posición en el mundo cuando, al ser empujado por Amy, ésta gire. Es
decir, no debe girar con Amy como si fuese un objeto hijo del Transform de ella. Expresado de otra
manera, Amy empuja el menhir, no carga con el.
El menhir además deberá controlar la colisión con otros objetos para evitar atravesar muros y
situaciones similares. Para ello, uno de los menhires del proyecto tiene un Rigidbody, por lo que
actuará de modo correcto sin ninguna acción adicional. Pero en el caso del otro, deberemos
controlar desde el componente Pushable, usando Raycast, que no haya nada en el camino del
menhir. El chequeo se hará lanzando Raycast desde cada uno de los puntos de detección de
colisiones que están definidos en la superficie del menhir. El Raycast se lanzará en la misma
dirección y con la misma dirección que el desplazamiento que se pretende realizar.
Solo se debe empujar cuando Amy se mueve hacia adelante, y siempre que el objeto a empujar esté
delante de ella. No se debe mover el menhir cuando Amy se mueve hacia atrás ya sea alejándose o
acercándose al menhir, ni cuando se mueva hacia adelante pero el objeto no esté delante de ella. El
mecanismo de detección del objeto en frente de Amy, que se menciona más arriba, debe ser
suficiente para los casos en que Amy se mueve sin tener enfrente el menhir, pero cuando Amy esta
enfrente y cerca del menhir y comienza a moverse hacia atrás, se necesitará un mecanismo extra
para evitar que el menhir la acompañe como si ella estuviese tirando de el.
Tampoco se puede empujar si Amy está en el aire. Por lo tanto si al aproximarse al menhir, Amy
salta, al chocar con el menhir este no se moverá. Solo cuando Amy llegue al suelo, si sigue
avanzando, comenzará a mover el menhir.
Además de todo lo anterior, cuando Amy esté empujando, deberá ejecutar la animación push.
Deberá haber transiciones desde los estados Run y Idle a Push y viceversa. No se contemplan otras
transiciones que pueden llegar a darse en el juego.

## Interacción con objetos
Crearemos una serie de objetos a los que pondremos la etiqueta Pickable. Amy podrá cargar estos
objetos situándose frente a ellos al pulsar el jugador el botón Interaction. Si Amy tiene uno de
estos objetos cargados lo soltará al pulsar la misma tecla. El objeto cargado se situará delante del
cuerpo de Amy, más o menos de la altura de sus hombros hacia abajo. Amy también podrá elevarlo
más, hasta 0.6 metros por encima de esa posición para dejarlo en algún lugar alto, pero no podrá
caminar ni saltar con el objeto cargado en una posición elevada. Para elevar y bajar el objeto
usaremos los botones PushUp y PushDown, que definiremos como las teclas R y F, respectivamente.
La detección de objetos Pickable se hará con un RayCast a un 0.7 metros de altura a una distancia
de hasta 0.5 m. Si no se detecta un objeto a esa altura se buscará a 0.35 m. De esta manera se da
prioridad al objeto superior si hay dos apilados.

## Manzanas, naranjas, limones
Crearemos tres prefabs que representarán tres tipos de fruta. Por simplicidad, consistirán
simplemente en pequeñas esferas de color verde, naranja y amarillo. Estos prefabs incorporarán un
script Fruit, que permitirá que cuando sean capturados por Amy registren su puntuación en el
GameManager.
Cada fruta tendrá una puntuación según su tipo (100, 120 y 150 para manzanas, limones y naranjas
respectivamente) y un tiempo de permanencia en la escena de 30 segundos. Transcurrido el tiempo
de permanencia se destruirán.
IES Muralla Romana Curso 2022-2023 Lugo
P.M.V. 2º Curso
Estas frutas aparecerán aleatoriamente en el entorno de juego (no a lo largo de toda la plataforma),
pero siempre a una distancia mínima respecto a Amy de 4m. Se situarán a 1 metro de altura.
La captura de la fruta ocurrirá simplemente por pasar lo suficientemente cerca del objeto. Al
capturar una fruta se reproducirá el sonido fruit_captured.mp3.

## Paso de nivel
Cuando se alcance una puntuación igual o superior a 1000 puntos (aunque pondremos un valor
menor para hacer pruebas), se podrá subir de nivel, para lo que aparecerá en la escena un
transportador dimensionalTM que consistirá en una pequeña plataforma a la que, una vez que Amy
se suba, se dará por finalizado el nivel, perdiendo el jugador el control sobre Amy. A partir de este
momento se realizará mediante una corrutina un desvanecimiento de la escena reduciendo la
intensidad de la luz a razón de 0.002 de intensidad cada centésima de segundo. Una vez la
intensidad llegue a 0.02 o menos se iniciará una cuenta de 2 segundos tras los cuales se cargará el
siguiente nivel, AmyYLasPlataformas.
