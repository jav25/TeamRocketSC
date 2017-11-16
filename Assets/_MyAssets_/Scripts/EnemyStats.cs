using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    /**************************************************************************************************
     * This Class handles the stats of each individual enemy.
     **************************************************************************************************/

    private GameManager gameManager;

    public int maxHealth = 105;
    public int health;
    public int attack = 10;
    public int coverBonus;
    public int accurracy;
    public int ammo;

    public string charClass;

    public GameObject gm;

	// Use this for initialization
	void Start () {
        gameManager = gm.GetComponent<GameManager>();
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        GetHealth();
	}

    int GetHealth()
    {
        return health;
    }

    public int TakeDamage(int damage)
    {
        health -= damage;
        return health;
    }

}
