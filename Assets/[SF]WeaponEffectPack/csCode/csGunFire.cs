using UnityEngine;
using System.Collections;

public class csGunFire : MonoBehaviour {


    public bool isSpread = true; 
    public GameObject Bullet;
    public Transform GunFirePoint;
    public GameObject Muzzle;
    public GameObject AnimationObject;
    public int SpreadRange = 10;
    public int SpreadScale = 500;
    public int BulletCount = 1;
    float delay = 2;
    public float MaxDelay = 1;

	void Update () {

        delay += Time.deltaTime;

        if (delay >= MaxDelay)
        {
            delay = 0;
            GunFire();
        }
	}

    void GunFire()
    {
        Instantiate(Muzzle, GunFirePoint.transform.position, GunFirePoint.transform.rotation);

        for (int i = 0; i < BulletCount; i++)
        {
            RaycastHit Hit;
            Vector3 direction = transform.InverseTransformDirection(Vector3.forward);
            if(isSpread)
                direction += new Vector3(Random.Range(SpreadRange, -SpreadRange), Random.Range(SpreadRange, -SpreadRange), Random.Range(SpreadRange, -SpreadRange)) / SpreadScale;
            if (Physics.Raycast(transform.position, direction, out Hit, 100))
                Instantiate(Bullet, Hit.point, Hit.collider.gameObject.transform.rotation);
        }
    }
}
