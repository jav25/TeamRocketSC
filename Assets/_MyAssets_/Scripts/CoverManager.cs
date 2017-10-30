using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour {

    public Collider player1;
    public Collider player2;
    public Collider player3;

    private Collider temp;

    public Vector3 directionV;
    public int direction;

    public int p1 = 0;
    public int p2 = 1;
    public int p3 = 2;

    public string parentTag;

    public bool enter = false;

    // north = 0
    // south = 1
    // east = 2
    // west = 3

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

    //try making another function that does all the stuff inside if(!enter) but by using collider other to get what the char id is
    //and then find a way to ignore that collider if another one comes in

    public void OnTriggerEnter(Collider other)
    {
        if (!enter)
        {
            if (player1.gameObject.GetComponent<UnitTurn>().charId == 0 && p1 == 0)
            {
                player1.gameObject.GetComponent<PlayerController>().trigger = true;
                p1 = -1;
            }
            else if (player2.gameObject.GetComponent<UnitTurn>().charId == 1 && p2 == 1)
            {
                player2.gameObject.GetComponent<PlayerController>().trigger = true;
                p2 = -1;
            }
            else if (player3.gameObject.GetComponent<UnitTurn>().charId == 2 && p3 == 2)
            {
                player3.gameObject.GetComponent<PlayerController>().trigger = true;
                p3 = -1;
            }
        }
    }
}
