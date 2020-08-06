using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PredioComportament : MonoBehaviour
{
    //Last rope transform
    private Transform lastRope;
    //script from last rope to get gameobject infos
    private RopeMovement lastRopeMovement;
    //bool released from rope
    private bool released;
    //Dissecated 2d velocity components
    private float velX;
    private float velY;
    //velocity multiplier
    public float velocity;
    //RigidBody from object
    private Rigidbody2D _rigidbody2D;
    //Angle betwen world down vector and object down vector
    private float angle;
    private bool landed;
    private bool spawnedOtherPredio;

    public float distance;

    private RopeMovement _ropeMovement;

    void Start()
    {
        //set variables
        lastRope = GameObject.Find("RopeTop").transform.Find("RopeMiddle").Find("RopeBot");
        lastRopeMovement = lastRope.GetComponent<RopeMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //set gravity to 0 to the building block don't go down while on the rope
        _rigidbody2D.gravityScale = 0;
        _ropeMovement = lastRope.GetComponent<RopeMovement>();
    }
    
    void Update()
    {
        //Check if already landed on ground
        if(!landed)
        {
            //Check if was realeased from the rope
            //If wasn't...
            if (!released)
            {
                //...Set position to the rope point position;
                transform.position = lastRope.position - lastRope.up * distance;
                //Set the angle to the angle of last rope segment;
                transform.eulerAngles = lastRope.eulerAngles;
            }
            //If was...
            else
            {
                //step 4
                //Check if the rotation from the object is with the vector down alined with world vector down
                //If isn't...
                if (-transform.up != Vector3.down)
                {
                    //...Ajust to gradativily be the same
                    if (-transform.up.x < 0)
                    {
                        transform.eulerAngles += new Vector3(0, 0, angle / 100);
                    }
                    else if (-transform.up.x > 0)
                    {
                        transform.eulerAngles -= new Vector3(0, 0, angle / 100);
                    }
                }
                
                //Aplies movement to the box based on velocity
                transform.position += new Vector3(velX * velocity, velY * velocity, 0);
            }

            //step 2
            //If the keyboard "space" button was pressed...
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //...marks tha was released;
                released = true;
                //Reaply gravity;
                _rigidbody2D.gravityScale = 1;
                //Aplies gravity for each child of the building box
                foreach (Transform child in transform)
                {
                    var rbChild = child.GetComponent<Rigidbody2D>();
                    if (rbChild != null)
                    {
                        rbChild.gravityScale = 1;
                    }
                }

                //step 3
                //Check rope position
                //if is right...
                if (lastRope.eulerAngles.z > 0)
                {
                    //...Check the direction of rope movement
                    //Going right...
                    if (lastRopeMovement.forceOfRotation > 0)
                    {
                        //...calculates sin and cos from angle where sin is Y velocity and cos X velocity;
                        velX = Mathf.Cos(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                        velY = Mathf.Sin(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                    }
                    //Going left...
                    else if (lastRopeMovement.forceOfRotation < 0)
                    {
                        //...calculates sin and cos from angle where sin is Y velocity and cos X velocity;
                        //Inverts the value because the value of "forceOfRotation" is negative and represents the side that rope goes
                        velX = -Mathf.Cos(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                        velY = -Mathf.Sin(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                    }
                }
                //Check rope position
                //if is left...
                if (lastRope.eulerAngles.z < 0)
                {
                    //...Check the direction of rope movement
                    //Going right...
                    if (lastRopeMovement.forceOfRotation > 0)
                    {
                        //...calculates sin and cos from angle where sin is Y velocity and cos X velocity;
                        velX = -Mathf.Cos(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                        velY = -Mathf.Sin(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                    }

                    //Going left...
                    if (lastRopeMovement.forceOfRotation < 0)
                    {
                        //...calculates sin and cos from angle where sin is Y velocity and cos X velocity;
                        //Inverts the value because the value of "forceOfRotation" is negative and represents the side that rope goes
                        velX = Mathf.Cos(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                        velY = Mathf.Sin(lastRope.transform.eulerAngles.z * Mathf.PI / 180);
                    }
                }

                
                //Check wich side the rope is going to invert velocity in X case is going for the opposite direction that was to be
                if (lastRopeMovement.forceOfRotation > 0)
                {
                    if (velX < 0)
                    {
                        velX = -velX;
                    }
                }
                else if (lastRopeMovement.forceOfRotation < 0)
                {
                    if (velX > 0)
                    {
                        velX = -velX;
                    }
                }

                angle = Vector3.Angle(-transform.up, Vector3.down);
            }
        }
        //If isn't landed...
        else
        {
            //...If have a parent
            if(transform.parent != null)
            {
                //...Set the rotation to the parent rotation.
                transform.rotation = transform.parent.rotation;
            }
        }
    }
    
    //Checks when collide occurs
    private void OnCollisionEnter2D(Collision2D other)
    {
        //If other building wasn't spawned
        if(!spawnedOtherPredio)
        {
            //Spawn a build
            _ropeMovement.InstantiateBuilding();
            //Set true the bool that checks if the building is spawned
            spawnedOtherPredio = true;
        }
        //decreases velocity and invert in Y axis
        velX /= 2; 
        velY = -velY / 2;
        landed = true;
        spawnedOtherPredio = true;
    
    }
    
    //Check when collision is maintened
    private void OnCollisionStay2D(Collision2D other)
    {
        //Inverts velocity in Y axis and halfs it.
        velX = -velX / 2;
    }
}
