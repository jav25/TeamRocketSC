using UnityEngine;
using System.Collections;

public class csCollisionCode : MonoBehaviour {

    public csProjectileCode Code; //Parent Object Code.
    public bool OnMove = true; //Move Check
    public bool isExplosion; //Explosion Check
    public bool isDestroy; //Destroy Check
    public bool isRaycastHit; //if you want use Raycast Colision Check, Check this.
    public bool isAnimationPlay;
    public float MaxLength; //MaxLength Set
    public float Speed = 60;
    bool isExplosionPlayed = true; // is Check Explosion is provoke when you use Raycast. 
    public bool isFlame=true; // Flame Thrower Check
    [HideInInspector]
    public bool Flaming = true; // Check Flaming
    [HideInInspector]
    public Transform FlameParticle; //This is For inherit to Parent Object
    [HideInInspector]
    public Transform FlameParticle2; //This is For Collision Object
    public string FireName; // For Check the FireEmission name in the Hitted Object.
	Vector3 Velocity = Vector3.zero;

    void FixedUpdate()
    {

        if (OnMove)
        {
            if (Code.isRigidBody) // if Parents code isRigidBody is true, execute this.
            {
                Velocity = (Code.GetComponent<Rigidbody>().rotation * Vector3.forward) * (Speed);
                Code.GetComponent<Rigidbody>().velocity = Vector3.Lerp(Code.GetComponent<Rigidbody>().velocity, Velocity, Time.fixedDeltaTime);
            }
        }
    }

    void Update()
    {
        if (OnMove)
        {
            if (!Code.isRigidBody) // if Parents code isRigidBody is false, execute this.
                Code.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }


        if (isAnimationPlay)
            GetComponent<Animation>().Play(); // usually, use this to check flame object Collision.

        if (isFlame)
        {
            if (Flaming == false) // if Flaming is False FlameParticle2 Change null
            {
                FlameParticle2 = null;
            }

            if (FlameParticle2 != null) // if Flaming is Ture and FlameParticle2 is not null, execute this 'if' function 
            {
                if (FlameParticle2.Find(FireName)) // Find "Fire"name in the FlameParticle2 Object.
                {
                    Transform Finds = FlameParticle2.Find(FireName).transform; //Set Fire Value to Finds Value becuase of Hitted object's Plame Emission
                    csFlameCollider Fc = (csFlameCollider)Finds.GetComponent("csFlameCollider"); //Change "csFlameCollider" Code
                    Fc.delay = 5; // keep emitting
                }
            }
        }

        if (isRaycastHit) //this is execute when you check the isRaycastHit
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength))
            {
                if (isExplosionPlayed) // for one execute Explosion
                {
                    isExplosionPlayed = false;
                    Code.MakeExplosion(hit.point, hit.collider.transform.rotation);
                }
                Code.DestroyObj();
            }
        }
    }

    private void OnTriggerEnter(Collider Object)
    {
        //this is execute when you don't check the isRaycastHit
        if (!isRaycastHit)
                HitFunction(Object);
            else
                HitFunction(Object);
    }

    private void OnTriggerStay(Collider Object) //Functioning isFlame is true
    {
        if (isFlame)
        {
            FlameParticle2 = Object.transform;
        }
    }

    private void OnTriggerExit(Collider Object) //Functioning isFlame is true
    {
        if (isFlame)
        {
            FlameParticle2 = null;
        }
    }

    void HitFunction(Collider Object)
    {
        if (isExplosion)
            Code.MakeExplosion();
        if (OnMove)
		{
			Speed = 0;
            OnMove = false;
		}
        if (isDestroy)
            Code.DestroyObj();
        if (isFlame)
        {
            if (!(Object.transform.Find(FireName)))
            {
                FlameParticle2 = Object.transform;
                FlameParticle = Code.Flame(Object,FireName); // send Object Information and FireEmission Name
                FlameParticle.parent = Object.transform;
            }
        }
    }
}
