using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Velocidad base, rigidBody, variabe estatica para cambiar el sentido de movimiento
    public float runningSpeed = 1.5f;
    private Rigidbody2D rigidbody;
    public static bool turnAround = false;
    private Vector3 startPosition;

    //Asociamos el rigidbody
    void Awake()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        

    }

    private void Start()
    {

        this.startPosition = this.transform.position;

    }

    void FixedUpdate()
    {

        if(turnAround == true)
        {

            this.transform.eulerAngles = new Vector3(0f, 180.0f, 0f);
        }
        else
        {

            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        }

        //Si estamos en juego que se desplace
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {

            if(this.rigidbody.velocity.x < runningSpeed)
            {
                 
                if(turnAround == false)
                {

                    rigidbody.velocity = new Vector2(runningSpeed, rigidbody.velocity.y);

                }
                else
                {

                    rigidbody.velocity = new Vector2(-runningSpeed, rigidbody.velocity.y);

                }

            }

        }

    }
}
