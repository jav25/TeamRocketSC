using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /**************************************************************************************************
     * This Class handles the various components of the turns and selecting of characters.
     **************************************************************************************************/

    private CameraMove camMove;
    private UnitTurn uTurn;
    private PlayerController playerCon;
    private PlayerStats playerStats;
    private EnemyStats enemyStats;
    private ControlLaser controlLaser;
    private ControlLaser special;

    private int eCount;                                             //holds the element for an enemy in an enemy array
    private static int charId = 0;                                  //holds the id of the currently selected character
    private int count = 0;                                          //this counts how many characters have ended their turn during this turn
    public int turnCount = 0;                                       //just a counter for the turn
    private GameObject nextCam;                                     //used for controlling both teams on one computer
    private GameObject nextPlayer;
    private GameObject lastPlayer;
    private GameObject[] nextTeam = new GameObject[3];              //used for holding the next team up
    public static GameObject[] enemyOrder = new GameObject[3];
    public static GameObject[] playerOrder = new GameObject[3];

    public GameObject[] playerTeam = new GameObject[3];             //an array of player characters
    public GameObject[] enemyTeam = new GameObject[3];              //an array of enemy characters, used for controlling both teams on one computer
    public GameObject currentPlayer;                                //current player in the array
    public GameObject camHolder;                                    //Camera Holder object
    public GameObject enemyHolder;                                  //Holds the enemy Camera Holder, used for controlling both teams on one computer
    public GameObject playerActions;                                //PlayerActions Hud


    public GameObject gameOverCanvas;
    public GameObject RobotWin;
    public GameObject HumanWin;

    public Image pSlider;
    public Image eSlider;
    public Image aSlider;
    public Image healthBar;
    public Image emptyBar;
    public Image enemyBar;
    public Image ammoBar;

    public GameObject[] healthImages = new GameObject[21];
    public GameObject[] enemyImages = new GameObject[21];
    public GameObject[] ammoImages = new GameObject[12];
    //public int pHealthCount;
    //public int eHealthCount;

    //public GameObject[] playerHealth = new GameObject[21];

    //public Slider eSlider;                                           //displays enemy health

    public Text characterName;
    public Text characterClass;
    public Text characterHealth;
    public Text characterAmmo;
    public Text enemyName;
    public Text enemyHealth;
    public Text BattleLog;
    public Text SpecialCounter;

    public Button moveButton;

    public GameObject[] enemyCam = new GameObject[3];               //used for cycling between which enemy to attack
    public GameObject[] aCam = new GameObject[3];

    public GameObject enemyDisplay;                                 //HUD for cycling between which enemy to attack

    public bool playerTurn;
    public bool enemyTurn;
    public bool specialMove = false;

    public Animator pAnim;
    public Animator eAnim;

    public bool robotWin = false;
    public bool humanWin = false;

    public bool gameOver = false;

    public bool masterKey;                                          //use if you want to move the same character over and over

    // Use this for initialization
    void Start()
    {
        playerTurn = true;
        enemyTurn = false;

        camMove = camHolder.GetComponent<CameraMove>();

        currentPlayer = playerTeam[charId];
        characterName.text = currentPlayer.name;
        //testing both teams on one computer
        nextTeam = playerTeam;
        nextCam = camHolder;
        //

        //eSlider.maxValue = enemyTeam[0].GetComponent<EnemyStats>().maxHealth;
        //eSlider.value = enemyTeam[0].GetComponent<EnemyStats>().health;

        playerStats = currentPlayer.GetComponent<PlayerStats>();
        characterClass.text = playerStats.charClass;
        characterHealth.text = playerStats.health + "/" + playerStats.maxHealth;
        characterAmmo.text = playerStats.remainingAmmo + "/" + playerStats.ammo;

        for (int i = 0; i < healthImages.Length; i++)
        {
            healthImages[i] = pSlider.gameObject.transform.GetChild(i).gameObject;
            enemyImages[i] = eSlider.gameObject.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < ammoImages.Length; i++)
        {
            ammoImages[i] = aSlider.gameObject.transform.GetChild(i).gameObject;
        }

        playerCon = currentPlayer.GetComponent<PlayerController>();
        uTurn = currentPlayer.GetComponent<UnitTurn>();
        controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
        controlLaser.gameObject.SetActive(true);
        special = currentPlayer.GetComponent<UnitTurn>().special.GetComponent<ControlLaser>();
        special.gameObject.SetActive(true);

        enemyOrder[0] = enemyTeam[0];
        enemyOrder[1] = enemyTeam[1];
        enemyOrder[2] = enemyTeam[2];
        playerOrder[0] = playerTeam[0];
        playerOrder[1] = playerTeam[1];
        playerOrder[2] = playerTeam[2];

        uTurn.charId = charId;

        playerTeam[charId + 1].GetComponent<UnitTurn>().charId = charId + 1;
        playerTeam[charId + 2].GetComponent<UnitTurn>().charId = charId + 2;

        Setup();
        SetupEnemyHealthBar();

        nextPlayer = playerTeam[charId + 1];
        lastPlayer = playerTeam[charId + 2];
    }

    // Update is called once per frame
    void Update()
    {
        if (masterKey == false)
        {
            if (playerTurn == true)
            {
                PlayerTurn();
            }
            else if (enemyTurn == true)
            {
                EnemyTurn();
            }
        }
        else
        {
            MasterKey();
        }
    }

    void Setup()                                        //Sets up the character
    {
        int deadCount = 0;
        if (playerTurn)
        {

            for (int i = 0; i < playerTeam.Length; i++)
            {
                if (playerTeam[i].GetComponent<UnitTurn>().isDead)
                {
                    deadCount++;
                }
            }
            if (deadCount == playerTeam.Length)
            {
                //End Game
                robotWin = true;
                EndGame();
            }
            else
            {
                deadCount = 0;
            }

            if(currentPlayer.GetComponent<UnitTurn>().specialCount > 0)
            {
                currentPlayer.GetComponent<UnitTurn>().specialCount--;
            }

            SpecialCounter.text = currentPlayer.GetComponent<UnitTurn>().specialCount.ToString();

            camMove.player = currentPlayer;
            camMove.playerCam = currentPlayer.transform.GetChild(5).gameObject;
            playerCon = currentPlayer.GetComponent<PlayerController>();
            uTurn = currentPlayer.GetComponent<UnitTurn>();
            //controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser.gameObject.SetActive(true);
            special = currentPlayer.GetComponent<UnitTurn>().special.GetComponent<ControlLaser>();
            special.gameObject.SetActive(true);
            uTurn.charTurn = playerTurn;
            playerCon.enabled = playerTurn;
            uTurn.enabled = true;
            uTurn.SelectCharacter();
            playerStats = currentPlayer.GetComponent<PlayerStats>();
            characterClass.text = playerStats.charClass;
            characterHealth.text = playerStats.health + "/" + playerStats.maxHealth;
            characterAmmo.text = playerStats.remainingAmmo + "/" + playerStats.ammo;
            SetupPlayerHealthBar();
            SetupPlayerAmmoBar();
        }
        else if (enemyTurn)
        {
            for (int i = 0; i < playerTeam.Length; i++)
            {
                if (playerTeam[i].GetComponent<UnitTurn>().isDead)
                {
                    deadCount++;
                }
            }
            if (deadCount == playerTeam.Length)
            {
                //End Game
                humanWin = true;
                EndGame();
            }
            else
            {
                deadCount = 0;
            }

            if (currentPlayer.GetComponent<UnitTurn>().specialCount > 0)
            {
                currentPlayer.GetComponent<UnitTurn>().specialCount--;
            }

            SpecialCounter.text = currentPlayer.GetComponent<UnitTurn>().specialCount.ToString();

            camMove.player = currentPlayer;
            camMove.playerCam = currentPlayer.transform.GetChild(5).gameObject;
            playerCon = currentPlayer.GetComponent<PlayerController>();
            uTurn = currentPlayer.GetComponent<UnitTurn>();
            //controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser.gameObject.SetActive(true);
            special = currentPlayer.GetComponent<UnitTurn>().special.GetComponent<ControlLaser>();
            special.gameObject.SetActive(true);
            uTurn.charTurn = enemyTurn;
            playerCon.enabled = enemyTurn;
            uTurn.enabled = true;
            uTurn.SelectCharacter();
            enemyStats = currentPlayer.GetComponent<EnemyStats>();
            characterClass.text = enemyStats.charClass;
            characterHealth.text = enemyStats.health + "/" + enemyStats.maxHealth;
            characterAmmo.text = enemyStats.remainingAmmo + "/" + enemyStats.ammo;
            SetupPlayerHealthBar();
            SetupPlayerAmmoBar();
        }
    }

    void GetNextPlayer()                                //Gets the next player in line
    {
        if (playerTurn)
        {
            if (count >= 2)
            {
                count = 0;
                EndPlayerTurn();
            }
            else
            {
                GameObject tempPlayer;
                tempPlayer = currentPlayer;
                currentPlayer = nextPlayer;
                characterName.text = currentPlayer.name;
                Setup();

                playerActions.SetActive(playerTurn);
                moveButton.gameObject.SetActive(playerTurn);

                nextPlayer = lastPlayer;
                lastPlayer = tempPlayer;
                if (count == 2)
                {
                    uTurn.charSelect = false;
                }
                count++;
            }
        }
        else if (enemyTurn)
        {
            if (count >= 2)
            {
                count = 0;
                EndPlayerTurn();
            }
            else
            {
                GameObject tempPlayer;
                tempPlayer = currentPlayer;
                currentPlayer = nextPlayer;
                characterName.text = currentPlayer.name;
                Setup();

                playerActions.SetActive(enemyTurn);
                moveButton.gameObject.SetActive(enemyTurn);

                nextPlayer = lastPlayer;
                lastPlayer = tempPlayer;
                if (count == 2)
                {
                    uTurn.charSelect = false;
                }
                count++;
            }
        }
    }

    public void ChangeCharacters()                              //changes the selected character by moving through the array
    {
        if (nextPlayer.GetComponent<UnitTurn>().charSelect == true && lastPlayer.GetComponent<UnitTurn>().charSelect == true)
        {
            int tempId = uTurn.rayId;

            //DeactivateCharacters();
            currentPlayer = playerTeam[tempId];
            characterName.text = currentPlayer.name;
            if (currentPlayer == playerTeam[charId])
            {
                nextPlayer = playerTeam[charId + 1];
                lastPlayer = playerTeam[charId + 2];
            }
            else if (currentPlayer == playerTeam[charId + 1])
            {
                nextPlayer = playerTeam[charId + 2];
                lastPlayer = playerTeam[charId];
            }
            else if (currentPlayer == playerTeam[charId + 2])
            {
                nextPlayer = playerTeam[charId];
                lastPlayer = playerTeam[charId + 1];
            }
            Setup();
        }
        else if (lastPlayer.GetComponent<UnitTurn>().charSelect == false)
        {
            int tempId = uTurn.rayId;
            nextPlayer = currentPlayer;
            //DeactivateCharacters();
            currentPlayer = playerTeam[tempId];
            characterName.text = currentPlayer.name;
            Setup();
        }
    }

    public void Reselect()
    {
        uTurn.UndoMove();
        camMove.FlipCamera();
        playerActions.SetActive(playerTurn);
        moveButton.gameObject.SetActive(true);
    }

    public void PlayerTurn()
    {
        if (masterKey == false)
        {
            if (enemyTurn == false)
            {
                if (!currentPlayer.GetComponent<UnitTurn>().isDead)
                {
                    pAnim = currentPlayer.gameObject.GetComponent<Animator>();
                    uTurn.enabled = playerTurn;
                    uTurn.charHalo.enabled = playerTurn;
                    playerCon.enabled = playerTurn;
                    if (uTurn.charAction == false)              //if the character can still do an action
                    {
                        playerActions.SetActive(playerTurn);
                        moveButton.gameObject.SetActive(false);
                        playerCon.trigger = false;
                        uTurn.charAction = true;
                    }
                    else if (uTurn.charTurn == false)           //if a character used their turn moving
                    {
                        uTurn.TurnOver();
                        camMove.FlipCamera();
                        uTurn.enabled = false;
                        playerCon.trigger = false;
                        playerCon.enabled = false;
                        GetNextPlayer();
                    }
                }
                else if (currentPlayer.GetComponent<UnitTurn>().isDead)
                {
                    //Debug.Log("Test");
                    GetNextPlayer();
                }
            }
        }
        else
        {
            MasterKey();
        }
    }

    public void EndPlayerTurn()                                 //resets the playerTeam to prepare for the next turn
    {
        playerTeam = enemyTeam;
        enemyTeam = nextTeam;
        nextTeam = playerTeam;
        camHolder = enemyHolder;
        enemyHolder = nextCam;
        nextCam = camHolder;

        for (int i = 0; i < 3; i++)
        {
            currentPlayer = playerTeam[i];
            uTurn = currentPlayer.GetComponent<UnitTurn>();
            //controlLaser = uTurn.gun.GetComponent<ControlLaser>();
            playerCon = currentPlayer.GetComponent<PlayerController>();
            uTurn.NewTurn();
        }

        if (playerTurn)
        {
            currentPlayer = playerTeam[charId];
            camMove = camHolder.GetComponent<CameraMove>();
            characterName.text = currentPlayer.name;
            camMove.player = currentPlayer;
            camMove.playerCam = currentPlayer.transform.GetChild(5).gameObject;
            uTurn = currentPlayer.GetComponent<UnitTurn>();
            //controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser.gameObject.SetActive(true);
            special = currentPlayer.GetComponent<UnitTurn>().special.GetComponent<ControlLaser>();
            special.gameObject.SetActive(true);
            playerCon = currentPlayer.GetComponent<PlayerController>();
            enemyStats = currentPlayer.GetComponent<EnemyStats>();
            characterClass.text = enemyStats.charClass;
            characterHealth.text = enemyStats.health + "/" + enemyStats.maxHealth;
            characterAmmo.text = enemyStats.remainingAmmo + "/" + enemyStats.ammo;
            nextPlayer = playerTeam[charId + 1];
            lastPlayer = playerTeam[charId + 2];
            enemyHolder.SetActive(false);
            camHolder.SetActive(true);
            //playerTurn = false;
            playerActions.SetActive(playerTurn);
            moveButton.gameObject.SetActive(playerTurn);
            enemyTurn = true;
            playerTurn = false;
            SetupPlayerHealthBar();
            SetupPlayerAmmoBar();
        }
        else if (enemyTurn)
        {
            currentPlayer = playerTeam[charId];
            camMove = camHolder.GetComponent<CameraMove>();
            characterName.text = currentPlayer.name;
            camMove.player = currentPlayer;
            camMove.playerCam = currentPlayer.transform.GetChild(5).gameObject;
            uTurn = currentPlayer.GetComponent<UnitTurn>();
            //controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser = currentPlayer.GetComponent<UnitTurn>().gun.GetComponent<ControlLaser>();
            controlLaser.gameObject.SetActive(true);
            special = currentPlayer.GetComponent<UnitTurn>().special.GetComponent<ControlLaser>();
            special.gameObject.SetActive(true);
            playerCon = currentPlayer.GetComponent<PlayerController>();
            playerStats = currentPlayer.GetComponent<PlayerStats>();
            characterClass.text = playerStats.charClass;
            characterHealth.text = playerStats.health + "/" + playerStats.maxHealth;
            characterAmmo.text = playerStats.remainingAmmo + "/" + playerStats.ammo;
            nextPlayer = playerTeam[charId + 1];
            lastPlayer = playerTeam[charId + 2];
            enemyHolder.SetActive(false);
            camHolder.SetActive(true);
            playerActions.SetActive(enemyTurn);
            moveButton.gameObject.SetActive(enemyTurn);
            playerTurn = true;
            enemyTurn = false;
            SetupPlayerHealthBar();
            SetupPlayerAmmoBar();
        }
        turnCount++;

        //testing both teams on one computer ****doesn't work currently due to playerCam
        //playerTeam = enemyTeam;
        //enemyTeam = nextTeam;
        //nextTeam = playerTeam;
        //camHolder = enemyHolder;
        //enemyHolder = nextCam;
        //nextCam = camHolder;
        /*for (int i = 0; i < 3; i++)
        {
            currentPlayer = playerTeam[i];
            playerCon = currentPlayer.GetComponent<PlayerController>();
            uTurn.NewTurn();
        }*/
        //currentPlayer = playerTeam[charId];
        //camMove = camHolder.GetComponent<CameraMove>();
        //enemyHolder.SetActive(false);
        //camHolder.SetActive(true);
        //camMove.player = currentPlayer;
        //playerCon = currentPlayer.GetComponent<PlayerController>();
        //playerStats = currentPlayer.GetComponent<PlayerStats>();
        //nextPlayer = playerTeam[charId + 1];
        //lastPlayer = playerTeam[charId + 2];
        //playerTurn = false;
        //playerActions.SetActive(playerTurn);
        //moveButton.gameObject.SetActive(playerTurn);
        //
    }

    public void EnemyTurn()                                 //about to work on this, this is where the enemy ai will go
    {
        //enemyTurn = false;
        //playerTurn = true;
        //EndEnemyTurn();

        if (masterKey == false)
        {
            if (playerTurn == false)
            {
                if (currentPlayer.GetComponent<UnitTurn>().isDead == false)
                {
                    pAnim = currentPlayer.gameObject.GetComponent<Animator>();
                    uTurn.enabled = enemyTurn;
                    uTurn.charHalo.enabled = enemyTurn;
                    playerCon.enabled = enemyTurn;
                    if (uTurn.charAction == false)              //if the character can still do an action
                    {
                        playerActions.SetActive(enemyTurn);
                        moveButton.gameObject.SetActive(false);
                        playerCon.trigger = false;
                        uTurn.charAction = true;
                    }
                    else if (uTurn.charTurn == false)           //if a character used their turn moving
                    {
                        uTurn.TurnOver();
                        camMove.FlipCamera();
                        uTurn.enabled = false;
                        playerCon.trigger = false;
                        playerCon.enabled = false;
                        GetNextPlayer();
                    }
                }
                else if (currentPlayer.GetComponent<UnitTurn>().isDead == true)
                {
                    GetNextPlayer();
                }

            }
        }
        else
        {
            MasterKey();
        }
    }

    public void EndEnemyTurn()                              //this will setup the enemyteam for the next turn, currently just enables the players to go again
    {
        //playerActions.SetActive(playerTurn);
        //moveButton.gameObject.SetActive(playerTurn);
    }

    public void ActivateCharacters()                            //Activates a character to move
    {
        if (masterKey == false)
        {
            uTurn.enabled = true;
            camMove.Move();
            uTurn.MoveActive();
        }
    }

    public void DeactivateCharacters()                          //Deactivates a character
    {
        //use this for dead players
        //uTurn.enabled = false;
        //playerCon.enabled = false;
    }

    public void EndTurn()                                       //End Turn button
    {
        uTurn.TurnOver();
        //camMove.FlipCamera();
        playerCon.trigger = false;
        playerCon.enabled = false;
        controlLaser.gameObject.SetActive(false);
        special.gameObject.SetActive(false);
        GetNextPlayer();
    }

    public void ReloadTurn()
    {
        if (playerTurn)
        {
            if (playerStats.remainingAmmo == playerStats.ammo)
            {
                //Debug.Log("Your ammo is already full");
                BattleLog.text = "Your ammo is already full";
            }
            else
            {
                playerStats.ReloadGun();
                EndTurn();
            }
        }
        else if (enemyTurn)
        {
            if (enemyStats.remainingAmmo == enemyStats.ammo)
            {
                //Debug.Log("Your ammo is already full");
                BattleLog.text = "Your ammo is already full";
            }
            else
            {
                enemyStats.ReloadGun();
                //Debug.Log("Reloaded");
                EndTurn();
            }
        }
    }

    public void CancelAttack()
    {
        if (playerTurn)
        {
            enemyDisplay.SetActive(false);
            enemyCam[eCount].SetActive(false);
            if (camMove.freeMove == true)
            {
                camHolder.SetActive(true);
            }
            else
            {
                camMove.FlipCamera();
                camHolder.SetActive(true);
            }
            playerActions.SetActive(true);
        }
        else if (enemyTurn)
        {
            camHolder.SetActive(true);
            enemyDisplay.SetActive(false);
            aCam[eCount].SetActive(false);
            playerActions.SetActive(true);
        }
    }

    public void AttackTurn()                                        //Attack button
    {
        if (playerTurn)
        {
            if (playerStats.remainingAmmo > 0)
            {
                uTurn.FindEnemies();
            }
            else
            {
                //Play sound indicating wrong
                BattleLog.text = "You have no ammo left!";
            }
        }
        else if (enemyTurn)
        {
            if (enemyStats.remainingAmmo > 0)
            {
                uTurn.FindEnemies();
            }
            else
            {
                //Play sound indicating wrong
                BattleLog.text = "You have no ammo left!";
            }
        }
    }

    public void AttackEnemy()
    {
        if (playerTurn)
        {
            if (enemyTeam[0].GetComponent<UnitTurn>().isDead)
            {
                //Debug.Log("This Enemy is Dead");
                BattleLog.text = enemyTeam[0] + " is dead";
            }
            else
            {
                int tempAcc = playerStats.CalculateHit(enemyTeam[0].transform.position);
                DecreaseEnemyHealthBar(playerStats.AttackDamage(tempAcc));
                if (enemyTeam[0].GetComponent<EnemyStats>().health <= 0)
                {
                    enemyTeam[0].GetComponent<EnemyStats>().health = 0;
                    eAnim.CrossFade("Dead", 0.2f);
                    enemyTeam[0].GetComponent<UnitTurn>().isDead = true;
                }
                for (int i = 0; i < enemyCam.Length; i++)
                {
                    enemyCam[i].SetActive(false);
                    enemyTeam[i] = enemyOrder[i];
                }
                enemyDisplay.SetActive(false);
                camHolder.SetActive(true);
                StartCoroutine(WaitAFuckingSecond());
                //else
                //{
                    //EndTurn();
                //}
            }
        }
        else if (enemyTurn)
        {
            if (enemyTeam[0].GetComponent<UnitTurn>().isDead)
            {
                //Debug.Log("This Enemy is Dead");
                BattleLog.text = enemyTeam[0] + " is dead";
            }
            else
            {
                int tempAcc = enemyStats.CalculateHit(enemyTeam[0].transform.position);
                DecreaseEnemyHealthBar(enemyStats.AttackDamage(tempAcc));
                if (enemyTeam[0].GetComponent<PlayerStats>().health <= 0)
                {
                    enemyTeam[0].GetComponent<PlayerStats>().health = 0;
                    eAnim.CrossFade("Dead", 0.2f);
                    enemyTeam[0].GetComponent<UnitTurn>().isDead = true;
                }
                for (int i = 0; i < aCam.Length; i++)
                {
                    aCam[i].SetActive(false);
                    enemyTeam[i] = playerOrder[i];
                }
                enemyDisplay.SetActive(false);
                camHolder.SetActive(true);
                StartCoroutine(WaitAFuckingSecond());
                //EndTurn();
            }
        }
    }

    public void CycleThroughEnemies()      //receives the enemies that are close to the player
    {

        if (playerTurn)
        {
            playerActions.SetActive(false);
            if (camHolder.GetComponent<CameraMove>().freeMove == true)
            {
                camHolder.SetActive(false);
            }
            else
            {
                camHolder.GetComponent<CameraMove>().FlipCamera();
                camHolder.SetActive(false);
            }

            eCount = 0;
            enemyDisplay.SetActive(true);
            enemyCam[eCount].SetActive(true);
            SetupEnemyHealthBar();
            enemyName.text = enemyTeam[eCount].gameObject.name;
            enemyHealth.text = enemyTeam[eCount].GetComponent<EnemyStats>().health + "/" + enemyTeam[eCount].GetComponent<EnemyStats>().maxHealth;
            eAnim = enemyTeam[eCount].gameObject.GetComponent<Animator>();
        }
        else if (enemyTurn)
        {
            playerActions.SetActive(false);
            if (camHolder.GetComponent<CameraMove>().freeMove == true)
            {
                camHolder.SetActive(false);
            }
            else
            {
                camHolder.GetComponent<CameraMove>().FlipCamera();
                camHolder.SetActive(false);
            }

            eCount = 0;
            enemyDisplay.SetActive(true);
            aCam[eCount].SetActive(true);
            SetupEnemyHealthBar();
            enemyName.text = enemyTeam[eCount].gameObject.name;
            enemyHealth.text = enemyTeam[eCount].GetComponent<PlayerStats>().health + "/" + enemyTeam[eCount].GetComponent<PlayerStats>().maxHealth;
            eAnim = enemyTeam[eCount].gameObject.GetComponent<Animator>();
        }
    }

    public void PreviousEnemy()                                             //cycles to the last nearEnemy in the array
    {
        if (playerTurn)
        {
            enemyCam[eCount].SetActive(false);
            enemyCam[enemyTeam[charId + 2].gameObject.GetComponent<UnitTurn>().charId].SetActive(true);
            RotateEnemyTeam(2);
        }
        else if (enemyTurn)
        {
            aCam[eCount].SetActive(false);
            aCam[enemyTeam[charId + 2].gameObject.GetComponent<UnitTurn>().charId].SetActive(true);
            RotateEnemyTeam(2);
        }
    }

    public void NextEnemy()                                                 //cycles to the next nearEnemy in the array
    {
        if (playerTurn)
        {
            enemyCam[eCount].SetActive(false);
            enemyCam[enemyTeam[charId + 1].GetComponent<UnitTurn>().charId].SetActive(true);
            RotateEnemyTeam(1);
        }
        else if (enemyTurn)
        {
            aCam[eCount].SetActive(false);
            aCam[enemyTeam[charId + 1].gameObject.GetComponent<UnitTurn>().charId].SetActive(true);
            RotateEnemyTeam(1);
        }
    }

    void RotateEnemyTeam(int dummy)                                 //shifts the nearEnemy array
    {
        if (playerTurn)
        {
            if (dummy == 2)
            {
                GameObject[] dummyTemp = new GameObject[enemyTeam.Length];
                dummyTemp[0] = enemyTeam[dummy];
                dummyTemp[1] = enemyTeam[0];
                dummyTemp[2] = enemyTeam[dummy - 1];
                for (int i = 0; i < enemyTeam.Length; i++)
                {
                    enemyTeam[i] = dummyTemp[i];
                }
                eCount = enemyTeam[0].gameObject.GetComponent<UnitTurn>().charId;
                enemyName.text = enemyTeam[0].gameObject.name;
                enemyHealth.text = enemyTeam[0].GetComponent<EnemyStats>().health + "/" + enemyTeam[0].GetComponent<EnemyStats>().maxHealth;
                eAnim = enemyTeam[0].gameObject.GetComponent<Animator>();
                SetupEnemyHealthBar();
            }
            else if (dummy == 1)
            {
                GameObject[] dummyTemp = new GameObject[enemyTeam.Length];
                dummyTemp[0] = enemyTeam[dummy];
                dummyTemp[1] = enemyTeam[dummy + 1];
                dummyTemp[2] = enemyTeam[0];

                for (int i = 0; i < enemyTeam.Length; i++)
                {
                    enemyTeam[i] = dummyTemp[i];
                }
                eCount = enemyTeam[0].gameObject.GetComponent<UnitTurn>().charId;
                enemyName.text = enemyTeam[0].gameObject.name;
                enemyHealth.text = enemyTeam[0].GetComponent<EnemyStats>().health + "/" + enemyTeam[0].GetComponent<EnemyStats>().maxHealth;
                eAnim = enemyTeam[0].gameObject.GetComponent<Animator>();
                SetupEnemyHealthBar();
            }
        }
        else if (enemyTurn)
        {
            if (dummy == 2)
            {
                GameObject[] dummyTemp = new GameObject[enemyTeam.Length];
                dummyTemp[0] = enemyTeam[dummy];
                dummyTemp[1] = enemyTeam[0];
                dummyTemp[2] = enemyTeam[dummy - 1];
                for (int i = 0; i < enemyTeam.Length; i++)
                {
                    enemyTeam[i] = dummyTemp[i];
                }
                eCount = enemyTeam[0].gameObject.GetComponent<UnitTurn>().charId;
                enemyName.text = enemyTeam[0].gameObject.name;
                enemyHealth.text = enemyTeam[0].GetComponent<PlayerStats>().health + "/" + enemyTeam[0].GetComponent<PlayerStats>().maxHealth;
                eAnim = enemyTeam[0].gameObject.GetComponent<Animator>();
                SetupEnemyHealthBar();
            }
            else if (dummy == 1)
            {
                GameObject[] dummyTemp = new GameObject[enemyTeam.Length];
                dummyTemp[0] = enemyTeam[dummy];
                dummyTemp[1] = enemyTeam[dummy + 1];
                dummyTemp[2] = enemyTeam[0];

                for (int i = 0; i < enemyTeam.Length; i++)
                {
                    enemyTeam[i] = dummyTemp[i];
                }
                eCount = enemyTeam[0].gameObject.GetComponent<UnitTurn>().charId;
                enemyName.text = enemyTeam[0].gameObject.name;
                enemyHealth.text = enemyTeam[0].GetComponent<PlayerStats>().health + "/" + enemyTeam[0].GetComponent<PlayerStats>().maxHealth;
                eAnim = enemyTeam[0].gameObject.GetComponent<Animator>();
                SetupEnemyHealthBar();
            }
        }
    }

    public void SetupPlayerHealthBar()
    {
        if (playerTurn)
        {
            int temp = playerStats.health;
            int temp1 = temp / 5;

            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
            for (int i = 0; i < temp1; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = healthBar.sprite;
            }
        }
        else if (enemyTurn)
        {
            int temp = enemyStats.health;
            int temp1 = temp / 5;

            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
            for (int i = 0; i < temp1; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = healthBar.sprite;
            }
        }
    }

    public void DecreasePlayerHealthBar(int damage)
    {
        if (playerTurn)
        {
            int temp = playerStats.TakeDamage(damage);
            int temp1 = temp / 5;
            int temp2 = healthImages.Length - temp1;

            for (int i = 0; i < temp1; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = healthBar.sprite;
            }
            for (int i = healthImages.Length - temp2; i < healthImages.Length; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
        }
        else if (enemyTurn)
        {
            int temp = enemyStats.TakeDamage(damage);
            int temp1 = temp / 5;
            int temp2 = healthImages.Length - temp1;

            for (int i = 0; i < temp1; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = healthBar.sprite;
            }
            for (int i = healthImages.Length - temp2; i < healthImages.Length; i++)
            {
                healthImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
        }
    }

    public void SetupPlayerAmmoBar()
    {
        if (playerTurn)
        {
            int temp = playerStats.remainingAmmo;
            if (temp != 0)
            {
                int temp1 = temp / playerStats.fireRate; //4/1=4
                int temp2 = ammoImages.Length / temp1; //12/4 = 3

                for (int i = 0; i < ammoImages.Length; i++)
                {
                    ammoImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
                }
                for (int i = 0; i < playerStats.remainingAmmo; i++)
                {
                    ammoImages[i].GetComponent<Image>().sprite = ammoBar.sprite;
                }
            }
        }
        else if (enemyTurn)
        {
            int temp = enemyStats.remainingAmmo;
            if (temp != 0)
            {
                int temp1 = temp / enemyStats.fireRate; //4/1=4
                int temp2 = ammoImages.Length / temp1; //12/4 = 3

                for (int i = 0; i < ammoImages.Length; i++)
                {
                    ammoImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
                }
                for (int i = 0; i < enemyStats.remainingAmmo; i++)
                {
                    ammoImages[i].GetComponent<Image>().sprite = ammoBar.sprite;
                }
            }
        }

    }

    public void DecreasePlayerAmmoBar()
    {
        if (playerTurn)
        {
            int temp = playerStats.FireGun();

            for (int i = 0; i < temp; i++)
            {
                ammoImages[i].GetComponent<Image>().sprite = ammoBar.sprite;
            }
            for (int i = temp; i < ammoImages.Length; i++)
            {
                ammoImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
        }
        else if (enemyTurn)
        {
            int temp = enemyStats.FireGun();

            for (int i = 0; i < temp; i++)
            {
                ammoImages[i].GetComponent<Image>().sprite = ammoBar.sprite;
            }
            for (int i = temp; i < ammoImages.Length; i++)
            {
                ammoImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
        }
    }

    public void SetupEnemyHealthBar()
    {
        if (playerTurn)
        {
            int temp = enemyTeam[0].GetComponent<EnemyStats>().health;
            int temp1 = temp / 5;

            for (int i = 0; i < enemyImages.Length; i++)
            {
                enemyImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
            for (int i = 0; i < temp1; i++)
            {
                enemyImages[i].GetComponent<Image>().sprite = enemyBar.sprite;
            }
        }
        else if (enemyTurn)
        {
            int temp = enemyTeam[0].GetComponent<PlayerStats>().health;
            int temp1 = temp / 5;

            for (int i = 0; i < enemyImages.Length; i++)
            {
                enemyImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
            }
            for (int i = 0; i < temp1; i++)
            {
                enemyImages[i].GetComponent<Image>().sprite = enemyBar.sprite;
            }
        }
    }

    public void DecreaseEnemyHealthBar(int damage)
    {
        if (playerTurn)
        {
            if (!special)
            {
                controlLaser.hitSpot = enemyTeam[0];
                special.hitSpot = enemyTeam[0];
                int temp = enemyTeam[0].GetComponent<EnemyStats>().TakeDamage(damage);
                int temp1 = temp / 5;
                int temp2 = enemyImages.Length - temp1;

                for (int i = 0; i < temp1; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = enemyBar.sprite;
                }
                for (int i = enemyImages.Length - temp2; i < enemyImages.Length; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
                }
                if (damage > 0)
                {
                    pAnim.SetBool("Aim", true);
                    StartCoroutine(RotateToFaceEnemy(enemyTeam[0].transform.position));
                    //pAnim.SetBool("Aim", true);
                    //eAnim.CrossFade("damage", 0.2f);
                    //pAnim.SetBool("Aim", false);
                    DecreasePlayerAmmoBar();
                    BattleLog.text = currentPlayer.name + " did " + damage + " damage to " + enemyTeam[0].name;
                }
                else
                {
                    BattleLog.text = currentPlayer.name + " did not hit " + enemyTeam[0].name;
                    DecreasePlayerAmmoBar();
                }
            }
            else
            {
                controlLaser.hitSpot = enemyTeam[0];
                special.hitSpot = enemyTeam[0];
                int temp = enemyTeam[0].GetComponent<EnemyStats>().TakeDamage(currentPlayer.GetComponent<PlayerStats>().weaponDamage * 2);
                int temp1 = temp / 5;
                int temp2 = enemyImages.Length - temp1;

                for (int i = 0; i < temp1; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = enemyBar.sprite;
                }
                for (int i = enemyImages.Length - temp2; i < enemyImages.Length; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
                }
                if (damage > 0)
                {
                    pAnim.SetBool("Aim", true);
                    StartCoroutine(RotateToFaceEnemy(enemyTeam[0].transform.position));
                    //pAnim.SetBool("Aim", true);
                    //eAnim.CrossFade("damage", 0.2f);
                    //pAnim.SetBool("Aim", false);
                    DecreasePlayerAmmoBar();
                    BattleLog.text = currentPlayer.name + " did " + damage + " damage to " + enemyTeam[0].name;
                }
                else
                {
                    BattleLog.text = currentPlayer.name + " did not hit " + enemyTeam[0].name;
                    DecreasePlayerAmmoBar();
                }
            }
        }
        else if (enemyTurn)
        {
            if (!special)
            {
                controlLaser.hitSpot = enemyTeam[0];
                special.hitSpot = enemyTeam[0];
                int temp = enemyTeam[0].GetComponent<PlayerStats>().TakeDamage(damage);
                int temp1 = temp / 5;
                int temp2 = enemyImages.Length - temp1;

                for (int i = 0; i < temp1; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = enemyBar.sprite;
                }
                for (int i = enemyImages.Length - temp2; i < enemyImages.Length; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
                }
                if (damage > 0)
                {
                    pAnim.SetBool("Aim", true);
                    StartCoroutine(RotateToFaceEnemy(enemyTeam[0].transform.position));
                    //eAnim.CrossFade("damage", 0.2f);
                    DecreasePlayerAmmoBar();
                    BattleLog.text = currentPlayer.name + " did " + damage + " damage to " + enemyTeam[0].name;
                }
                else
                {
                    BattleLog.text = currentPlayer.name + " did not hit " + enemyTeam[0].name;
                    DecreasePlayerAmmoBar();
                }
            }
            else
            {
                
                controlLaser.hitSpot = enemyTeam[0];
                special.hitSpot = enemyTeam[0];
                int temp = enemyTeam[0].GetComponent<PlayerStats>().TakeDamage(currentPlayer.GetComponent<EnemyStats>().weaponDamage * 2);
                int temp1 = temp / 5;
                int temp2 = enemyImages.Length - temp1;

                for (int i = 0; i < temp1; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = enemyBar.sprite;
                }
                for (int i = enemyImages.Length - temp2; i < enemyImages.Length; i++)
                {
                    enemyImages[i].GetComponent<Image>().sprite = emptyBar.sprite;
                }
                if (damage > 0)
                {
                    pAnim.SetBool("Aim", true);
                    StartCoroutine(RotateToFaceEnemy(enemyTeam[0].transform.position));
                    //eAnim.CrossFade("damage", 0.2f);
                    DecreasePlayerAmmoBar();
                    BattleLog.text = currentPlayer.name + " did " + damage + " damage to " + enemyTeam[0].name;
                }
                else
                {
                    BattleLog.text = currentPlayer.name + " did not hit " + enemyTeam[0].name;
                    DecreasePlayerAmmoBar();
                }
            }
        }
    }

    IEnumerator WaitAFuckingSecond()
    {
        yield return new WaitForSeconds(3);
        pAnim.SetBool("Aim", false);
        specialMove = false;
        EndTurn();
    }

    IEnumerator RotateToFaceEnemy(Vector3 targetDir/*, bool aimAtTheEnd = false*/)
    {
        Quaternion startRot = currentPlayer.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(enemyTeam[0].transform.position);
        float t = 0;


        while (t < 1)
        {
            t += Time.deltaTime * 3;

            //playerTeam[0].transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            //currentPlayer.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            currentPlayer.transform.LookAt(targetDir);
            yield return null;
        }
        if (specialMove)
        {
            special.FireLaser();
        }
        else
        {
            controlLaser.FireLaser();
        }
        eAnim.CrossFade("damage", 0.2f);
        //pAnim.SetBool("Aim", false);
            //yield return null;
    }

    public void MasterKey()
    {
        currentPlayer = playerTeam[0];
        uTurn.MoveActive();
        playerActions.SetActive(false);
    }

    public void QuickKill()
    {
        if (currentPlayer.GetComponent<UnitTurn>().specialCount == 0)
        {
            specialMove = true;
            currentPlayer.GetComponent<UnitTurn>().specialCount = 2;
            AttackEnemy();
        }
        else
        {
            BattleLog.text = "You can't use your special right now";
        }
        /*enemyTeam[0].GetComponent<EnemyStats>().TakeDamage(105);
        if (enemyTeam[0].GetComponent<EnemyStats>().health <= 0)
        {
            enemyTeam[0].GetComponent<EnemyStats>().health = 0;
            eAnim.CrossFade("Dead", 0.2f);
            enemyTeam[0].GetComponent<UnitTurn>().isDead = "Yes";
        }*/
    }

    public void ActivateMasterKey()
    {
        if (masterKey)
        {
            masterKey = false;
        }
        else if (!masterKey)
        {
            masterKey = true;
        }
    }

    public void EndGame()
    {
        gameOver = true;
        playerActions.SetActive(false);

        if (robotWin)
        {
            RobotWin.SetActive(true);
        }
        else if (humanWin)
        {
            HumanWin.SetActive(true);
        }
        gameOverCanvas.SetActive(true);
    }

}
