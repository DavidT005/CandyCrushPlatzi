using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private static Color selectedColor = new Color(0.5f,0.5f,0.5f,1.0f); // A static variable for color to be shared by all candies
    private static Candy previousSelected = null;
    
    private SpriteRenderer spriteRenderer;  //Image for the candy
    private bool isSelected = false; // Is candy selected?
    
    public int id; //Identifier for each individual candy

    private Vector2[] adjacentDirections = new Vector2[]{   //An ennumrate vector array for the directions of candy checking
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
    };


    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>(); //Initializes sprite renderer
        
    }

    private void SelectCandy()  //Method to call when candy touched
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;   //changes the color of Candy to color on select
        previousSelected = gameObject.GetComponent<Candy>();  //Saves the info of the selected candy
    }

    private void DeselectCandy()
    {
        isSelected = false;
        spriteRenderer.color = Color.white; //Resets tint to white
        previousSelected = null;  //Removes selected candy info

    }

    void OnMouseDown()  //Selects or deselects candy depending on the situation
    {
        if ( (spriteRenderer.sprite == null) || //Checks if sprite is null, this happens when previous selected candy was delted
        BoardManager.sharedInstance.isShifting) //... or if candies are switching
        {
            return; // Does nothing on these two conditions
        }

        if (isSelected)
        {
            DeselectCandy();    //Deselects candy if selected candy touched 
        }
        else
        {
            if(previousSelected == null)
            {
                SelectCandy();  //Selects candy if there was no slected candy before
            }
            else
            {
                if (CanSwipe()) //Runs if candies are switchable
                {
                    SwapSprite(previousSelected);   //Switched the candies sprites and ids
                    previousSelected.DeselectCandy();   //Deselects previous candy

                }
                else    //Runs if candies are not switchable
                {
                    previousSelected.DeselectCandy();   //Deselects previous candy
                    SelectCandy();  //selects new candy
                }


            }
        }

    }


    public void SwapSprite(Candy newCandy)  //Swaps the sprites of two candies
    {
        if(spriteRenderer.sprite == newCandy.GetComponent<SpriteRenderer>().sprite)
        {
            return; //Does nothing if candies are the same type
        }

        Sprite oldCandy = newCandy.spriteRenderer.sprite;   //We store the sprite for second candy on auxiliar variable

        newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;    //The second candy gets the sprite of the first candy

        this.spriteRenderer.sprite = oldCandy;  //Stores the sprite for second candy in first candy


        // We do something very similar with the ids
        int tempId = newCandy.id;
        newCandy.id = this.id;
        this.id = tempId;

    }


    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,   //We create a Ray and store the info for collission on "hit"
        direction); //The raycast goes in this direction

        if(hit.collider != null)
        {
            return hit.collider.gameObject; //Returns the game object hit in case raycast hits something
        }
        else
        {
            return null;    //returns null if raycast hit is null
        }
    }

    private List<GameObject> GetAllNeighbors()  // A method to check cady tipe on four directions
    {
        List<GameObject> neighbors = new List<GameObject>();    //We initialize an empty list of game objects

        foreach(Vector2 direction in adjacentDirections)    // Iterates over every game direction on the directions list
        {
            neighbors.Add(GetNeighbor(direction));  //Adds the game object returned bu GetNeighbor on all directions
        }

        return neighbors;   //Returns the list for all objects found

    }

    private bool CanSwipe() //A method to check if two candies are interchangable
    {
        return GetAllNeighbors().Contains(previousSelected.gameObject); //Returns true if the previous selected candy is neighbor
    }



}
