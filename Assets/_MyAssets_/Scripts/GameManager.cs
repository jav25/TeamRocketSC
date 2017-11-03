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

    private int eCount;                                             //holds the element for an enemy in an enemy array
    private static int charId = 0;                                  //holds the id of the currently selected character
    private int count = 0;                                          //this counts how many characters have ended their turn during this turn
    public int turnCount = 0;                                       //just a counter for the turn
    private GameObject nextCam;                                     //used for controlling both teams on one computer
    private GameObject nextPlayer;
    private GameObject lastPlayer;
    private GameObject[] nextTeam = new GameObject[3];              //used for holding the next team up

    public GameObject[] playerTeam = new GameObject[3];             //an array of player characters
    public GameObject[] enemyTeam = new GameObject[3];              //an array of enemy characters, used for controlling both teams on one computer
    public GameObject currentPlayer;                                //current player in the array
    public GameObject camHolder;                                    //Camera Holder object
    public GameObject enemyHolder;                                  //Holds the enemy Camera Holder, used for controlling both teams on one computer
    public GameObject playerActions;                                //PlayerActions Hud

    public Slider slider;                                           //displays enemy health

    public Button moveButton;

    public GameObject[] enemyCam = new GameObject[3];               //used for cycling between which enemy to attack

    public GameObject[] tempTeam;                                   //holds the enemies when the attack button is pressed

    public GameObject enemyDisplay;                                 //HUD for cycling between which enemy to attack

    public bool playerTurn;
    public bool enemyTurn;

    public bool masterKey;                                          //use if you want to move the same character over and over

    // Use this for initialization
    void Start()
    {
        playerTurn = true;
        enemyTurn = false;

        camMove = camHolder.GetComponent<CameraMove>();

        currentPlayer = playerTeam[charId];

        //testing both teams on one computer
        //nextTeam = playerTeam;
        //nextCam = camHolder;
        //

        slider.maxValue = enemyTeam[0].GetComponent<EnemyStats>().maxHealth;
        slider.value = enemyTeam[0].GetComponent<EnemyStats>().health;

        playerStats = currentPlayer.GetComponent<PlayerStats>();

        playerCon = currentPlayer.GetComponent<PlayerController>();
        uTurn = currentPlayer.GetComponent<UnitTurn>();

        uTurn.charId = charId;

        playerTeam[charId + 1].GetComponent<UnitTurn>().charId = charId + 1;
        playerTeam[charId + 2].GetComponent<UnitTurn>().charId = charId + 2;

        Setup();

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
        camMove.player = currentPlayer;
        camMove.playerCam = currentPlayer.transform.GetChild(5).gameObject;
        playerCon = currentPlayer.GetComponent<PlayerController>();
        uTurn = currentPlayer.GetComponent<UnitTurn>();
        uTurn.charTurn = playerTurn;
        playerCon.enabled = playerTurn;
        uTurn.enabled = true;
        uTurn.SelectCharacter();
        playerStats = currentPlayer.GetComponent<PlayerStats>();
    }

    void GetNextPlayer()                                //Gets the next player in line
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

    public void ChangeCharacters()                              //changes the selected character by moving through the array
    {
        if (nextPlayer.GetComponent<UnitTurn>().charSelect == true && lastPlayer.GetComponent<UnitTurn>().charSelect == true)
        {
            int tempId = uTurn.rayId;

            //DeactivateCharacters();
            currentPlayer = playerTeam[tempId];
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
        }
        else
        {
            MasterKey();
        }
    }

    public void EndPlayerTurn()                                 //resets the playerTeam to prepare for the next turn
    {
        for (int i = 0; i < 3; i++)
        {
            currentPlayer = playerTeam[i];
            uTurn = currentPlayer.GetComponent<UnitTurn>();
            playerCon = currentPlayer.GetComponent<PlayerController>();
            uTurn.NewTurn();
        }

        currentPlayer = playerTeam[charId];
        camMove.player = currentPlayer;
        camMove.playerCam = currentPlayer.transform.GetChild(5).gameObject;
        uTurn = currentPlayer.GetComponent<UnitTurn>();
        playerCon = currentPlayer.GetComponent<PlayerController>();
        playerStats = currentPlayer.GetComponent<PlayerStats>();
        nextPlayer = playerTeam[charId + 1];
        lastPlayer = playerTeam[charId + 2];
        playerTurn = false;
        playerActions.SetActive(playerTurn);
        moveButton.gameObject.SetActive(playerTurn);

        turnCount++;

        //testing both teams on one computer ****doesn't work currently due to playerCam
        //playerTeam = enemyTeam;
        //enemyTeam = nextTeam;
        //nextTeam = playerTeam;
        //camHolder = enemyHolder;
        //enemyHolder = nextCam;
        //nextCam = camHolder;
        //for (int i = 0; i < 3; i++)
        //{
        //    currentPlayer = playerTeam[i];
        //    playerCon = currentPlayer.GetComponent<PlayerController>();
        //    playerCon.NewTurn();
        //}
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

        enemyTurn = true;
    }

    public void EnemyTurn()                                 //about to work on this, this is where the enemy ai will go
    {
        enemyTurn = false;
        playerTurn = true;
        EndEnemyTurn();
    }

    public void EndEnemyTurn()                              //this will setup the enemyteam for the next turn, currently just enables the players to go again
    {
        playerActions.SetActive(playerTurn);
        moveButton.gameObject.SetActive(playerTurn);
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
        camMove.FlipCamera();
        playerCon.trigger = false;
        playerCon.enabled = false;
        GetNextPlayer();
    }

    public void AttackTurn()                                        //Attack button
    {
        //need to flip off all the other switches so that the player can only attack
        uTurn.FindEnemies();
    }

    public void AttackEnemy()
    {
        tempTeam[0].GetComponent<EnemyStats>().TakeDamage(playerStats.AttackDamage());
        slider.value = tempTeam[0].GetComponent<EnemyStats>().health;
        for(int i = 0; i < enemyCam.Length; i++)
        {
            enemyCam[i].SetActive(false);
        }
        enemyDisplay.SetActive(false);
        camHolder.SetActive(true);
        EndTurn();
    }

    public void CycleThroughEnemies(GameObject[] Team, int index)      //receives the enemies that are close to the player
    {
        playerActions.SetActive(false);
        if(camHolder.GetComponent<CameraMove>().freeMove == true)
        {
            camHolder.SetActive(false);
        }
        else
        {
            camHolder.GetComponent<CameraMove>().FlipCamera();
            camHolder.SetActive(false);
        }
        enemyDisplay.SetActive(true);
        enemyCam[index].SetActive(true);
        eCount = index;
        tempTeam = Team;
        slider.maxValue = tempTeam[index].GetComponent<EnemyStats>().maxHealth;
        slider.value = enemyTeam[index].GetComponent<EnemyStats>().health;
    }

    public void PreviousEnemy()                                             //cycles to the last nearEnemy in the array
    {
        //int temp = tempTeam[charId];
        enemyCam[eCount].SetActive(false);
        enemyCam[tempTeam[charId + 2].gameObject.GetComponent<UnitTurn>().charId].SetActive(true);
        //slider.maxValue = tempTeam[charId + 2].GetComponent<EnemyStats>().maxHealth;
        //slider.value = enemyTeam[charId + 2].GetComponent<EnemyStats>().health;
        RotateEnemyTeam(2);
    }

    public void NextEnemy()                                                 //cycles to the next nearEnemy in the array
    {
        enemyCam[eCount].SetActive(false);
        enemyCam[tempTeam[charId + 1].gameObject.GetComponent<UnitTurn>().charId].SetActive(true);
        //slider.maxValue = tempTeam[charId + 1].GetComponent<EnemyStats>().maxHealth;
        //slider.value = enemyTeam[charId + 1].GetComponent<EnemyStats>().health;
        RotateEnemyTeam(1);
    }

    void RotateEnemyTeam(int dummy)                                 //shifts the nearEnemy array
    {
        
        if (dummy == 2)
        {
            GameObject[] dummyTemp = new GameObject[tempTeam.Length];
            dummyTemp[0] = tempTeam[dummy];
            dummyTemp[1] = tempTeam[0];
            dummyTemp[2] = tempTeam[dummy - 1];
            for (int i = 0; i < tempTeam.Length; i++)
            {
                tempTeam[i] = dummyTemp[i];
            }
            eCount = tempTeam[0].gameObject.GetComponent<UnitTurn>().charId;
            slider.maxValue = tempTeam[0].GetComponent<EnemyStats>().maxHealth;
            slider.value = tempTeam[0].GetComponent<EnemyStats>().health;
        }
        else if(dummy == 1)
        {
            GameObject[] dummyTemp = new GameObject[tempTeam.Length];
            dummyTemp[0] = tempTeam[dummy];
            dummyTemp[1] = tempTeam[dummy + 1];
            dummyTemp[2] = tempTeam[0];
            
            for (int i = 0; i < dummyTemp.Length; i++)
            {
                tempTeam[i] = dummyTemp[i];
            }
            eCount = tempTeam[0].gameObject.GetComponent<UnitTurn>().charId;
            slider.maxValue = tempTeam[0].GetComponent<EnemyStats>().maxHealth;
            slider.value = tempTeam[0].GetComponent<EnemyStats>().health;
        }
    }

    public void MasterKey()
    {
        currentPlayer = playerTeam[0];
        uTurn.MoveActive();
        playerActions.SetActive(false);
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

}
