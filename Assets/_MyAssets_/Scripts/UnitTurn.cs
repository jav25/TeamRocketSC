using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitTurn : MonoBehaviour
{

    private NavMeshAgent navMesh;

    private GameObject self_cache = null;
    private GameObject closest;                                     //A reference to the closest enemy

    private Vector3 tempV;                                          //A temporary Vector for the enemies, used to find which enemy is closest
    private int index = 0;                                          //An index in the array for the closest enemy

    private PlayerController playerCon;
    private PlayerStats playerStats;
    private EnemyStats enemyStats;
    private MoveDistance moveDis;
    private CameraMove camMove;
    private GameManager gameManager;

    public GameObject mainCam;                                      //The Camera Holder
    public GameObject line;                                         //The Characters move line
    public GameObject gm;
    public GameObject[] enemy = new GameObject[3];                  //An array of enemies
    public GameObject gun;
    public GameObject special;

    public int specialCount = 0;

    public Behaviour charHalo;

    public bool charMove;                                           //Is the character moving
    public bool charTurn;                                           //Is it this characters turn
    public bool charSelect;                                         //Can this character be selected
    public bool charAction;                                         //Can this character still do an action
    public bool charEnable;                                         //Is this character Enabled
    public bool destinationSet;                                     //Has the destination been set
    public bool exitTrigger = false;                                //Has the Character exited cover

    public float currentVel = 0f;                                   //Velocity of the character
    public float moveDistance = 4f;                                 //Distance the character can move and still use an action
    public float maxDistance = 10f;                                 //The max distance a character can move

    public int charId = 0;                                          //The characters id
    public int rayId = 0;                                           //Used for selecting another character

    public Vector3 currentSpot;                                     //The current spot of the character
    public Vector3 rayDistance;                                     //The location to move the character to
    public Vector3 move;                                            //I need to remove this
    //public Vector3 groundNormal;


    public GameObject otherPlayer1;
    public GameObject otherPlayer2;

    public Vector3 otherVector1;                                    //The other characters Vectors
    public Vector3 otherVector2;

    public string team;

    public bool isDead = false;


    void Awake()
    {
        camMove = mainCam.GetComponent<CameraMove>();

        self_cache = gameObject;

        moveDis = line.GetComponent<MoveDistance>();

        gameManager = gm.GetComponent<GameManager>();

        playerCon = gameObject.GetComponent<PlayerController>();

        playerStats = gameObject.GetComponent<PlayerStats>();
        enemyStats = gameObject.GetComponent<EnemyStats>();

        charMove = false;
        charTurn = false;
        charSelect = true;
        charAction = true;
        charEnable = true;
        destinationSet = false;

        navMesh = GetComponent<NavMeshAgent>();
        currentSpot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        otherVector1 = new Vector3(otherPlayer1.transform.position.x, otherPlayer1.transform.position.y, otherPlayer1.transform.position.z);
        otherVector2 = new Vector3(otherPlayer2.transform.position.x, otherPlayer2.transform.position.y, otherPlayer2.transform.position.z);

        team = gameObject.tag;
    }

    public void SelectCharacter()
    {
        charSelect = true;
        currentSpot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
        destinationSet = false;
    }

    public void MoveActive()
    {
        moveDis = line.GetComponent<MoveDistance>();

        moveDis.EnableLine();

        charMove = true;
        charTurn = true;
        charAction = true;

        currentSpot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        otherVector1 = new Vector3(otherPlayer1.transform.position.x, otherPlayer1.transform.position.y, otherPlayer1.transform.position.z);
        otherVector2 = new Vector3(otherPlayer2.transform.position.x, otherPlayer2.transform.position.y, otherPlayer2.transform.position.z);

        charHalo.enabled = charEnable;
        line.SetActive(charEnable);

        MoveCharacter();

    }

    public void UnActive()
    {
        currentSpot = rayDistance;
        moveDis.DisableLine();
        gameManager.currentPlayer.GetComponent<UnitTurn>().charHalo.enabled = !charEnable;
        line.SetActive(!charEnable);
    }

    public void NewTurn()
    {
        SelectCharacter();
        charTurn = true;
        charSelect = true;
        charAction = true;
        charEnable = true;
        destinationSet = false;
    }

    public void UndoMove()                          //Used to cancel moving a character
    {
        SelectCharacter();
        charTurn = true;
        charSelect = true;
        charAction = true;
        charEnable = true;
        charMove = false;
        destinationSet = false;
        moveDis.DisableLine();
        line.SetActive(!charEnable);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver)
        {
            if (!isDead)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (charSelect == true && camMove.freeMove == true)
                {
                    if (Input.GetButtonDown("Left Click") && gameManager.currentPlayer.GetComponent<UnitTurn>().destinationSet == false)
                    {
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.tag == gameObject.transform.tag && hit.transform.gameObject != gameManager.currentPlayer)
                            {
                                if (hit.transform.gameObject.GetComponent<UnitTurn>().charSelect == false)
                                {

                                }
                                else
                                {
                                    gameManager.currentPlayer.GetComponent<UnitTurn>().rayId = hit.transform.gameObject.GetComponent<UnitTurn>().charId;
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
            /*else if (isDead == "Yes")
            {
                Debug.Log("TEST");
                //gameManager.EndTurn();
            }*/
        }
    }

    public void MoveCharacter()
    {
        charSelect = false;
        currentVel = navMesh.velocity.sqrMagnitude;
        move = navMesh.velocity;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!destinationSet)
        {
            if (Input.GetButtonDown("Right Click"))
            {
                destinationSet = true;
                if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Terrain")))
                {
                    if (!hit.collider.isTrigger)
                    {                                       //Is the new area in a collider and a trigger
                        exitTrigger = false;
                    }
                    else
                    {
                        exitTrigger = true;
                    }
                    //if (hit.transform.tag == "Player" && hit.transform.gameObject != gameObject)
                    //{

                    //}
                    rayDistance = new Vector3(hit.point.x, 0, hit.point.z);
                    if (hit.transform.tag != self_cache.tag)
                    {
                        if (Mathf.Abs(Vector3.Distance(rayDistance, currentSpot)) >= maxDistance)   //distance is to far to travel
                        {
                            //play sound or something to say thats too far to move
                            destinationSet = false;
                            Debug.Log("That Distance is too far to travel");
                        }
                        else if (Mathf.Abs(Vector3.Distance(rayDistance, otherVector2)) < 1 || Mathf.Abs(Vector3.Distance(rayDistance, otherVector1)) < 1)      //Checks to see if the new location is too close to another player
                        {
                            //Display that you can't move where another character is standing
                            destinationSet = false;
                            Debug.Log("Test Distance");
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
            else if (Input.GetButtonDown("Left Click"))                  //If the user left clicks the current player then undo the move
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        gameManager.Reselect();
                    }
                }
            }
        }
        playerCon.UpdateMove(move, rayDistance, currentVel);
    }

    public void FindEnemies()                                       //Searches for nearby enemies
    {
        if (team == "Player")
        {
            enemy = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemy.Length != 0)
            {
                index = 2;
                for (int i = 0; i < enemy.Length; i++)
                {
                    if (Vector3.Distance(currentSpot, enemy[i].transform.position) > playerStats.maxDistance)
                    {
                        //Display enemy is too far 0% chance of hit
                        Debug.Log(enemy[i]);
                    }
                    else
                    {
                        gameManager.CycleThroughEnemies();
                    }
                }
            }
        }
        else if(team == "Enemy")
        {
            enemy = GameObject.FindGameObjectsWithTag("Player");
            if (enemy.Length != 0)
            {
                index = 2;
                for (int i = 0; i < enemy.Length; i++)
                {
                    if (Vector3.Distance(currentSpot, enemy[i].transform.position) > enemyStats.maxDistance)
                    {
                        //Display enemy is too far 0% chance of hit
                        Debug.Log(enemy[i]);
                    }
                    else
                    {
                        gameManager.CycleThroughEnemies();
                    }
                }
            }
        }

        //if(enemy.Length != 0)
        //{
        //    index = 2;
        //    for(int i = 0; i < enemy.Length; i++)
        //    {
        //        if(Vector3.Distance(currentSpot, enemy[i].transform.position) > playerStats.maxDistance)
        //        {
        //            //Display enemy is too far 0% chance of hit
        //            Debug.Log(enemy[i]);
        //        }
        //        else
        //        {
        //            gameManager.CycleThroughEnemies();
        //        }
        //    }
        //}

        //enemy = GameObject.FindGameObjectsWithTag("Enemy");
        //if (enemy.Length != 0)                                                              //if an enemy is found
        //{
        //    closest = enemy[0];

        //    if (Vector3.Distance(currentSpot, closest.transform.position) > playerStats.maxDistance)      //checks to see if the enemies are out of range
        //    {
        //        Debug.Log(currentSpot);
        //        Debug.Log(closest.transform.position);
        //        Debug.Log(Vector3.Distance(currentSpot, closest.transform.position));
        //        closest = null;
        //        //Display a message that says no enemies are in range
        //        Debug.Log("No Enemies in range");
        //    }
        //    else if (Vector3.Distance(currentSpot, closest.transform.position) < playerStats.maxDistance) //the enemies are in range
        //    {
        //        GameObject[] tempEnemy = new GameObject[enemy.Length];                      //temp array to hold the enemies

        //        for (int i = 0; i < enemy.Length; i++)
        //        {
        //            tempEnemy[i] = enemy[i];
        //            if (Vector3.Distance(enemy[i].transform.position, currentSpot) < Vector3.Distance(tempV, currentSpot))    //gets the closest enemy to the player
        //            {
        //                closest = enemy[i];
        //                index = i;
        //                tempV = closest.transform.position;
        //            }
        //        }

        //        tempEnemy[0] = closest;                                                     //sets the closest enemy to the first spot in the temp array
        //        enemy[index] = enemy[0];                                                     //shifts the first enemy to the location of the closest enemy

        //        for (int i = index; i < tempEnemy.Length; i++)                               //adds the rest of the enemies to the temp array
        //        {
        //            tempEnemy[i] = enemy[i];
        //        }
        //        for (int i = 0; i < tempEnemy.Length; i++)                                  //adds the sorted list of enemies back into the enemy array
        //        {
        //            enemy[i] = tempEnemy[i];
        //        }
        //        playerCon.CharacterAim();                                                   //Have the Character aim at the enemies
        //        index = enemy[0].gameObject.GetComponent<UnitTurn>().charId;
        //        gameManager.CycleThroughEnemies(enemy, index);                               //sends the enemies to the gameManager to cycle through which one to attack
        //    }
        //}
        //else                                                            //will be probably used for determining if all enemies are dead then mission accomplished
        //{
        //    Debug.Log("NO ENEMIES WAHH");
        //}

    }

    IEnumerator WaitUntilReachTargetClose()
    {
        yield return new WaitForSeconds(0.025f);
        yield return new WaitUntil(() => navMesh.remainingDistance == 0);
        if (navMesh.remainingDistance == 0)
        {
            if (!exitTrigger)                   //if the player leaves cover then reset the stance
            {
                playerCon.trigger = false;
            }
            playerCon.StopMoving();
        }
        charMove = false;
        charAction = false;
    }

    IEnumerator WaitUntilReachTarget()
    {
        yield return new WaitForSeconds(0.025f);
        yield return new WaitUntil(() => navMesh.remainingDistance == 0);
        if (navMesh.remainingDistance == 0)
        {
            if (!exitTrigger)               //if the player leaves cover then reset the stance
            {
                playerCon.trigger = false;
            }
            playerCon.StopMoving();
        }
        charTurn = false;
        charMove = false;
    }
}
