using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Louis2D : MonoBehaviour
{

    public static Script_Louis2D sharedInstance;


    //mana y vida
    private int healthPoints;
    private int manaPoints;

    //Variables del personaje sobre su Sprite, colisiones y animaciones

    private Vector3 startPosition;
    public Animator animator;
    private Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public LayerMask groundLayer;

    //Velocidad de correr y andar

    private float walkSpeed = 1f;
    private float runningSpeed = 2.4f;
    private float jumpSpeed = 5f;

    //Variables que cambiaran de correr a andar y girar el Sprite

    private bool isRunning = false;
    private bool isWalk = true;
    private bool isStop = false;
    private bool isJump = false;
    private bool mirrorSprite = true;

    //Varibles que permiten pulsador dos veces una tecla, correr y parar

    private KeyCode lastKey;
    private float timeRunning;
    private int timeStop;

    //Variables para cambiar las coordenadas cartesianas (x,y)

    private float axisVelocityX;
    private float axisVelocityY;


    //Resetea la vida y el mana del jugador
    private const int INITIAL_HEALTH = 100, INITIAL_MANA = 100;


    //Es un metodo que se ejecuta cada segundo o lo que queramos
    //Ejecuta el comando cada medio segundo
    IEnumerator TirePlayer()
    {

        while(this.healthPoints <= 100 && this.healthPoints >= 0)
        {

            this.healthPoints--;

            yield return new WaitForSeconds(0.5f);

        }
        //Si la curutina acaba la desactivas hasta el proximo frame
        yield return null;

    }


    private void Awake()
    {

        sharedInstance = this;
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.startPosition = transform.position;

    }
    

    public void StartGame()
    {
        //Reinicio de condiciones y animaciones
        animator.SetBool("isStop", false);
        animator.SetBool("isWalk", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isDead", false);
        SetIsRunning(false);
        SetIsWalk(false);
        SetIsStop(false);
        SetIsJump(false);
        SetIsStop(false);
        transform.position = this.startPosition;
        
        this.healthPoints = INITIAL_HEALTH;
        this.manaPoints = INITIAL_MANA;

        //Llamamos a la Corutina
        StartCoroutine("TirePlayer");
        
    }
    
    //Valores fijos en los frames el update no lo es
    void FixedUpdate()
    {

        //Limitador para que solo se pueda desplazar en el juego
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {

            if(this.healthPoints <= 0)
            {

                KillPlayer();

            }

        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            
            //Controladores de estar quieto, parar al correr y saltar
            ControllerIdle();
            ControllerStop();
            ControllerJump();

            // Teclas para andar y paso a correr
            if (GetIsRunning() == false && GetIsWalk() == true && GetIsStop() == false)
            {

                ControllerKeyWalkLeft();
                ControllerKeyWalkRight();

            }

            //  Comienzo a correr y  paso a parar y teclas para elegir la dirección
            if (GetIsWalk() == false && GetIsRunning() == true)
            {

                animator.SetBool("isRunning", true);
                animator.SetBool("isWalk", false);

                ControllerKeyRunLeft();
                ControllerKeyRunRight();

            }

        }

    }
    
    //Desplazamiento de la tecla A        
    private void ControllerKeyRunRight()
    {

        if (this.lastKey == KeyCode.A && this.lastKey != KeyCode.D)
        {

            SetVelocityAxisXY(-GetRunningSpeed(), 0);
            SetTimeController(15);

            if (Input.GetButtonDown("Izquierda") && GetIsJump() == false)
            {

                animator.SetBool("isRunning", false);
                animator.SetBool("isStop", true);

                SetIsRunning(false);
                SetIsWalk(false);
                SetIsStop(true);

            }

        }

    }

    //Desplazamiento de la tecla D    
    private void ControllerKeyRunLeft()
    {

        if (this.lastKey == KeyCode.D && this.lastKey != KeyCode.A)
        {

            SetVelocityAxisXY(GetRunningSpeed(), 0);
            SetTimeController(15);

            if (Input.GetButtonDown("Derecha") && GetIsJump() == false)
            {

                animator.SetBool("isRunning", false);
                animator.SetBool("isStop", true);

                SetIsRunning(false);
                SetIsWalk(false);
                SetIsStop(true);

            }

        }

    }
    

    // Tecla A para andar y paso a correr 
    private void ControllerKeyWalkRight()
    {

        if (Input.GetButton("Derecha") && !Input.GetButton("Izquierda"))
        {

            if (GetMirrorSprite())
            {

                spriteRenderer.transform.Rotate(Vector3.up * -180);
                SetMirrorSprite(false);

            }
            
            animator.SetBool("isWalk", true);
            SetVelocityAxisXY(-GetWalkSpeed(), 0);

        }

        if (Input.GetButtonDown("Derecha") && !Input.GetButtonDown("Izquierda") && GetIsJump() == false)
        {

            if (this.lastKey == KeyCode.A && Time.time - GetTimeRunning() < 0.5f && Time.time - GetTimeRunning() > 0.1f)
            {

                SetIsRunning(true);
                SetIsWalk(false);

            }

            this.lastKey = KeyCode.A;
            SetTimeRunning(Time.time);

        }

    }

    //Tecla D para andar  y paso a correr
    private void ControllerKeyWalkLeft()
    {

        if (Input.GetButton("Izquierda") && !Input.GetButton("Derecha"))
        {

            if (!GetMirrorSprite())
            {

                spriteRenderer.transform.Rotate(Vector3.up * -180);
                SetMirrorSprite(true);

            }

            animator.SetBool("isWalk", true);
            SetVelocityAxisXY(GetWalkSpeed(), 0);

        }

        if (Input.GetButtonDown("Izquierda") && !Input.GetButtonDown("Derecha") && GetIsJump() == false)
        {

            if (Time.time - GetTimeRunning() < 0.5f && this.lastKey == KeyCode.D && Time.time - GetTimeRunning() > 0.1f)
            {

                SetIsRunning(true);
                SetIsWalk(false);

            }

            this.lastKey = KeyCode.D;
            SetTimeRunning(Time.time);

        }

    }

    //Secuencia de parada y paso a estar quieto    
    private void ControllerStop()
    {

        if (GetIsWalk() == false && GetIsRunning() == false && GetIsJump() == false)
        {

            this.timeStop -= 1;

            if (this.timeStop <= 0 && GetIsRunning() == false && GetIsWalk() == false && GetIsJump() == false)
            {

                animator.SetBool("isStop", false);

                SetIsStop(false);
                SetIsWalk(true);
                SetIsRunning(false);

            }

        }

    }

    //Secuencia de estar   quieto     
    private void ControllerIdle()
    {

        if (GetIsRunning() == false && GetIsStop() == false)
        {

            animator.SetBool("isWalk", false);

        }

    }

    //Secuencia de salto y regreso a andar/correr 
    private void ControllerJump()
    {

        if (GetIsStop() == false && Input.GetButtonDown("Jump") && GetIsJump() == false && GetStartJump() == true)
        {
                
            SetVelocityAxisXY(0f, GetJumpSpeed());
            animator.SetBool("isJump", true);
            SetIsJump(true);

        }
        
        if(GetIsStop() == false && GetStartJump() == true && GetIsJump() == false)
        {

            animator.SetBool("isJump", false);

        }

        if (GetStartJump() == true)
        {
            
            SetIsJump(false);

        }

    }

    // Muerte del jugador 
    public void KillPlayer()
    {

        animator.SetBool("isStop", false);
        animator.SetBool("isWalk", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isDead", true);
        SetIsRunning(false);
        SetIsWalk(false);
        SetIsStop(false);
        SetIsJump(false);
        SetIsStop(false);
        GameManager.sharedInstance.GameOver();

        //Este valor guarda en un fichero datos para ser utilizados, porque ya no es ram
        float currentMaxScore = PlayerPrefs.GetFloat("maxScore", 0);

        //1ºObten la maxima puntuacion conocida, si no existe es cero
        //2º Si la puntuacion maxima es menor que la actual cambiala por la nueva
        if(currentMaxScore < this.GetDistance())
        {

            PlayerPrefs.SetFloat("maxScore", this.GetDistance());

        }

    }

    //Sistema de coordenadas (x,y)
    private void SetVelocityAxisXY(float axisVelocityX, float axisVelocityY)
    {

        if (axisVelocityX == 0f)
        {

            this.axisVelocityX = rigidBody.velocity.x;

        }
        else
        {

            this.axisVelocityX = axisVelocityX;

        }

        if (axisVelocityY == 0f)
        {

            this.axisVelocityY = rigidBody.velocity.y;

        }
        else
        {

            this.axisVelocityY = axisVelocityY;

        }
        
        rigidBody.velocity = new Vector2(this.axisVelocityX, this.axisVelocityY);

    }

    // Get y Set de velocidades 
    private float GetRunningSpeed()
    {

        if (this.healthPoints > 50)
        {

            SetRunningSpeed(3f);
            return this.runningSpeed;

        }
        else
        {

            SetJumpSpeed(2.4f);
            return this.runningSpeed;

        }

    }

    private void SetRunningSpeed(float speedRunning)
    {

        this.runningSpeed = speedRunning;

    }

    private float GetWalkSpeed()
    {

        return this.walkSpeed;

    }

    private void SetWalkSpeed(float speedWalk)
    {

        this.walkSpeed = speedWalk;

    }

    private float GetJumpSpeed()
    {

        if (this.manaPoints > 7)
        {

            SetJumpSpeed(6);
            this.manaPoints -= 30;
            return this.jumpSpeed;

        }
        else
        {

            SetJumpSpeed(5);
            return this.jumpSpeed;

        }

    }

    private void SetJumpSpeed(float speedJump)
    {

        this.jumpSpeed = speedJump;

    }

    //Get y Set de variables de control
    private bool GetIsRunning()
    {

        return this.isRunning;

    }

    private void SetIsRunning(bool changeRun)
    {

        this.isRunning = changeRun;

    }

    private bool GetIsWalk()
    {

        return this.isWalk;

    }

    private void SetIsWalk(bool changeWalk)
    {

        this.isWalk = changeWalk;

    }

    private bool GetIsStop()
    {

        return this.isStop;

    }

    private void SetIsStop(bool stopped)
    {

        this.isStop = stopped;

    }

    private bool GetIsJump()
    {

        return this.isJump;

    }

    private void SetIsJump(bool getUp)
    {

        this.isJump = getUp;

    }

    private bool GetStartJump()
    {

        Vector3 vectorAddition = new Vector3(0.16f, 0f, 0f);

        if (Physics2D.Raycast(this.transform.position + vectorAddition, Vector2.down, 0.4f, groundLayer) ||
            Physics2D.Raycast(this.transform.position - vectorAddition, Vector2.down, 0.4f, groundLayer) ||
            Physics2D.Raycast(this.transform.position                 , Vector2.down, 0.3f, groundLayer)  )
        {

            return true;

        }
        else
        {

            return false;

        }

    }


    //Get y Set de contadores
    private bool GetMirrorSprite()
    {

        return this.mirrorSprite;

    }

    private void SetMirrorSprite(bool mirror)
    {

        this.mirrorSprite = mirror;

    }

    private float GetTimeRunning()
    {

        return this.timeRunning;

    }

    private void SetTimeRunning(float time)
    {

        this.timeRunning = time;

    }


    private float GetTimeController()
    {

        return this.timeStop;

    }

    private void SetTimeController(int timeCount)
    {

        this.timeStop = timeCount;

    }

    //rigidboy.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse); Fuerza = masa * aceleracion, esto es un impulso vertical
    //Input.GetMouseButtonDown(0) es el boton del raton de la izquierda
    //Input.GetButtonDown("nombre) Para juegos multiplataforma donde hay juegos que no definen las teclas Project Setting/Input
    //Axes cambiamos el numero


    //Diferencia de la distancia desde que apareces hasta que se finaliza
    public float GetDistance()
    {
        //Hago el teorema de pitagoras al hallar la distancia y obtener el modulo
        float travelledDistance = Vector2.Distance(new Vector2(startPosition.x,0), new Vector2(this.transform.position.x,0));
        return travelledDistance;

    }

    public void CollectHealth(int points)
    {

        if (this.healthPoints + points > 100)
        {

            this.healthPoints = 100;

        }
        else
        {

            this.healthPoints += points;

        }

    }

    public void CollectMana(int points)
    {

        

        if (this.manaPoints + points > 100)
        {

            this.manaPoints = 100;

        }
        else
        {

            this.manaPoints += points;

        }

    }

    public int GetHealth()
    {

        return this.healthPoints;

    }

    public int GetMana()
    {

        return this.manaPoints;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy")
        {

            this.healthPoints -= 10;

        }

    }

}