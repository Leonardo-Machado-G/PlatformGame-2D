using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveZone : MonoBehaviour
{

    //Variable para limitar el numero de zonas destruidas
    float timeSinceLastDestruction = 0.0f;

    //Metodo para cuando atravesamos el collider se destruyan zonas
    //Existen formas de evitar hacer esto cuando se tiene dos colliders
    //Se utilizaria el collider compuesto porque si tienes dos identicos hay problemas
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(timeSinceLastDestruction > 3.0f)
        {
            
            LevelGenerator.sharedInstance.AddLevelBlock();
            LevelGenerator.sharedInstance.RemoveOldLevelBlock();
            timeSinceLastDestruction = 0.0f;

        }

    }

    private void Update()
    {

        timeSinceLastDestruction += Time.deltaTime;

    }

}
