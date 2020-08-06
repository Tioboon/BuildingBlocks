using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make the camera go up with the high of buildings
public class CameraScript : MonoBehaviour
{
    //Variable that controls Camera Y position
    internal int goUpAmount;
    private Vector3 initPos;

    private void Start()
    {
        //Stores initial position
        initPos = transform.position;
    }

    private void Update()
    {
        //Set position to the initial position + the deslocation in Y that "goUpAmount" stores
        transform.position = new Vector3(initPos.x, initPos.y + goUpAmount * 1.5f, initPos.z);
    }
}
