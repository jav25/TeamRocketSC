using UnityEngine;
using System.Collections;
using System;


public class csProjectileCode : MonoBehaviour
{
    public float ParentTransformDestroyTime = 4f; //if Hit Collider, Destroy ParentTransform ( Except : Flame Collider );
    public GameObject Explosion; //Explosion Effect. 
    public Transform ChildColliderTf; //Collider Object.
    public bool isRigidBody; //// if you want to Move Object use of RigidBody, Check this.
    public Transform FlameEffect;


    public void DestroyObj()
    {
        Destroy(ChildColliderTf.gameObject);
        Destroy(transform.gameObject, ParentTransformDestroyTime);
    }

    public void MakeExplosion()
    {
        GameObject Exp = Instantiate(Explosion,transform.position, transform.rotation) as GameObject;
        Exp.transform.parent = this.transform;
    }

    public void MakeExplosion(Vector3 Pos, Quaternion Rot)
    {
        Instantiate(Explosion, Pos, Rot);
    }

    public Transform Flame(Collider Col, string Name)
    {
        Transform Flm = Instantiate(FlameEffect, Col.transform.position, Col.transform.rotation) as Transform; // set Transform information of FlameEffect.
        csFlameCollider Fc = (csFlameCollider)Flm.transform.gameObject.GetComponent("csFlameCollider"); // set csFlameCollider Code using Flm.
        Fc.Name = Name; //Send FireEmissionName to csFlameCollider Code
        Flm.parent = Col.transform;
        return Flm;
    }

}
