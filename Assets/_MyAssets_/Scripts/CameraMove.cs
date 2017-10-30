using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    /**************************************************************************************************
     * This Class handles camera controls and also handles switching between the character camera
     * (charCam) and a free moving camera (freeCam).
     * 
     * currently these height values aren't taking into account the ability for a character to be on top
     * of a building or higher than the maxHeight. These values are currently just for using on a plane
     **************************************************************************************************/

    public GameObject player;               //Holds the currently selected character
    public GameObject playerCam;            //Holds the current players camera

    public float currRot = 0f;              //Camera Holders current Rotation
    public float antiRot = 0f;              //opposite of the Camera Holders Rotation
    public float camSpeed = 0.5f;           //Camera Holders move speed
    public float rotSpeed = .5f;            //Camera Holders rotation speed
    public float zoomSpeed = .3f;           //Camera Holders zoom speed
    public float currHeight;                //Current distance/height of the Camera Holder
    public float minHeightg = 2f;           //The lowest height the Camera Holder can reach when a character is on the ground
    public float minHeightr = 5f;           //The lowest height the Camera Holder can reach when a character is on a building
    public float maxHeightg = 8f;           //The highest height the Camera Holder can reach when a character is on the ground
    public float maxHeightr = 10f;          //The highest height the Camera Holder can reach when a character is on a building

    public string tags;                     //Gets the tag of the object (used with the raycasts)

    public bool flag;                       //Allows the while loop to run without the Update function running
    public bool freeMove;                   //Camera Holder can move around the map freely
    public bool charMove;                   //Camera is focused on the character
    public bool rotateLeft;                 //rotate left around the character
    public bool rotateRight;                //rotate right around the character
    public bool moveCam;                    //bool for moving the Camera Holder with edge scrolling
    public bool keys;                       //bool for moving the Camera Holder with wasd

    public Vector3 camDirection;            //Vector3 for moving the Camera Holder
    public Vector3 camHeight;               //Vector3 for raising/lowering the Camera Holder
    public Vector3 offset;                  //Vector3 values for the starting location of the Camera Holder
    public Vector3 charDistance;            //Vector3 values for the distance between the character and Camera Holder

    RaycastHit hit;

    public bool masterkey;

    void Start()
    {
        currHeight = transform.position.y;
        offset = new Vector3(0f, 2f, -2f);
        flag = true;
        freeMove = true;
        charMove = false;
        rotateLeft = false;
        rotateRight = false;
    }

    void Update()
    {
        camHeight = new Vector3(transform.position.x, currHeight, transform.position.z); //sets the height of the Camera holder

        charDistance = player.transform.position + offset;  //calculates the distance the camera will be behind a character

        if (flag == true)
        {
            Refresh();
        }
    }

    void LateUpdate()
    {
        if (masterkey == false)
        {
            if (charMove == true && rotateLeft == true)
            {
                playerCam.transform.RotateAround(player.transform.position, Vector3.up, -90 * Time.deltaTime);
                antiRot += 90;
            }
            else if (charMove == true && rotateRight == true)
            {
                playerCam.transform.RotateAround(player.transform.position, Vector3.up, 90 * Time.deltaTime);
                antiRot += -90;
            }
        }
    }

    public void FlipCamera()                //Flip between charCam and freeCam after the Character moves
    {
        if (!masterkey)
        {
            ResetFreeCam();
        }
    }

    public void Move()                      //If the move button is selected then switch to charCam
    {
        if (!masterkey)
        {
            freeMove = false;
            charMove = true;
            StartCoroutine(RotateCam(Vector3.up * antiRot, rotSpeed));
        }
    }

    public void Refresh()
    {
        if (freeMove == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)         //lowers the Camera Holders height
            {   //add code here to check if the player is on a roof or on the ground and then clamp the height to the correct height
                currHeight -= zoomSpeed;
                currHeight = Mathf.Clamp(currHeight, minHeightg, maxHeightg); //makes sure the Camera Holders height doesn't go below the min height
                transform.position = camHeight;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)    //raises the Camera Holders height
            {
                currHeight += zoomSpeed;
                currHeight = Mathf.Clamp(currHeight, minHeightg, maxHeightg); //makes sure the Camera Holders height doesn't go above the max height
                transform.position = camHeight;
            }

            if (Input.GetButtonDown("Rotate Left"))
            {
                currRot += -90;
                antiRot += 90;
                StartCoroutine(RotateCam(Vector3.up * -90, rotSpeed));
            }
            else if (Input.GetButtonDown("Rotate Right"))
            {
                currRot += 90;
                antiRot += -90;
                StartCoroutine(RotateCam(Vector3.up * 90, rotSpeed));
            }

            if (moveCam)                                                    //detects if the mouse is inside the edge scroll panel
            {
                transform.position += camDirection;
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

            if (Input.GetButtonDown("Middle Click"))
            {
                if (!masterkey)
                {
                    freeMove = false;
                    charMove = true;
                    StartCoroutine(RotateCam(Vector3.up * antiRot, rotSpeed));
                }
                else
                {
                    StartCoroutine(FlyCam(camSpeed));
                }
                
            }
        }
        else if (charMove == true && !masterkey)
        {
            if (Input.GetButtonDown("Up") || Input.GetButtonDown("Down") || Input.GetButtonDown("Right") || Input.GetButtonDown("Left") || moveCam)
            {   //if the user trys to move the camera around the map then switch to free mode
                StartCoroutine(WaitUntilRotation());
            }
            if (Input.GetButtonDown("Rotate Left"))
            {
                rotateLeft = true;
            }
            if (Input.GetButtonUp("Rotate Left"))
            {
                rotateLeft = false;
            }
            if (Input.GetButtonDown("Rotate Right"))
            {
                rotateRight = true;
            }
            if (Input.GetButtonUp("Rotate Right"))
            {
                rotateRight = false;
            }
        }
    }

    public void ResetFreeCam()                          //reset the freeCam to behind the character
    {
        if (!masterkey)
        {
            currHeight = Mathf.Round(charDistance.y);
            transform.eulerAngles = new Vector3(0, 0, 0);
            currRot = 0;
            antiRot = 0;
            charMove = false;
            freeMove = true;
            playerCam.SetActive(charMove);
            gameObject.transform.GetChild(0).gameObject.SetActive(!charMove);
            flag = true;
        }
    }

    public void StartMovingForward(bool opposite)
    {
        moveCam = true;

        camDirection = (!opposite) ? transform.forward : -transform.forward;
        camDirection *= camSpeed;
    }

    public void StartMovingSides(bool opposite)
    {
        moveCam = true;

        camDirection = (!opposite) ? transform.right : -transform.right;
        camDirection *= camSpeed;
    }

    public void StopMoving()
    {
        moveCam = false;
    }

    IEnumerator WaitUntilRotation()                             //if the charCam is rotated then wait until it is behind the character before switching to freeCam
    {
        if (!masterkey)
        {
            flag = false;

            if (antiRot > 0)
            {
                while (Mathf.Round(playerCam.transform.eulerAngles.y) != Mathf.Round(player.transform.eulerAngles.y))
                {
                    playerCam.transform.RotateAround(player.transform.position, Vector3.up, 90 * Time.deltaTime);
                    yield return null;
                }
                ResetFreeCam();
            }
            else if (antiRot < 0)
            {
                while (Mathf.Round(playerCam.transform.eulerAngles.y) != Mathf.Round(player.transform.eulerAngles.y))
                {
                    playerCam.transform.RotateAround(player.transform.position, Vector3.up, -90 * Time.deltaTime);
                    yield return null;
                }
                ResetFreeCam();
            }
            else
            {
                ResetFreeCam();
            }
        }
    }

    IEnumerator RotateCam(Vector3 angle, float time)            //smoothly rotates the camera 90 degrees
    {
        Quaternion fromAngle = transform.rotation;
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + angle);

        for (float t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);

            yield return null;
        }
        if (transform.rotation.y != -90 && charMove == false)                        //without this the rotation is +- 3 degrees, this makes sure the rotation is exactly 90
        {
            transform.rotation = Quaternion.Euler(0, currRot, 0);
        }
        if (charMove == true && !masterkey)                   //if changing to charCam
        {
            transform.eulerAngles = new Vector3(Mathf.Round(player.transform.eulerAngles.x), Mathf.Round(player.transform.eulerAngles.y), Mathf.Round(player.transform.eulerAngles.z));
            StartCoroutine(FlyCam(camSpeed));
        }
    }

    IEnumerator FlyCam(float time)                              //smoothly snaps the camera to the currently selected player
    {
        Vector3 fromDistance = transform.position;
        Vector3 toDistance = charDistance;

        for (float t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Slerp(fromDistance, toDistance, t);
            yield return null;
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(!charMove);
        playerCam.SetActive(charMove);
    }
 
    public void ActivateMasterKey()
    {
        if (masterkey)
        {
            masterkey = false;
        }
        else if (!masterkey)
        {
            masterkey = true;
        }
    }

}
