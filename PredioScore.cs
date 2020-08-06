using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//Controls the building block comportament in collision
public class PredioScore : MonoBehaviour
{
    //Canvas and score
    private TextMeshProUGUI _scoreText;
    public Transform canvas;
    //Render
    private SpriteRenderer _spriteRenderer;
    //Floor that the building block most go
    public String _goToThisFloor;
    //bool if collided
    private bool collidedWithOtherBuilding;
    //bool if canvas is active
    private bool canvasDeactivated;
    //bool the building box gameobject
    public GameObject buildGO;
    //position in instant of collision
    private Vector3 fixedPos;
    //Rope script
    private RopeMovement _ropeMovement;
    //Build counter
    public int buildingNumber;
    //bool if collided with floor
    private bool collidedWithGround;
    //GameController script
    private GameController _gameController;
    
    
    //Score color
    private float r;
    private float g;
    private float b;
    private bool rGoingDown;
    private bool gGoingDown;
    private bool bGoingDown;
    

    void Start()
    {
        //Set variables
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        canvas = transform.Find("Canvas");
        _scoreText = canvas.Find("Text").GetComponent<TextMeshProUGUI>();
        _spriteRenderer = transform.Find("Square").GetComponent<SpriteRenderer>();
        _ropeMovement = GameObject.Find("RopeTop").transform.Find("RopeMiddle").Find("RopeBot").GetComponent<RopeMovement>();;
        //Set the color of the build in order Blue -> Red -> Green -> Blue -> Red, and goes on...
        var ran = _ropeMovement.numbOfChilds % 3;
        switch(ran){
            //Case variable that defines color is 0 (blue)
            case 0:
                //If blue already lose...
                if (_gameController.bluePlayerLose)
                {
                    //...Instantiate another building and destroy this.
                    _ropeMovement.InstantiateBuilding();
                    Destroy(gameObject);
                }
                //If is blue is on game...
                //Set the color to blue;
                _spriteRenderer.color = new Color(0.14f, 0.33f, 0.80f);
                //Set wich floor this block can attach;
                _goToThisFloor = "B";
                //Set the number of the build;
                buildingNumber = _ropeMovement.numbOfChilds -4;
                return;
            //than do the same for red and green
            case 1:
                if (_gameController.redPlayerLose)
                {
                    _ropeMovement.InstantiateBuilding();
                    Destroy(gameObject);
                }
                _spriteRenderer.color = new Color(0.80f, 0.14f, 0.30f);
                _goToThisFloor = "R";
                buildingNumber = _ropeMovement.numbOfChilds -4;
                return;
            case 2:
                if (_gameController.greenPlayerLose)
                {
                    _ropeMovement.InstantiateBuilding();
                    Destroy(gameObject);
                }
                _spriteRenderer.color = new Color(0.33f, 0.60f, 0.14f);
                _goToThisFloor = "G";
                buildingNumber = _ropeMovement.numbOfChilds -4;
                return;
        }
    }

    private void Update()
    {
        //If Building box collide with other or with the floor...
        if (collidedWithOtherBuilding || collidedWithGround)
        {
            //...Set the position to the position in the momento of collision;
            transform.position = fixedPos;
            //...Fix the scale to not distorce the object when parenting
            transform.localScale = new Vector3(1,1,1);
        }
        //If the floor that this build can attach is RED...
        if (_goToThisFloor == "R")
        {
            ///...Score color set to be majoritary red and variating g & b values.
            _scoreText.color = new Color(0.8f, g, b);
            //variate the Blue value
            if (!bGoingDown)
            {
                b += 0.01f;
                if (b >= 0.8f)
                {
                    bGoingDown = true;
                }
            }
            else
            {
                b -= 0.01f;
                if (b <= 0)
                {
                    bGoingDown = false;
                }
            }
            //variate the Green value
            if (!gGoingDown)
            {
                g += 0.005f;
                if (g >= 0.4)
                {
                    gGoingDown = true;
                }
            }
            else
            {
                g -= 0.005f;
                if (g <= 0)
                {
                    gGoingDown = false;
                }
            }
        }
        //Than do this to the other buildings based on floor compatibility (blue and green)
        else if (_goToThisFloor == "B")
        {
            _scoreText.color = new Color(r, g, 0.8f);
            if (!rGoingDown)
            {
                r += 0.01f;
                if (r >= 0.8f)
                {
                    rGoingDown = true;
                }
            }
            else
            {
                r -= 0.01f;
                if (r <= 0)
                {
                    rGoingDown = false;
                }
            }
            if (!gGoingDown)
            {
                g += 0.005f;
                if (g >= 1)
                {
                    gGoingDown = true;
                }
            }
            else
            {
                g -= 0.005f;
                if (g <= 0)
                {
                    gGoingDown = false;
                }
            }
        }
        else if (_goToThisFloor == "G")
        {
            _scoreText.color = new Color(r, 0.8f, b);
            if (!rGoingDown)
            {
                r += 0.005f;
                if (r >= 0.4f)
                {
                    rGoingDown = true;
                }
            }
            else
            {
                r -= 0.005f;
                if (r <= 0)
                {
                    rGoingDown = false;
                }
            }
            if (!bGoingDown)
            {
                b += 0.01f;
                if (b >= 0.8f)
                {
                    bGoingDown = true;
                }
            }
            else
            {
                b -= 0.01f;
                if (b <= 0)
                {
                    bGoingDown = false;
                }
            }
        }
    }

    //If the building block collides...
    void OnCollisionEnter2D(Collision2D other)
    {
        //...Get the script from the other building;
        PredioScore otherPredioScore = other.transform.GetComponent<PredioScore>();
        //If the script exists... (If collided with another building).
        if (otherPredioScore != null)
        {
            //...If not already collided...
            if(!collidedWithOtherBuilding && !collidedWithGround)
            {
                //...If the other building has the same color...
                if (otherPredioScore._spriteRenderer.color == _spriteRenderer.color)
                {
                    //Set this to has the same parent from other building;
                    transform.parent = otherPredioScore.transform.parent;
                    //Fix the scale to not distorce with base in the parent;
                    transform.localScale = new Vector3(1,1,1);
                    //Stores the colision position;
                    fixedPos = transform.position;
                    //Stores the number of buildings with the same color that are togheter;
                    int score = transform.parent.parent.GetComponent<FloorScript>().CheckNumbOfChilds();
                    //Set the score canvas text to the score variable number;
                    _scoreText.text = score.ToString();
                    //Count the number of childs of each color in the rope script;
                    if (_goToThisFloor == "B")
                    {
                        _ropeMovement.numbOfChildsBlue += 1;
                    }
                    else if (_goToThisFloor == "R")
                    {
                        _ropeMovement.numbOfChildsRed += 1;
                    }
                    else if (_goToThisFloor == "G")
                    {
                        _ropeMovement.numbOfChildsGreen += 1;
                    }
                }
                //If the color is different...
                else
                {
                    //Destroy the building block in question
                    Destroy(gameObject);
                }
                //Set that collided with other building
                collidedWithOtherBuilding = true;
            }
        }

        //If collided with a floor...
        FloorScript otherFloor = other.transform.GetComponent<FloorScript>();
        if(otherFloor != null)
        {
            //...If wasn't collided before...
            if(!collidedWithGround && !collidedWithOtherBuilding)
            {
                //...If number of blue buildings is more than 0 and this build has to go to the blue floor...
                if (_ropeMovement.numbOfChildsBlue > 0 && _goToThisFloor == "B")
                {
                    //...Destroy the gameobject;
                    Destroy(gameObject);
                    //Cancel the realocation of score panel;
                    otherFloor.CheckNumbOfChilds();
                }
                //Same for red
                if (_ropeMovement.numbOfChildsRed > 0 && _goToThisFloor == "R")
                {
                    Destroy(gameObject);
                    otherFloor.CheckNumbOfChilds();
                }
                //Same for green
                if (_ropeMovement.numbOfChildsGreen > 0 && _goToThisFloor == "G")
                {
                    Destroy(gameObject);
                    otherFloor.CheckNumbOfChilds();
                }
                //If the floor that the build most go is the floor that he collided...
                if (_goToThisFloor == otherFloor.name)
                {
                    //...Check if the floor have any buildings already
                    if (_goToThisFloor == "B" && _ropeMovement.numbOfChildsBlue <= 0)
                    {
                        //...If this is the first building, add to the count of respective color.
                        _ropeMovement.numbOfChildsBlue += 1;
                    }
                    else if (_goToThisFloor == "R" && _ropeMovement.numbOfChildsRed <= 0)
                    {
                        //...If this is the first building, add to the count of respective color.
                        _ropeMovement.numbOfChildsRed += 1;
                    }
                    else if (_goToThisFloor == "G" && _ropeMovement.numbOfChildsGreen <= 0)
                    {
                        //...If this is the first building, add to the count of respective color.
                        _ropeMovement.numbOfChildsGreen += 1;
                    }
                    //Set the parent to the buildings container;
                    transform.parent = otherFloor.buildingsContainer;
                    //Fix the scale of the object; 
                    transform.localScale = new Vector3(1,1,1);
                    //Set the score number;
                    int score = other.transform.GetComponent<FloorScript>().CheckNumbOfChilds();
                    //Set the score text;
                    _scoreText.text = score.ToString();
                    //Set the scale object to the scale of original object;
                    transform.localScale = buildGO.transform.localScale;
                    //Stores the position of the collision;
                    fixedPos = transform.position;
                }
                //If collided in a floor with the wrong collor...
                else
                {
                    //...Destroy object and recount the childs;
                    Destroy(gameObject);
                    otherFloor.CheckNumbOfChilds();
                }

                //Set the collision bool to true;
                collidedWithGround = true;
            }
        }
    }
}
