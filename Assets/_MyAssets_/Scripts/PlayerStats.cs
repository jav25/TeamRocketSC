using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{ 
    /**************************************************************************************************
     * This Class handles the stats of each individual character.
     **************************************************************************************************/

    private GameManager gameManager;
    private PlayerStats playerStats;

    private GameObject closest;                             //the closest enemy to the player
    private Vector3 tempV;

    public int maxHealth = 100;
    public int health;
    public int attack = 10;

    public GameObject gm;

    public bool enemiesNear;

    public GameObject[] enemy = new GameObject[3];          //an array with the nearest enemies

    public int temp = 0;


    // Use this for initialization
    void Start()
    {
        gameManager = gm.GetComponent<GameManager>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        GetHealth();
    }

    int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

}
