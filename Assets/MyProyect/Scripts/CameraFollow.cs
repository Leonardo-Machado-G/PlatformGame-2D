using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /*
    public static CameraFollow sharedInstance;
    */
    //Recordar cambiar los ajustes de configuracion
    //Coordenada del personaje al que seleccionaremos y seguiremos
    public Transform target;

    //Cantidad de movimiento que voy a mover la camera y la altura a la que comienza, el desplazamiento respecto del target
    public Vector3 offset = new Vector3(0.1f, 0.0f, -10.0f);

    //Tiempo de ajuste que tarda la camara en comenzar a seguir
    public float dampTime = 0.3f;

    //Velocidad de desplazamiento de la camara
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {

        //Unity trata de renderizar al limite de frames que le indiquemos
        Application.targetFrameRate = 60;

    }

    private void Update()
    {

        //Camera camera = GetComponent<Camera>();
        //Estabamos uniendo las coordenadas del player con la de la camara para que se desplacen igual

        //WorldToViewPoint transforma las coordenadas en las de la camara
        //Es decir el jugador se situa en un origen respecto a la camara y te devuelve esas coordenadas
        //Si escribimos Debug, nos marcaria 10 porque el personaje esta en el 0 y nosotros en -10
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);

        //Desde la camara de arriba  hasta la siguiente instruccion ya cambia de coordenada y da problemas
        //De coordenadas del mundo a coordenadas de pantalla y viceversa
        //Target es posicion exacta del personaje - donde este la camara respecto del offset, de la pequeña diferencia entre
        //El player y la camara, dondr quiero ir - donde esta ahora, es la cantidad que se le suma a la camara
        //De un frame al otro
        Vector3 delta = target.position - GetComponent<Camera>().WorldToViewportPoint(new Vector3(offset.x, offset.y, point.z));

        //Conociendo donde esta ahora y la pequeña cantidad de movimiento que tiene que moverse creamos el destino
        //donde ira situado la camara
        Vector3 destination = point + delta;

        //Ahora corregimos el eje y, z de la camara, porque delta sube los 2 ejes
        destination = new Vector3(destination.x, offset.y ,offset.z);

        //Asignamos la posicion de la camara conforme hemos configurado
        //Smooth suaviza los cambios de posicion(donde esta, donde quiere ir, ref velocity suaviza el cambio de velocidad si lo hay,
        //damptime es el tiempo de ajuste para comenzar a seguir
        this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampTime);

    }

    public void ResetCameraPosition()
    {

        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().WorldToViewportPoint(new Vector3(offset.x, offset.y, point.z));
        Vector3 destination = point + delta;
        destination = new Vector3(destination.x, offset.y, offset.z);
        this.transform.position = destination;

    }

}
