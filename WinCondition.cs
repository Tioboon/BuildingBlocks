using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    //If the buildings container of some color becames not visible in screen, than the color player loses.
    private void OnBecameInvisible()
    {
        if(transform.parent.name == "B")
        {
            _gameController.bluePlayerLose = true;
        }
        else  if(transform.parent.name == "R")
        {
            _gameController.redPlayerLose = true;
        }
        else  if(transform.parent.name == "G")
        {
            _gameController.greenPlayerLose = true;
        }
    }
}
