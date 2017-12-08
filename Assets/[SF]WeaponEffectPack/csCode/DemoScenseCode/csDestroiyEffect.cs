using UnityEngine;
using System.Collections;

public class csDestroiyEffect : MonoBehaviour {

    public float DestroyTime;

    void Start()
    {
        if(DestroyTime > 0)
        Destroy(gameObject, DestroyTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.C))
            Destroy(gameObject);
    }
}
