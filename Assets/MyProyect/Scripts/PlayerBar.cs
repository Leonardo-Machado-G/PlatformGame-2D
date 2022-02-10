using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{

    //Tipos de barras
    public enum BarType
    {

        health, mana

    }

    //Asociamos el Slider
    public BarType type;
    private Slider slider;

    //Asociaos la variable con el slider object
    void Awake()
    {

        this.slider = GetComponent<Slider>();

    }


    void Update()
    {
        
        //cargamos constantemente los valores de las barras
        switch (this.type)
        {

            case BarType.health:
            {

                    this.slider.value = Script_Louis2D.sharedInstance.GetHealth();

                break;

            }
            case BarType.mana:
            {

                    this.slider.value = Script_Louis2D.sharedInstance.GetMana();

                break;

            }

        }

    }
}
