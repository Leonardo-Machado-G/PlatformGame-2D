using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMoviment : MonoBehaviour
{
    //Indicador de movimiento del enemigo
    public bool movingForward = true;

    //Al atravesar su trigger solitaria indicarle que se de la vuelta
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
       
        if(movingForward == true)
        {
            
            Enemy.turnAround = true;

        }
        else
        {

            Enemy.turnAround = false;

        }

        movingForward = !movingForward;

    }

}
