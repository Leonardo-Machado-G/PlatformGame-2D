using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    public float speed = 0.0f;

    void Awake()
    {
        
        this.rigidBody = GetComponent<Rigidbody2D>();

    }

    
    void FixedUpdate()
    {

        //Velocidad de movimiento del escenario
        this.rigidBody.velocity = new Vector2(speed, 0);

        //Posicion del padre //De mi posicion dame la de mi padre x
        float parentPosition = this.transform.parent.transform.position.x;

        //Desplazar la capa
        if(this.transform.position.x - parentPosition >= 27)
        {
            //Coordenadas relativas
            this.transform.position = new Vector3(parentPosition - 27, this.transform.position.y, this.transform.position.z);

        }

    }

}
