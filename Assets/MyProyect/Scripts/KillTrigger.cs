using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {

            Script_Louis2D.sharedInstance.KillPlayer();

        }

    }

}
