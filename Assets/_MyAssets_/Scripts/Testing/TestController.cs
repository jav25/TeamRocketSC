using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestController : MonoBehaviour
{

    private Animator anim;

    private NavMeshAgent navMesh;

    private GameObject self_cache = null;

    public float currentVel = 0f;

    public Vector3 currentSpot;
    public Vector3 rayDistance;

    public bool trigger = true;
    public bool flag;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();

        self_cache = gameObject;

        currentSpot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        currentVel = navMesh.velocity.sqrMagnitude;

        if (Input.GetButtonDown("Right Click"))
        {
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Terrain")))
            {
                rayDistance = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                if (hit.transform.tag != self_cache.tag)
                {
                    if (navMesh.SetDestination(hit.point))
                    {
                        navMesh.isStopped = false;
                    }
                }
            }
        }

        if (currentVel <= 0)
        {
            anim.SetFloat("Movement", 0);
        }
        if (currentVel != 0)
        {
            Vector3 dir = rayDistance - currentSpot;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6);
            anim.SetFloat("Movement", 1, 0.1f, Time.deltaTime);
        }
    }
}