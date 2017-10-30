using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDistance : MonoBehaviour
{
    /**************************************************************************************************
     * This Class handles drawing the move line.
     **************************************************************************************************/

    private LineRenderer line;

    private RaycastHit hit;
    private Ray ray;

    public GameObject player;

    public Material moveDis;                        //distance the character can move and still use an action
    public Material sprintDis;                      //distance a character moves that is too far to still use an action
    public Material farDis;                         //distance is too far for a character to move to

    public Vector3 charPos;                         //the current characters position
    public Vector3 mousePos;                        //the mouses position
    public Vector3 lineDis;                         //the starting distance for the line renderer
    public Vector3 currLineDis;                     //draw the line to the location of the mouse

    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.material = moveDis;                    //colors the line to move Distance

        charPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        lineDis = new Vector3(charPos.x, 0, charPos.z);
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Terrain")))   //gets the location of the mouse relative to where it is pointing on the plane/ground
        {
            currLineDis = new Vector3(hit.point.x, 0, hit.point.z);             //updates where the line should be drawn to
            if (Vector3.Distance(currLineDis, lineDis) <= 4)                    //if the distance between the mouse and the character is 4 or less (move distance) make the line green
            {
                line.material = moveDis;
            }
            else if (Vector3.Distance(currLineDis, lineDis) > 4 && Vector3.Distance(currLineDis, lineDis) <= 10)    //make the line yellow (sprint distance)
            {
                line.material = sprintDis;
            }
            else if (Vector3.Distance(currLineDis, lineDis) > 10)               //make the line red (too far)
            {
                line.material = farDis;
            }

            mousePos = new Vector3(hit.point.x, 0, hit.point.z);
            line.SetPosition(0, charPos);                               //draws the line at the start point
            line.SetPosition(1, mousePos);                              //draws the line at the end point
        }
    }

    public void EnableLine()                                            //if the character is selected to move then turn on the line/ update the new positions
    {
        charPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        lineDis = new Vector3(charPos.x, 0, charPos.z);
    }
    public void DisableLine()
    {
        charPos = new Vector3(player.transform.position.x, 0, player.transform.position.z); //once the character has moved update the characters positon
    }

}
