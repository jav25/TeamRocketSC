using UnityEngine;
using System.Collections;

public class csFlame : MonoBehaviour {

    public Transform FlameParticle;
    public csCollisionCode ColCode;
    public Collider Col;
    public AudioSource SoundEffect;
    float CurrentPitch = 0.0f;
    bool FlameStart = true;

    void Start()
    {
        FlameStart = false;
        Col.enabled = false;
    }

    void Update()
    {
        ChildParticleCheck();

        if (SoundEffect.pitch <= 0)
            SoundEffect.mute = true;
        else
            SoundEffect.mute = false;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            CurrentPitch += 0.05f;
            if (CurrentPitch > 1)
                CurrentPitch = 1;
            SoundEffect.pitch = Mathf.Clamp(CurrentPitch, 0, 1);
            ColCode.Flaming = true;
            FlameStart = true;
            Col.enabled = true;
        }
        else
        {
            CurrentPitch -= 0.05f;
            if (CurrentPitch < 0)
                CurrentPitch = 0;
            SoundEffect.pitch = Mathf.Clamp(CurrentPitch, 0, 1);
            ColCode.Flaming = false;
            FlameStart = false;
            Col.enabled = false;
        }
    }

    void ChildParticleCheck()
    {
        ParticleSystem[] ParticleSystems = FlameParticle.GetComponentsInChildren<ParticleSystem>(); //Scan All Shuriken Particle inside of _Effect
        for (int i = 0; i < ParticleSystems.Length; i++) //if FlameStart is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
        {
            if (FlameStart)
                ParticleSystems[i].Play();
            else
                ParticleSystems[i].Stop();
        }
    }
}
