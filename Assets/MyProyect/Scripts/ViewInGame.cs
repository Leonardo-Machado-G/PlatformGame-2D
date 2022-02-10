using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewInGame : MonoBehaviour
{
    //Declaro las tres variables text que utilizare e importo el paquete
    public Text collectableLabel;
    public Text scoreLabel;
    public Text maxscoreLabel;

    void Update()
    {
        //Si estoy en modo de juego inGame se pondra en la variable un texto con la cantidad de monedas
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {

            int currentObjects = GameManager.sharedInstance.collectedItems;
            this.collectableLabel.text = currentObjects.ToString();


        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {

            //pasar la distancia a texto y ponerle pocos decimales indicandolo con f1
            float travelledDistance = Script_Louis2D.sharedInstance.GetDistance();
            this.scoreLabel.text = "Score\n" + travelledDistance.ToString("f1");
            float maxScore = PlayerPrefs.GetFloat("maxScore", 0);
            this.maxscoreLabel.text = "MaxScore\n" + maxScore.ToString("f1");

        }

    }

}
