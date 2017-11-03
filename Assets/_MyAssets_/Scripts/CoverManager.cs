using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public Vector3 directionV;
    public int direction;

    public int p = -1;

    public string parentTag;

    /*
     * north = 0
     * south = 1
     * east = 2
     * west = 3
     */

    public int FindDirection()
    {
        directionV = gameObject.GetComponent<BoxCollider>().center;

        if (directionV.z == 1)
        {
            direction = 0;
            parentTag = transform.parent.tag;
        }
        else if (directionV.z == -1)
        {
            direction = 1;
            parentTag = transform.parent.tag;
        }
        else if (directionV.x == .5)
        {
            direction = 2;
            parentTag = transform.parent.tag;
        }
        else if (directionV.x == -.5)
        {
            direction = 3;
            parentTag = transform.parent.tag;
        }
        else
        {
            direction = -1;
            parentTag = "blank";
        }
        return direction;
    }

}
