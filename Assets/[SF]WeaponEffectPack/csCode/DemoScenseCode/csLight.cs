using UnityEngine;
using System.Collections;

public class csLight : MonoBehaviour {

    public float StartFadeTime = 0.5f;
    public float FadingTime = 1;
    float _Time = 0;
    Light _Light;

    public bool SetIntensity = true;
    public float WaitIntensityUpTime = 0.0f;
    public float IntensityValue = 1.0f;
    float _IntensityValue = 0.0f;

    void Start()
    {
        _Light = this.GetComponent<Light>();

        if (SetIntensity)
            _IntensityValue = IntensityValue;
    }
	
	void Update () {
        
        _Time += Time.deltaTime;
        _Light.intensity = _IntensityValue;

        if (WaitIntensityUpTime > 0)
        {
            if (_Time >= WaitIntensityUpTime && _Time < StartFadeTime)
            {
                if (_IntensityValue <= IntensityValue)
                    _IntensityValue += Time.deltaTime;

                if (_IntensityValue >= IntensityValue)
                    _IntensityValue = IntensityValue;
            }
        }

        if (_Time > StartFadeTime)
        {
            if (_IntensityValue >= 0)
                _IntensityValue -= Time.deltaTime * FadingTime;
        }
	}
}
