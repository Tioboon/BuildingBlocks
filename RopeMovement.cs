using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RopeMovement : MonoBehaviour
{
    //Rotation force
    public float forceOfRotation;

    //Child counter
    //Seted to three because i use the rest of division by three to define the color
    public int numbOfChilds = 3;

    //Separated child count
    public int numbOfChildsRed;
    public int numbOfChildsBlue;
    public int numbOfChildsGreen;

    //Script of each floor by color
    public FloorScript rFloor;
    public FloorScript gFloor;
    public FloorScript bFloor;

    //Script from camera;
    private CameraScript _cameraScript;

    //The last child to appear
    private int largestChild;

    //Initial position
    private Vector3 initPos;
    
    private GameController _gameController;
    
    void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        initPos = transform.position;
        _cameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        InvokeRepeating("changeRopeSide", 1, 2);
        InstantiateBuilding();
    }

    public void InstantiateBuilding()
    {
        //If is the last rope...
        if(name == "RopeBot")
        {
            //...Instantiate new building block;
            var newPredio = Instantiate(buildingGO, transform.position, quaternion.identity);
            //Rename the build;
            newPredio.name = "Building Nº" + (numbOfChilds-3);
            //Count the child numbers;
            numbOfChilds += 1;
            //Check each floor child numbers;
            rFloor.CheckNumbOfChilds();
            gFloor.CheckNumbOfChilds();
            bFloor.CheckNumbOfChilds();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If is the last rope...
        if(name == "RopeBot")
        {
            //If number of red childs is more than the others...
            if (numbOfChildsRed >= numbOfChildsBlue && numbOfChildsRed >= numbOfChildsGreen)
            {
                //..And is higher than 3...
                if (numbOfChildsRed > 3)
                {
                    //...Set the Y axis height of the camera;
                    _cameraScript.goUpAmount = numbOfChildsRed - 3;
                    //Get the last child number that appears on screen;
                    largestChild = numbOfChildsRed - 3;
                }
            }
            //Same for green
            else if (numbOfChildsGreen >= numbOfChildsBlue && numbOfChildsGreen >= numbOfChildsRed)
            {
                if (numbOfChildsGreen > 3)
                {
                    _cameraScript.goUpAmount = numbOfChildsGreen - 3;
                    largestChild = numbOfChildsGreen - 3;
                }
            }
            //Same for blue
            else if (numbOfChildsBlue >= numbOfChildsRed && numbOfChildsRed >= numbOfChildsGreen)
            {
                if (numbOfChildsBlue > 3)
                {
                    _cameraScript.goUpAmount = numbOfChildsBlue - 3;
                    largestChild = numbOfChildsBlue - 3;
                }
            }
        }
        //If blue child count is less than the two other colors block minus three...
        if(numbOfChildsRed >= numbOfChildsBlue + 3 || numbOfChildsGreen >= numbOfChildsBlue + 3)
        {
            //...Blue player loses;
            _gameController.bluePlayerLose = true;
        }
        //If red child count is less than the two other colors block minus three...
        if(numbOfChildsBlue >= numbOfChildsRed + 3 || numbOfChildsGreen >= numbOfChildsRed + 3)
        {
            //...Red player loses;
            _gameController.redPlayerLose = true;
        }
        //If green child count is less than the two other colors block minus three...
        if(numbOfChildsRed >= numbOfChildsGreen + 3 || numbOfChildsBlue >= numbOfChildsGreen + 3)
        {
            //...Green player loses;
            _gameController.greenPlayerLose = true;
        }
        //Rotate the rope
        transform.eulerAngles += new Vector3(0, 0 ,forceOfRotation);
    }

    //Change the rotation direction of the rope
    private void changeRopeSide()
    {
        forceOfRotation = -forceOfRotation;
    }
}
