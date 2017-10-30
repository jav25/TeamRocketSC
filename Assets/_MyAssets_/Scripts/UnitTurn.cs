using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitTurn : MonoBehaviour {

    private NavMeshAgent navMesh;

    private GameObject self_cache = null;
    private GameObject closest;

    private Vector3 tempV;
    private int index = 0;

    private PlayerController playerCon;
    private MoveDistance moveDis;
    private CameraMove camMove;
    private GameManager gameManager;

    public GameObject mainCam;
    public GameObject line;
    public GameObject gm;
    public GameObject[] enemy = new GameObject[3];

    public Behaviour charHalo;

    public bool charMove;
    public bool charTurn;
    public bool charSelect;
    public bool charAction;
    public bool charEnable;

    public float currentVel = 0f;
    public float moveDistance = 4f;
    public float maxDistance = 10f;

    public int charId = 0;
    public int rayId = 0;

    public Vector3 currentSpot;
    public Vector3 rayDistance;
    public Vector3 move;
    public Vector3 groundNormal;

    void Awake()
    {
        camMove = mainCam.GetComponent<CameraMove>();

        self_cache = gameObject;

        moveDis = line.GetComponent<MoveDistance>();

        gameManager = gm.GetComponent<GameManager>();

        playerCon = gameObject.GetComponent<PlayerController>();

        charMove = false;
        charTurn = false;
        charSelect = true;
        charAction = true;
        charEnable = true;

        navMesh = GetComponent<NavMeshAgent>();
        currentSpot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

	public void SelectCharacter()
    {
        charSelect = true;
        rayId = gameObject.GetComponent<UnitTurn>().charId;
    }

    public void TurnOver()
    {
        currentSpot = rayDistance;
        moveDis.DisableLine();
        charHalo.enabled = !charEnable;
        line.SetActive(!charEnable);
        charMove = false;
        charTurn = false;
        charSelect = false;
        charAction = false;
    }

    public void MoveActive()
    {
        moveDis = line.GetComponent<MoveDistance>();

        moveDis.EnableLine();

        charMove = true;
        charTurn = true;
        charAction = true;

        currentSpot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        charHalo.enabled = charEnable;
        line.SetActive(charEnable);

        MoveCharacter();

    }

    public void UnActive()
    {
        currentSpot = rayDistance;
        moveDis.DisableLine();
        charHalo.enabled = !charEnable;
        line.SetActive(!charEnable);
    }

    public void NewTurn()
    {
        SelectCharacter();
        charTurn = true;
        charSelect = true;
        charAction = true;
        charEnable = true;
    }
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(charSelect == true && camMove.freeMove == true)
        {
            if(Input.GetButtonDown("Left Click"))
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.tag == "Player" && hit.transform.gameObject != gameObject)
                    {
                        if (hit.transform.gameObject.GetComponent<UnitTurn>().charSelect == false)
                        {

                        }
                        else
                        {
                            rayId = hit.transform.gameObject.GetComponent<UnitTurn>().charId;
                            UnActive();
                            gameManager.ChangeCharacters();
                        }
                    }
                }
            }
        }
        else if (charMove == true)
        {
            MoveCharacter();
        }

	}

    public void MoveCharacter()
    {
        charSelect = false;
        currentVel = navMesh.velocity.sqrMagnitude;
        move = navMesh.velocity;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Input.GetButtonDown("Right Click"))
        {
            if(Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Terrain")))
            {
                rayDistance = new Vector3(hit.point.x, 0, hit.point.z);
                if(hit.transform.tag != self_cache.tag)
                {
                    if (Mathf.Abs(Vector3.Distance(rayDistance, currentSpot)) >= maxDistance)   //distance is to far to travel
                    {
                        //play sound or something to say thats too far to move
                        Debug.Log("That Distance is too far to travel");
                    }
                    else if (Vector3.Distance(rayDistance, currentSpot) < maxDistance)
                    {
                        if (Vector3.Distance(rayDistance, currentSpot) <= moveDistance)         //the character can still take an action
                        {
                            currentSpot = rayDistance;
                            moveDis.DisableLine();
                            line.SetActive(!charEnable);
                            if (hit.transform.tag != self_cache.tag)
                            {
                                if (navMesh.SetDestination(hit.point))
                                {
                                    navMesh.isStopped = false;
                                    StartCoroutine(WaitUntilReachTargetClose());        //makes everything wait until the character is done moving before the next action can be chosen
                                }
                            }
                        }
                        else
                        {
                            UnActive();
                            if (hit.transform.tag != self_cache.tag)
                            {
                                if (navMesh.SetDestination(hit.point))
                                {
                                    navMesh.isStopped = false;
                                    StartCoroutine(WaitUntilReachTarget());
                                }
                            }
                        }
                    }
                }
            }
        }
        playerCon.UpdateMove(move, rayDistance, currentVel);
    }

    public void FindEnemies()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemy.Length != 0)                                                              //if an enemy is found
        {
            closest = enemy[0];

            if (Vector3.Distance(currentSpot, closest.transform.position) > 10)      //checks to see if the enemies are out of range
            {
                Debug.Log(currentSpot);
                Debug.Log(closest.transform.position);
                Debug.Log(Vector3.Distance(currentSpot, closest.transform.position));
                closest = null;
                //Display a message that says no enemies are in range
                Debug.Log("No Enemies in range");
            }
            else if (Vector3.Distance(currentSpot, closest.transform.position) < 10) //the enemies are in range
            {
                GameObject[] tempEnemy = new GameObject[enemy.Length];                      //temp array to hold the enemies

                for (int i = 0; i < enemy.Length; i++)
                {
                    tempEnemy[i] = enemy[i];
                    if (Vector3.Distance(enemy[i].transform.position, currentSpot) < Vector3.Distance(tempV, currentSpot))    //gets the closest enemy to the player
                    {
                        closest = enemy[i];
                        index = i;
                        tempV = closest.transform.position;
                    }
                }

                tempEnemy[0] = closest;                                                     //sets the closest enemy to the first spot in the temp array
                enemy[index] = enemy[0];                                                     //shifts the first enemy to the location of the closest enemy

                for (int i = index; i < tempEnemy.Length; i++)                               //adds the rest of the enemies to the temp array
                {
                    tempEnemy[i] = enemy[i];
                }
                for (int i = 0; i < tempEnemy.Length; i++)                                  //adds the sorted list of enemies back into the enemy array
                {
                    enemy[i] = tempEnemy[i];
                }
                gameManager.CycleThroughEnemies(enemy, index);                               //sends the enemies to the gameManager to cycle through which one to attack
            }
        }
        else                                                            //will be probably used for determining if all enemies are dead then mission accomplished
        {
            Debug.Log("NO ENEMIES WAHH");
        }

    }

    IEnumerator WaitUntilReachTargetClose()
    {
        yield return new WaitForSeconds(0.025f);
        yield return new WaitUntil(() => navMesh.remainingDistance == 0);
        if(navMesh.remainingDistance == 0)
        {
            playerCon.StopMoving();
        }
        charMove = false;
        charAction = false;
    }

    IEnumerator WaitUntilReachTarget()
    {
        yield return new WaitForSeconds(0.025f);
        yield return new WaitUntil(() => navMesh.remainingDistance == 0);
        if(navMesh.remainingDistance == 0)
        {
            playerCon.StopMoving();
        }
        charTurn = false;
        charMove = false;
    }
}
