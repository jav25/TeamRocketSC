using UnityEngine;
using System.Collections;

public class csMove : MonoBehaviour {

    public bool isUp = true;
    public float Ypos = 50;
    Vector3 Vec;

	void Update () {

        if (Ypos == 0)
            Ypos = transform.position.y;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -40, 40), Ypos, Mathf.Clamp(transform.position.z, -40, 40));

        if (isUp)
            Vec = Vector3.up;
        else
            Vec = Vector3.forward;

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vec * 20 * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(Vec * -20 * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * 20 * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.right * -20 * Time.deltaTime);

	}
}
