using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLaser : MonoBehaviour
{
    //public GameObject player;

    public GameObject hitSpot;

    public GameObject gm;

    private GameManager gameManager;

    public float Width = 1.0f; //LineRenderer Width Value
    public float Offset = 1.0f; //LineRenderer MainTexture Offset Value 
    public float MaxLength = Mathf.Infinity;
    public Color StartColor;
    public Color EndColor;
    public float AlphaSpeed = 1f;
    public Transform LaserHitEffect; //For Laser Hit Effect.
    public Material _Material; //For LineRenderer Material

    private LineRenderer _LineRenderer; //LineRenderer Value
    private float NowLength; // if Raycast Hit Something, Save Length Information Between this transform , RacastHit's hit point.
    private GameObject _Effect;
    float AlphaValue = 1.0f;

    void Start()
    {
        _LineRenderer = GetComponent<LineRenderer>(); //LineRenderer Set

        _LineRenderer.material = _Material;

        gameManager = gm.GetComponent<GameManager>();

        hitSpot = gameManager.enemyTeam[0];
    }

    public void FireLaser()
    {
         AlphaValue = 1f;
        _LineRenderer.GetComponent<Renderer>().enabled = true;
        _LineRenderer.SetWidth(Width, Width);
        _LineRenderer.SetColors(StartColor, EndColor);
        _LineRenderer.SetPosition(0, transform.position);

        //Vector3 NewPos = new Vector3(51.3f, 1f, 50.01f);
        

            Vector3 NewPos = new Vector3(hitSpot.transform.position.x, 1f, hitSpot.transform.position.z);

            _LineRenderer.SetPosition(1, NewPos); //LineRenderer 2 Position Set.
            Transform Obj = Instantiate(LaserHitEffect, transform.position, Quaternion.identity) as Transform; // Make Effect.
            Obj.transform.position = NewPos;
            //Obj.transform.rotation = hit.collider.transform.rotation;
            Obj.transform.parent = this.transform;

        StartCoroutine(DestroyLaser());
    }

    void Update()
    {
        /*if (Input.GetButtonDown("Test Fire"))
        {
            AlphaValue = 1f;
            _LineRenderer.GetComponent<Renderer>().enabled = true;
            _LineRenderer.SetWidth(Width, Width);
            _LineRenderer.SetColors(StartColor, EndColor);
            _LineRenderer.SetPosition(0, transform.position);

            //Vector3 NewPos = new Vector3(51.3f, 1f, 50.01f);

            Vector3 NewPos = hitSpot.transform.position;

            _LineRenderer.SetPosition(1, NewPos); //LineRenderer 2 Position Set.
            Transform Obj = Instantiate(LaserHitEffect, transform.position, Quaternion.identity) as Transform; // Make Effect.
            Obj.transform.position = NewPos;
            //Obj.transform.rotation = hit.collider.transform.rotation;
            Obj.transform.parent = this.transform;

            StartCoroutine(DestroyLaser());

        }*/

    }

    IEnumerator DestroyLaser()
    {
        while (AlphaValue > 0)
        {
            _LineRenderer.material.SetTextureOffset("_MainTex",
                   new Vector2(-Time.time * 30f * Offset, 0.0f)); //Because of Movement of Laser, Change x Offset throught Offset Value.

            AlphaValue -= Time.deltaTime * AlphaSpeed; // For disapperaing LineRenderer Texture, Alpha Value decreace.
            _LineRenderer.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(StartColor.r, StartColor.g, StartColor.b, AlphaValue)); // color or alpha value set.
            yield return null;
        }
        _LineRenderer.GetComponent<Renderer>().enabled = false;
    }
}
