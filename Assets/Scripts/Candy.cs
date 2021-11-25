using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private static Color selectedCOlor = new Color(0.5f,0.5f,0.5f,1.0f); // A static variable for color to be shared by all candies
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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
