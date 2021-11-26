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

    public bool isShifting { get; set; } //|EXPLAIN|player is shifting candies?, getter and setter, so this class is only with RW permissions

    private Candy selectedCandy;    // A variable to store which is the currently selected candy

    public const int MinCandiesToMatch = 2; // the minumun number neighboring candies needed to match

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


    private void CreateInitialBoard(Vector2 offset){    //Method to setup board, offset is the candies' size. starts on bottom left
        candies = candies = new GameObject[xSize,ySize]; //We create a matrix with xSize columns

        float startX = this.transform.position.x; //The x coordinate to start creation of candies
        float startY = this.transform.position.y; //The y coordinate to start creation of candies





        int idx = -1;   //A dummy value for the ID of candies



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
                
                // This do while checks no candies of the same type are next to each other...
                do
                {
                    idx = Random.Range(0, prefabs.Count);   //...by creating new ID...
                }while( (x > 0 && idx == candies[x-1,y].GetComponent<Candy>().id )||  //..checking it's different than one on left...
                (y>0 && idx == candies[x,y-1].GetComponent<Candy>().id)); //... and one below
                // If one of these two conditions is met, then a new idx is created until no match

                
                Sprite sprite = prefabs[idx]; //Saves a random sprite from the sprite array
                newCandy.GetComponent<SpriteRenderer>().sprite = sprite;    //Uses the randomly selected sprite for the new candy
                newCandy.GetComponent<Candy>().id = idx; //Stores the ID for the specific candy
                
                
                newCandy.transform.parent = transform;  //We set the new candy as a child of the BoardManager game object
                candies[x,y] = newCandy;    //we store the new candy on the candy matrix

            }
        }

    }   


    public IEnumerator FindNullCandies()    // Method to find holes for candies
    {
        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < xSize; y++)
            {
                if(candies[x,y].GetComponent<SpriteRenderer>().sprite == null)  //Checks if no candy sprite
                {
                    yield return StartCoroutine(MakeCandiesFall(x,y)); //Waits until MakeCandiesFall() stops executing
                    break;
                }
            }
        }

        // We check if there are new matches for all new candies
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                candies[x,y].GetComponent<Candy>().FindAllMatches();
            }
        }

    }

    private IEnumerator MakeCandiesFall(int x, int yStart, float shiftDelay = 0.2f)    // A method to make candies fall
    {
        isShifting = true;  //Sets isShifting to true so dependent processes don't rund in parallel


        List<SpriteRenderer> renderers = new List<SpriteRenderer>();    // A list to save all sprite renderers
        int nullCandies = 0;    // Var to store null qty

        for(int y = yStart; y < ySize; y++) // starts to search null candies from first null candy 
        {
            SpriteRenderer spriteRenderer = candies[x,y].GetComponent<SpriteRenderer>();  //Gets the SpriteRenderer for current position
            if(spriteRenderer.sprite == null) //Checks if there's a sprite
            {
                nullCandies++;  //We add one to the total null candies
            }

            renderers.Add(spriteRenderer);   //Adds the sprite renderer to the list to store it
        }

        for (int i = 0; i < nullCandies; i++)
        {

            GUIManager.sharedInstance.Score +=10;    //Adds 10 pints per destroyed candy

            yield return new WaitForSeconds(shiftDelay);    //Waits for the specified ammount of time for the candy to fall
            for (int j = 0; j < renderers.Count-1; j++) 
            {
                renderers[j].sprite = renderers[j+1].sprite;    //Puts the sprite for candy above on current candy
                renderers[j+1].sprite = GetNewCandy(x, ySize-1);   //Creates a new candy on the top of the board
            }
        }



        isShifting = false;  //Sets isShifting to false again
    }

    private Sprite GetNewCandy(int x, int y)  //Method to create new candies to fillup spaces
    {
        List<Sprite> possibleCandies = new List<Sprite>();  //A list to store all possible candies to create
        possibleCandies.AddRange(prefabs);  // We add the prefabs to the list, we need to add them so it's a copy, not a reference

        if (x>0)
        {
            possibleCandies.Remove(candies[x-1,y].GetComponent<SpriteRenderer>().sprite); //Removes candie next to current one from possible sprites
        }
        
        if (x<xSize - 1)
        {
            possibleCandies.Remove(candies[x+1,y].GetComponent<SpriteRenderer>().sprite); //Removes candie next to current one from possible sprites
        }

        if (y > 0)
        {
            possibleCandies.Remove(candies[x,y-1].GetComponent<SpriteRenderer>().sprite); //Removes candie below current one from possible sprites
        }

        return possibleCandies[Random.Range(0,possibleCandies.Count)];  //Selects the number for the candy sprite from possible vals


    }




}
