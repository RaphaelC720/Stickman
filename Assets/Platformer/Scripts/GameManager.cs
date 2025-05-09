using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //I can use a static variable to make this available
      //from anywhere in my code
    public static GameManager Me;

    //A link to the player
    public PlayerScript Player;
    //Keep a list of all the monsters
    
    void Start()
    {
        //Register myself to the static variable
        GameManager.Me = this;
    }

    void Update()
    {
       
    }
}
