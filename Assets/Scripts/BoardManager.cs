using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager sharedInstance;  //Set this script as singleton so no other like this can be called
    public List<Sprite> prefabs = new List<Sprite>();    //A list for sprites, not technically prefabs, to store graphics
    public GameObject currentCandy; //the candy selected
    public int xSize, ySize;    // the size for the board
    
    private GameObject[,] candies; //A gameobject matrix for storing all candies on board

    public bool isShifting { get; set; } //|COMMENT|player is shifting candies?, getter and setter, so this class is only with RW permissions



    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)    //checks if a BoardManager script is alredy running
        {
            sharedInstance = this;  //if there is no boardManager script running, this one becomes the shared Instance
        } 
        else
        {
            Destroy(gameObject);    // If there's a BoardManager already, this one gets destroyed
        }

        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;   //Stores the size for each candy in offset
        CreateInitialBoard(offset); // Calls the method to create board on start

    }


    private void CreateInitialBoard(Vector2 offset){    //Method to setup board, offset is the candies' size
        candies = candies = new GameObject[xSize,ySize]; //We create a matrix with xSize columns

        float startX = this.transform.position.x; //The x coordinate to start creation of candies
        float startY = this.transform.position.y; //The y coordinate to start creation of candies

        // This is a double loop to instantiate a candy per cell
        for (int x = 0; x < xSize; x++){    //Iterates over all columns
            for (int y = 0; y < ySize; y++){    //Iterates over all rows
                GameObject newCandy = Instantiate(  //We instantiate a prefab using instantiate method...
                currentCandy,   //Which candy will be put in
                new Vector3(   
                    startX + (offset.x*x), //...on the x_i position...
                    startY + (offset.y*y), //... and the y_j position
                    0),   //No depth
                    currentCandy.transform.rotation); // With the same rotation as original

                newCandy.name = string.Format("Candy[{0}][{1}]",x,y);  //We name the current candy "candyXY", eg candy[0][0] for 1st
                candies[x,y] = newCandy;    //we store the new candy on the candy matrix
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
