using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColectableType{

    healthPotion,
    manaPotion,
    money

}


public class Collectable : MonoBehaviour
{

    public ColectableType type = ColectableType.money;

    //Variable para saber si la moneda ha sido recogida
    private bool isCollected = false;
    public int value = 0;
    public AudioClip collecSound;

    //Metodo para activar la moneda y su collider
    void Show()
    {

        //Activamos el sprite de la moneda, la imagen
        //Por defecto los sprites estan desactivados pero se podrian activar mediante esta forma
        //El Sprite activa la animacion tambien
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        this.isCollected = false;

    }

    //Metodo para ocultar la moneda
    void Hide()
    {

        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        //this.GetComponent<CircleCollider2D>().enabled = false;

    }

    //Metodo para recolectar la moneda
    void Collect()
    {

        this.isCollected = true;
        Hide();
        //Para usar sonidos en conveniente tener solo 1 audio source e ir asociando mediante parametros a cada sonido el correspondiente
        AudioSource audio = GetComponent<AudioSource>();

        if (audio != null && this.collecSound != null)
        {

            audio.PlayOneShot(this.collecSound);

        }

        //En funcion de lo que sea hara una cosa u otra
        switch(this.type)
        {

            case ColectableType.money:
                {

                    GameManager.sharedInstance.CollectItem(this.value);

                    break;

                }

            case ColectableType.healthPotion:
                {

                    Script_Louis2D.sharedInstance.CollectHealth(this.value);

                    break;

                }

            case ColectableType.manaPotion:
                {

                    Script_Louis2D.sharedInstance.CollectMana(this.value);

                    break;

                }

        }

    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {

        if(otherCollider.tag == "Player")
        {

            Collect();

        }

    }
}
