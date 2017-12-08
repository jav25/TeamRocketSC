using UnityEngine;
using System.Collections;

public class csFlameCollider : MonoBehaviour {

    [HideInInspector]
    public float delay = 5; // value of keep Flaming  
    [HideInInspector]
    public csCollisionCode CollisionCode;
    [HideInInspector]
    public string Name; // receive FireName on csProjectileCode

    void Start()
    {
        delay = 5; 
        this.name = Name; // set this Name
    }

	void Update ()
    {
        //Debug.Log(delay);

        // if Fire Collider is not Colliding This Object, Decreace effect survive time
        delay -= Time.deltaTime;

        ParticleSystem Ps = new ParticleSystem();
        Ps = this.transform.GetComponent<ParticleSystem>();

        if (delay < 0) // if Effect Survive Time is less than 0, Change Name and Destroy.
        {
            this.name = Name + "-Destroing";
            Destroy(this.gameObject, Ps.startLifetime);
            Ps.Stop();
        }
        else
            Ps.Play();
	}

    IEnumerator Destroy(float T)
    {
        yield return new WaitForSeconds(T);
        Destroy(gameObject);
    }
}
