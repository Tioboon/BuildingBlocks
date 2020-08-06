using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Controls the win condition of the game.
    public GameObject winGO;
    public GameObject outGo;
    
    public bool bluePlayerLose;
    public bool greenPlayerLose;
    public bool redPlayerLose;
    
    private bool winSpawned;

    private void Update()
    {
        //resets the scene
        //If "r" keyboard button is pressed...
        if (Input.GetKeyDown(KeyCode.R))
        {
            //...Load Scene
            SceneManager.LoadScene("SampleScene");
        }
        //If red player wins...
        if (bluePlayerLose && greenPlayerLose)
        {
            //...If the win panel wasn't spawned before...
            if(!winSpawned)
            {
                //...Spawn the panel;
                var newWin = Instantiate(winGO, transform);
                //Set text to "color + wins"
                newWin.GetComponent<TextMeshProUGUI>().text = "Red Wins";
                //Set the text color to the color of buildings
                newWin.GetComponent<TextMeshProUGUI>().color = new Color(0.81f, 0.28f, 0.67f);
                //Than set the bools that controls if the panel was spawned to true
                winSpawned = true;
            }
        }
        //Do the same if green wins
        else if (greenPlayerLose && redPlayerLose)
        {
            if(!winSpawned)
            {
                var newWin = Instantiate(winGO, transform);
                newWin.GetComponent<TextMeshProUGUI>().text = "Blue Wins";
                newWin.GetComponent<TextMeshProUGUI>().color = new Color(0.28f, 0.67f, 0.81f);
                winSpawned = true;
            }
        }
        //And if blue wins
        else if (bluePlayerLose && redPlayerLose)
        {
            if(!winSpawned)
            {
                var newWin = Instantiate(winGO, transform);
                newWin.GetComponent<TextMeshProUGUI>().text = "Green Wins";
                newWin.GetComponent<TextMeshProUGUI>().color = new Color(0.67f, 0.81f, 0.28f);
                winSpawned = true;
            }
        }
        
    }
}
