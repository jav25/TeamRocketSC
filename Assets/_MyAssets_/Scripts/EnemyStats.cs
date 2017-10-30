using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    /**************************************************************************************************
     * This Class handles the stats of each individual enemy.
     **************************************************************************************************/

    public int maxHealth = 100;
    public int health;
    public int attack = 10;

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
