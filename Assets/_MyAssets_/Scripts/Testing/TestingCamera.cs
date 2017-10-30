using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCamera : MonoBehaviour {

    public float currRot = 0f;              
    public float antiRot = 0f;              
    public float camSpeed = 0.5f;           
    public float rotSpeed = .5f;            
    public float zoomSpeed = .3f;           
    public float currHeight;               
    public float minHeight = 3f;            
    public float maxHeight = 8f;

    public bool keys;
    public bool testMove;

    public string tags;

    public Vector3 testDirection;
    public Vector3 testHeight;

    RaycastHit hit;

    // Use this for initialization
    void Start () {
        currHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        testHeight = new Vector3(transform.position.x, currHeight, transform.position.z);

        Refresh();
	}

    public void Refresh()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currHeight -= zoomSpeed;
            currHeight = Mathf.Clamp(currHeight, minHeight, maxHeight);
            transform.position = testHeight;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currHeight += zoomSpeed;
            currHeight = Mathf.Clamp(currHeight, minHeight, maxHeight);
            transform.position = testHeight;
        }

        if (testMove)
        {
            transform.position += testDirection;
        }

        if (Input.GetButtonDown("Up"))
        {
            StartMovingForward(keys);
        }
        else if (Input.GetButtonUp("Up"))
        {
            StopMoving();
        }
        else if (Input.GetButtonDown("Down"))
        {
            keys = true;
            StartMovingForward(keys);
        }
        else if (Input.GetButtonUp("Down"))
        {
            keys = false;
            StopMoving();
        }
        else if (Input.GetButtonDown("Right"))
        {
            StartMovingSides(keys);
        }
        else if (Input.GetButtonUp("Right"))
        {
            StopMoving();
        }
        else if (Input.GetButtonDown("Left"))
        {
            keys = true;
            StartMovingSides(keys);
        }
        else if (Input.GetButtonUp("Left"))
        {
            keys = false;
            StopMoving();
        }
    }

    public void StartMovingForward(bool opposite)
    {
        testMove = true;

        testDirection = (!opposite) ? transform.forward : -transform.forward;
        testDirection *= camSpeed;
    }

    public void StartMovingSides(bool opposite)
    {
        testMove = true;

        testDirection = (!opposite) ? transform.right : -transform.right;
        testDirection *= camSpeed;
    }

    public void StopMoving()
    {
        testMove = false;
    }

}
