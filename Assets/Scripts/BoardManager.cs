using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager sharedInstance;  //Set this script as singleton so no other like this can be called
    public list<Sprite> prefabs = new list<Sprite>();    //A list for sprites, not technically prefabs, to store graphics
    public GameObject currentCandy; //the candy selected
    public int xSize, ySize;    // the size for the board
    
    private GameObject[,] candies; //A gameobject matrix for storing all candies on board

    public bool isShifting = {get,set}; //player is shifting candies?, getter and setter, so this class is only with RW permissions



    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null){    //checks if a BoardManager script is alredy running
            sharedInstance = this;  //if there is no boardManager script running, this one becomes the shared Instance
        } else{
            Destroy(gameObject);    // If there's a BoardManager already, this one gets destroyed
        }

        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;   //Stores the size for each candy in offset


    }

    private void createInitialBoard(Vector2 offset){    //Method to setup board, offset is the candies' size
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
