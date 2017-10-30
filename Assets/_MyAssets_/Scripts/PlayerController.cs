using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /**************************************************************************************************
     * This Class handles moving the character and changing the animations
     **************************************************************************************************/

    private Animator anim;

    private UnitTurn uTurn;

    public float currentVel = 0;

    public bool enter = false;
    public bool trigger = false;

    public int tempDir;
    public string tempTag;


    void Awake()
    {
        anim = GetComponent<Animator>();
        uTurn = GetComponent<UnitTurn>();
    }

    public void UpdateMove(Vector3 move, Vector3 target, float speed)
    {
        currentVel = speed;
        if (currentVel != 0)
        {
            Vector3 dir = target - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            anim.SetFloat("Movement", 1, 0.1f, Time.deltaTime);
        }
    }

    public void StopMoving()
    {
        anim.SetFloat("Movement", 0);
        if (trigger)
        {
            CoverDirection();
        }
        else
        {
            SetStance();
        }
    }

    public void SetStance()
    {
        anim.SetFloat("Stance", 0);
    }

    public void CoverDirection()
    {
        if (tempTag == "Half Cover")
        {
            anim.SetFloat("Stance", 1);
        }
        else if (tempTag == "Full Cover")
        {
            anim.SetFloat("Stance", 2);
        }
        else if (tempTag == "blank")
        {
            anim.SetFloat("Stance", 0);
        }

        if (tempDir == 0)
        {
            StartCoroutine(RotateToFaceDirection(Vector3.forward));
        }
        else if (tempDir == 1)
        {
            StartCoroutine(RotateToFaceDirection(-Vector3.forward));
        }
        else if (tempDir == 2)
        {
            StartCoroutine(RotateToFaceDirection(Vector3.right));
        }
        else if (tempDir == 3)
        {
            StartCoroutine(RotateToFaceDirection(-Vector3.right));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        enter = true;
        tempDir = other.gameObject.GetComponent<CoverManager>().FindDirection();
        tempTag = other.gameObject.GetComponent<CoverManager>().parentTag;
    }

    public void OnTriggerExit(Collider other)
    {
        trigger = false;
        enter = false;
    }

    IEnumerator RotateToFaceDirection(Vector3 targetDir, bool aimAtTheEnd = false)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 3;

            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
    }
}
