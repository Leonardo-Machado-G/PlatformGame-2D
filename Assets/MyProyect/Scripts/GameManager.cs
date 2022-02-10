using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Posibles estados del videjuego
public enum GameState
{

    menu, inGame, gameOver

}

public class GameManager : MonoBehaviour
{

    //Variable querefeencia al propio Game Manager
    public static GameManager sharedInstance;
    public Canvas menuCanvas, gameCanvas, gameOverCanvas;


    //Variable para saber en que estado del juego nos encontramos
    //Al inicio queremos que empiece en el menu principal
    public GameState currentGameState = GameState.menu;

    //Variable cuenta items
    public int collectedItems = 0;

    //public GameObject gameUI;
    private void Awake()
    {

        sharedInstance = this;

    }

    private void Start()
    {

        BackToMenu();

    }
    
    //Metodo del inicio del juego
    private void Update()
    {

        if (Input.GetButtonDown("Pausa"))
        {
           
            BackToMenu();

        }

        if (Input.GetButtonDown("Start") && this.currentGameState != GameState.inGame)
        {

            StartGame();

        }

        #if UNITY_EDITOR
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {

                ExitGame();

            }

        }
        #endif

    }

    public void StartGame()
    {

        //El juego pasa a estar en modo en juego
        SetGameState(GameState.inGame);
        
      
        //Remueve todos los bloques y genera uno solo si el jugador se ha desplazado del origen
        //Lo hace solo al haber pasado 5 coodenadas en x
        if(Script_Louis2D.sharedInstance.transform.position.x > 5f)
        {

            LevelGenerator.sharedInstance.RemoveAllBlocks();
            LevelGenerator.sharedInstance.AddLevelBlock();

        }

        //Llama al script de Louis y ejecuta su metodo comenzar el juego es necesario que este despues del bloque if de arriba
        Script_Louis2D.sharedInstance.StartGame();

        //Asignarle a camera el objeto con el tag main camera
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        //Hacer que camarafollow obtenga el componente de camera supongo que sera su tag
        CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();

        //Resetea mediante un metodo la posicion de camera para que este al inicio
        cameraFollow.ResetCameraPosition();

        //Reduce a cero el numero de monedas
        this.collectedItems = 0;

    }

    //Metodo que se llamara cuando el jugador muera
    public void GameOver()
    {

        SetGameState(GameState.gameOver);

    }

    //Metodo para volver al menu principal cuando el usuario lo que quiera hacer
    public void BackToMenu()
    {

        SetGameState(GameState.menu);

    }

    //Solo funciona una vez estamos dentro del juego
    public void ExitGame()
    {

        //Application.Quit();
        //Solo se puede usar en el unity editor
        #if UNITY_EDITOR
        {

            UnityEditor.EditorApplication.isPlaying = false;

        }
        #else
        {

            Application.Quit();

        }
        #endif

    }

    //Metodo encargado de cambiar el estado del juego
    void SetGameState(GameState newGameState)
    {

        if (newGameState == GameState.menu)
        {

            //Preparamos la escena en Unity para mostrar el menu

            menuCanvas.enabled = true;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            //gameUI.active = true;

        }
        else if (newGameState == GameState.inGame)
        {

            //Preparamos la escena en Unity para jugar

            menuCanvas.enabled = false;
            gameCanvas.enabled = true;
            gameOverCanvas.enabled = false;
            //gameUI.active = false;

        }
        else if (newGameState == GameState.gameOver)
        {

            //Preparamos la escena en Unity para el Game Over

            menuCanvas.enabled = false;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = true;
            //gameUI.SetActive(false);
        }

        //Asignamos el estao de juego actual al que nos ha llegado por parametro

        this.currentGameState = newGameState;
        
    }

    public void CollectItem(int itemValue)
    {

        this.collectedItems += itemValue;

    }

}
