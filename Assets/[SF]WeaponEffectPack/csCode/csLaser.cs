using UnityEngine;
using System.Collections;


public class csLaser : MonoBehaviour
{

    public float Width = 1.5f; //LineRenderer Width Value
    float CurrentWidth = 0.0f;
    public float Offset = 1.0f; //LineRenderer MainTexture Offset Value 
    public float MaxLength = Mathf.Infinity; 
    public Color StartColor; 
    public Color EndColor;
    public Transform LaserHitEffect; //For Laser Hit Effect.
    public Material _Material; //For LineRenderer Material
    public bool MakeParticle;
    bool isHit; //if Raycast Hit Something, Make this Effect to here.
	public AudioSource SoundEffect;
	float CurrentPitch = 0.0f;


    Light _Light;
    float LightIntensity = 1;
    private LineRenderer _LineRenderer; //LineRenderer Value
    private float NowLength; // if Raycast Hit Something, Save Length Information Between this transform , RacastHit's hit point.
    private GameObject _Effect;
    

    enum State
    {
        Start,
        Loop,
        End,
    };
    State LaserState = State.Start; 

    void Awake()
    {
        if (!(LineRenderer)transform.gameObject.GetComponent("LineRenderer")) // if LineRenderer is not attach, attach LineRenderer
            this.gameObject.AddComponent<LineRenderer>();
        if (MakeParticle != true)//if you Want to Particle Every Frame, Check this
        {
            Transform MakedEffect = Instantiate(LaserHitEffect, transform.position, transform.rotation) as Transform; // Make Effect
            _Effect = MakedEffect.gameObject; // Set MakeEffect Information To _Effect
            _Light = _Effect.GetComponentInChildren<Light>();

        }

        _LineRenderer = GetComponent<LineRenderer>(); // Set this GameObject LineRenderer Value 
        _LineRenderer.material = _Material; // Set Material
        _LineRenderer.SetWidth(0, 0); // Set Width
        _LineRenderer.SetColors(StartColor, EndColor); // Set Color

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength)) // if hit do Catch Something, Executing this Function
        {
            isHit = true; // for Particle Effect Execute, Set ishit to true
            NowLength = hit.distance; 
            if (MakeParticle)
                Instantiate(_Effect, hit.point, hit.transform.rotation); // Make Particle Every Frame.
            else
            {
                _Effect.transform.position = hit.point; 
                _Effect.transform.rotation = hit.transform.rotation;
            }
        }
        else // if hit don't Catch Something, Particle Effect didn't Execute. so Set is hit to false , Set NowLength to MaxLength
        {
            isHit = false;
            NowLength = MaxLength;
        }
      
        ChildParticleCheck(); // Contol Child of this gmaeobject's ParticleSystem

        //Laser Width Change.
        switch (LaserState)
        {
            case State.Start:
                CurrentWidth += 0.1f;
                _LineRenderer.SetWidth(CurrentWidth, CurrentWidth);
                CurrentPitch += 0.05f;
                if (CurrentPitch > 1)
                    CurrentPitch = 1;
                SoundEffect.pitch = Mathf.Clamp(CurrentPitch, 0, 1);
                if (CurrentWidth >= Width)
                {
                    LaserState = State.Loop;
                    CurrentWidth = Width;
                    _LineRenderer.SetWidth(Width, Width);
                    StartCoroutine(DelayLoopFunction(3));
                }
                break;
            case State.End:
                CurrentWidth -= 0.1f;
                CurrentPitch -= 0.025f;
                if (CurrentPitch < 0)
                    CurrentPitch = 0;
                SoundEffect.pitch = Mathf.Clamp(CurrentPitch, 0, 1);
                if (CurrentWidth <= 0)
                {
                    CurrentWidth = 0;
                    ChildParticleCheck();
                    Destroy(gameObject);
                }
                _LineRenderer.SetWidth(CurrentWidth, CurrentWidth);
                break;

        }

        Vector3 NewPos = this.transform.position + new Vector3(transform.forward.x * (NowLength - 1)
            , transform.forward.y * (NowLength - 1), transform.forward.z * (NowLength - 1)); //Set LineRenderer Length throught NowLength's information.                                                                         
        _LineRenderer.SetPosition(0, transform.position); //Set this GameObject's Position
        _LineRenderer.SetPosition(1, NewPos); //Set Next SetPosition use NewPos information
        _LineRenderer.material.SetTextureOffset("_MainTex",
            new Vector2(-Time.time * 30f * Offset, 0.0f)); //Because of Movement of Laser, Change x Offset throught Offset Value.
        _LineRenderer.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(NowLength/10, _LineRenderer.GetComponent<Renderer>().materials[0].mainTextureScale.y);
    }

    IEnumerator DelayLoopFunction(float Time)
    {
        yield return new WaitForSeconds(Time);
        LaserState = State.End;
    }

    void ChildParticleCheck()
    {
        ParticleSystem[] ParticleSystems = _Effect.GetComponentsInChildren<ParticleSystem>(); //Scan All Shuriken Particle inside of _Effect
        for (int i = 0; i < ParticleSystems.Length; i++) //if isHIt is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
        {
            if (isHit && CurrentWidth > 0)
            {
                LightIntensity += 0.1f;
                _Light.intensity = Mathf.Clamp(LightIntensity, 0, 1);
                ParticleSystems[i].Play();
            }
            else
            {
                ParticleSystems[i].Stop();
                _Light.intensity = 0;
            }
        }
    }
}
