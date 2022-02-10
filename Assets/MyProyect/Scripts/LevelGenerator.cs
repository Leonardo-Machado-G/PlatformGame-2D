using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //Impedir instanciarlo
    public static LevelGenerator sharedInstance;

    //Todos los prefabs de tipos de bloques en una lista de 0 hasta x
    public List<LevelBlock> allLevelBlocks = new List<LevelBlock>();

    //Asignar un StartPoint
    public Transform levelStartPoint;

    //Lista de bloques que ha creado
    public List<LevelBlock> currentBlocks = new List<LevelBlock>();

    public void Awake()
    {

        sharedInstance = this; // = new LevelGenerator();

    }

    private void Start()
    {

        GenerateInitialBlocks();

    }

    public void AddLevelBlock()
    {

        //genera un valor aleatorio entre a y b entero
        int randomIndex = Random.Range(0, allLevelBlocks.Count);

        //Instanciamos la posicion aleatoria, copia de la carpeta de mis prefabs el bloque y lo mete en esta variable, lo instancia
        //Nos devuelve un GameObject y lo transformamos en un LevelBlock
        LevelBlock currentBlock = (LevelBlock)Instantiate(allLevelBlocks[randomIndex]);

        //Pone el bloque generado como hijo del LevelGenerator que es esta clase
        currentBlock.transform.SetParent(this.transform, false);

        //Hay un problema con la aparicion de los bloques porque Unity coge el centro del objeto el punto seleccionado
        Vector3 spawnPosition = Vector3.zero;

        if (currentBlocks.Count == 0)
        {
            //La aparicion del primer bloque sucede en el Startpoint
            spawnPosition = levelStartPoint.position;

        }
        else
        {
            //Si hay más de un bloque instanciado el siguiente lo debes colocar en la ultima posicion instanciada
            spawnPosition = currentBlocks[currentBlocks.Count - 1].exitPoint.position;

        }
        //Vector para corregir el origen
        Vector3 correction = new Vector3(spawnPosition.x - currentBlock.startPoint.position.x , 
                                         spawnPosition.y - currentBlock.startPoint.position.y, 0);

        //Asigna al bloque la correccion hecha y lo añade a la lista
        currentBlock.transform.position = correction;
        currentBlocks.Add(currentBlock);

    }

    //Eliminamos el ultimo bloque
    public void RemoveOldLevelBlock()
    {

        LevelBlock oldestBlock = currentBlocks[0];

        //No escribimos solo oldestBlock porque destruyes su script
        currentBlocks.Remove(oldestBlock);
        Destroy(oldestBlock.gameObject);

    }

    //Elimina todos los ultimos bloques todo el rato
    public void RemoveAllBlocks()
    {

        while(currentBlocks.Count > 0)
        {

            RemoveOldLevelBlock();

        }

    }

    public void GenerateInitialBlocks()
    {

        for (int i = 0; i < 2; i++)
        {

            AddLevelBlock();

        }
    }

}
