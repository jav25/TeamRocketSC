using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{ 
    /**************************************************************************************************
     * This Class handles the stats of each individual character.
     **************************************************************************************************/

    private GameManager gameManager;
    //private PlayerStats playerStats;

    public int maxHealth = 100;
    public int health;
    public int coverBonus;
    public int armor;

    public int weaponDamage;
    public int weaponDistance;
    //public int weaponAccuracy;
    public int distanceDecrease;
    public int attack;
    public int accuracy;
    
    public int ammo;
    public int remainingAmmo;
    public int fireRate;

    public string charClass;

    public int maxDistance = 12;

    public GameObject gm;


    // Use this for initialization
    void Start()
    {
        gameManager = gm.GetComponent<GameManager>();
        health = maxHealth;

        if(charClass == "Assault")
        {
            ammo = 12;
            remainingAmmo = ammo;
            fireRate = 3;
            weaponDistance = 8; //if an enemy character is within the weaponDistance increase the accuracy
            weaponDamage = 8;
            armor = 10;
        }
        else if(charClass == "Heavy")
        {
            ammo = 4;
            remainingAmmo = ammo;
            fireRate = 1;
            weaponDistance = 6;
            weaponDamage = 15;
            armor = 15;
        }
        else if(charClass == "Ranger")
        {
            ammo = 6;
            remainingAmmo = ammo;
            fireRate = 1;
            weaponDistance = 12;
            weaponDamage = 10;
            armor = 8;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        GetHealth();
        GetAmmo();
    }

    int GetAmmo()
    {
        return ammo;
    }

    int GetHealth()
    {
        return health;
    }

    public int AttackDamage()
    {
        //int tempAcc = 
        //attack = accuracy - distanceDecrease;
        attack = weaponDamage;
        return attack;
    }

    public void FireGun()
    {
        remainingAmmo -= fireRate;
    }

    public void ReloadGun()
    {
        remainingAmmo = ammo;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

}
