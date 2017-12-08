using UnityEngine;
using System.Collections;

public class csDemoScenceControl : MonoBehaviour {

	public static float CameraDist = 10.0f;

    enum STATE
    {
        Le,
        Le2,
        F,
        Rk,
        Rk2,
        FT,
        G,
		LB,
		EMP
    }

    public string[] stLaserEffect;
    public string[] stLaserEffect2;
    public string[] stFire;
    public string[] stRocket;
    public string[] stRocket2;
    public string[] stFlameThrower;
    public string[] stGun;
	public string[] stLaserBall;
	public string[] stEMP;

    public Transform[] LaserEffect;
    public Transform[] LaserEffect2;
    public Transform[] Fire;
    public Transform[] Rocket;
    public Transform[] Rocket2;
    public Transform[] FlameThrower;
    public Transform[] Gun;
	public Transform[] LaserBall;
	public Transform[] EMP;

    public Transform MakePoint1;
    public Transform MakePoint2;
    public Transform MakePoint3;
    public Transform MakePoint4;
    public Transform MakePoint5;
	public Transform MakePoint6;
    public GUIText Text;
    public GUIText Text2;

    public GameObject Cube;
    public GameObject Cube2;
    public GameObject Cube3;
	public GameObject Cube4;
	public GameObject Spots;


    int LaserEffectLength;
    int LaserEffect2Length;
    int FireLength;
    int RocketLength;
    int Rocket2Length;
    int FlameThrowerLength;
    int GunLength;
	int LaserBallLength;
	int EMPLength;
    int Sum;
    int i;

    STATE _State = new STATE();

    void Start()
    {
        i = 1;
        LaserEffectLength = LaserEffect.Length;
        LaserEffect2Length = LaserEffect2.Length;
        FireLength = Fire.Length;
        RocketLength = Rocket.Length;
        Rocket2Length = Rocket2.Length;
        FlameThrowerLength = FlameThrower.Length;
        GunLength = Gun.Length;
		LaserBallLength = LaserBall.Length;
		EMPLength = EMP.Length;
        Sum = LaserEffectLength + LaserEffect2Length + FireLength + RocketLength+Rocket2Length + FlameThrowerLength + GunLength+LaserBallLength + EMPLength;
        
        Instantiate(LaserEffect[0], MakePoint1.transform.position, MakePoint1.transform.rotation);
        Text2.text = "1.Continue Fire Laser Beam 1";
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            if ((i - 1) <= Sum-2)
                i++;
            else
                i = 1;

            ChangeState();
            MakeEffect();
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if ((i - 1) > 0)
                i--;
            else
                i = Sum;

            ChangeState();
            MakeEffect();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeState();
            MakeEffect();
        }

        switch (_State)
        {
            case STATE.Le:
				CameraDist = 30;
                Text.text = "'W,S,A,D'-Move This Effect";
                Cube.SetActive(true);
                Cube2.SetActive(false);
                Cube3.SetActive(false);
				Spots.SetActive(false);
                break;
            case STATE.Le2:
				CameraDist = 30;
                Text.text = "";
                Cube.SetActive(false);
                Cube2.SetActive(true);
                Cube3.SetActive(false);
				Spots.SetActive(false);
                break;
            case STATE.F:
				CameraDist = 50;
                Text.text = "";
                Cube.SetActive(false);
                Cube2.SetActive(false);
                Cube3.SetActive(true);
				Spots.SetActive(false);
                break;
            case STATE.Rk:
				CameraDist = 30;
                Text.text = "";
                Cube.SetActive(false);
                Cube2.SetActive(true);
                Cube3.SetActive(false);
				Spots.SetActive(true);
                break;
            case STATE.Rk2:
				CameraDist = 50;
                Text.text = "";
                Cube.SetActive(false);
                Cube2.SetActive(false);
                Cube3.SetActive(false);
				Spots.SetActive(true);
                break;
            case STATE.FT:
				CameraDist = 30;
                Text.text = "'W,S,A,D'-Move This Effect , Mouse Left Button  - Fire Flame ";
                Cube.SetActive(false);
                Cube2.SetActive(true);
                Cube3.SetActive(false);
				Spots.SetActive(false);
                break;
            case STATE.G:
				CameraDist = 30;
                Text.text = "";
                Cube.SetActive(false);
                Cube2.SetActive(true);
                Cube3.SetActive(false);
				Spots.SetActive(true);
                break;
			case STATE.LB:
				CameraDist = 30;
				Text.text = "";
                Cube.SetActive(false);
                Cube2.SetActive(true);
                Cube3.SetActive(false);
				Spots.SetActive(false);
                break;
			case STATE.EMP:
				CameraDist = 40;
				Text.text = "";
				Cube.SetActive(false);
				Cube2.SetActive(false);
				Cube3.SetActive(false);
				Spots.SetActive(false);
				break;
            default:
                break;
        }
    }

    void ChangeState()
    {
        if (i >= 1 && i <= LaserEffectLength)
            _State = STATE.Le;
        else if
           (i > LaserEffectLength && i <= LaserEffectLength + LaserEffect2Length)
            _State = STATE.Le2;
        else if
           (i > LaserEffectLength + LaserEffect2Length && i <= LaserEffectLength + LaserEffect2Length + FireLength)
            _State = STATE.F;
        else if
           (i > LaserEffectLength + LaserEffect2Length + FireLength && i <= LaserEffectLength + LaserEffect2Length + FireLength + RocketLength)
            _State = STATE.Rk;
        else if
           (i > LaserEffectLength + LaserEffect2Length + FireLength + RocketLength && i <= LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length)
            _State = STATE.Rk2;
        else if
           (i > LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length && i <= LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength)
            _State = STATE.FT;
        else if
           (i > LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength && i <= LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength + GunLength)
            _State = STATE.G;
		else if
           (i > LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength+GunLength && i <= LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength + GunLength + LaserBallLength)
            _State = STATE.LB;
		else if
			(i > LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength + GunLength + LaserBallLength && i <= LaserEffectLength + LaserEffect2Length + FireLength + RocketLength + Rocket2Length + FlameThrowerLength + GunLength + LaserBallLength +EMPLength)
			_State = STATE.EMP;
    }

    void MakeEffect()
    {
        switch (_State)
        {
            case STATE.Le:
                Instantiate(LaserEffect[i - 1], MakePoint1.transform.position, MakePoint1.transform.rotation);
                Text2.text = i + "." + stLaserEffect[i - 1];
                break;
            case STATE.Le2:
                Instantiate(LaserEffect2[i - 1 - LaserEffectLength], MakePoint4.transform.position, MakePoint4.transform.rotation);
                Text2.text = i + "." + stLaserEffect2[i - 1 - LaserEffectLength];
                break;
            case STATE.F:
                Instantiate(Fire[i - 1 - LaserEffectLength - LaserEffect2Length], MakePoint3.transform.position, MakePoint3.transform.rotation);
                Text2.text = i + "." + stFire[i - 1 - LaserEffectLength - LaserEffect2Length];
                break;
            case STATE.Rk:
                Instantiate(Rocket[i - 1 - LaserEffectLength - LaserEffect2Length-FireLength], MakePoint4.transform.position, MakePoint4.transform.rotation);
                Text2.text = i + "." + stRocket[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength];
                break;
            case STATE.Rk2:
                Instantiate(Rocket2[i - 1 - LaserEffectLength - LaserEffect2Length-FireLength-RocketLength], MakePoint5.transform.position, MakePoint5.transform.rotation);
                Text2.text = i + "." + stRocket2[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength];
                break;
            case STATE.FT:
                Instantiate(FlameThrower[i - 1 - LaserEffectLength - LaserEffect2Length-FireLength-RocketLength-Rocket2Length], MakePoint2.transform.position, MakePoint2.transform.rotation);
                Text2.text = i + "." + stFlameThrower[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length];
                break;
            case STATE.G:
                Instantiate(Gun[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length-FlameThrowerLength], MakePoint2.transform.position, MakePoint2.transform.rotation);
                Text2.text = i + "." + stGun[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length - FlameThrowerLength];
                break;
			case STATE.LB:
                Instantiate(LaserBall[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length-FlameThrowerLength-GunLength], MakePoint4.transform.position, MakePoint4.transform.rotation);
                Text2.text = i + "." + stLaserBall[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length - FlameThrowerLength-GunLength];
                break;
			case STATE.EMP:
				Instantiate(EMP[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length-FlameThrowerLength-GunLength-LaserBallLength], MakePoint6.transform.position, MakePoint6.transform.rotation);
			Text2.text = i + "." + stEMP[i - 1 - LaserEffectLength - LaserEffect2Length - FireLength - RocketLength - Rocket2Length - FlameThrowerLength-GunLength - LaserBallLength];
			break;    
		default:
                break;
        }
    }
}