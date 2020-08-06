using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make the count of how many blocks are stacked in each color, than shows it in a score.
public class FloorScript : MonoBehaviour
{
    public Transform buildingsContainer;
    private int scoreShower;
    private GameController _gameController;

    private void Start()
    {
        //Stores the gamecontroller object
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public int CheckNumbOfChilds()
    {
        //Variable that count childs
        int ChildCounter = 0;
        //For each child in the container that stores buildings block...
        foreach (Transform child in buildingsContainer)
        {
            //...Add one to the counter
            ChildCounter += 1;
            PredioScore predioScore = child.GetComponent<PredioScore>();
            //If the number of the building block is higher than the score showed...
            if(predioScore.buildingNumber > scoreShower)
            {
                //...Show in score the number of the building block
                scoreShower = predioScore.buildingNumber;
            }
        }

        foreach (Transform child in buildingsContainer)
        {
            PredioScore predioScore = child.GetComponent<PredioScore>();
            //If the building block has the number equals to the score...
            if (child.name == "Building Nº" + scoreShower)
            {
                //...Show the score on top of building
                predioScore.canvas.gameObject.SetActive(true);
            }
            //If not...
            else
            {
                //...Hide the score
                predioScore.canvas.gameObject.SetActive(false);
            }
        }
        //reset scoreShower to not stack in the next call of the funcition
        scoreShower = 0;
        //Return the numbers of building blocks that are togheter
        return ChildCounter;
    }
}
