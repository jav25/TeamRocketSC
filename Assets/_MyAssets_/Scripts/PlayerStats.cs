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

    public int maxHealth = 105;
    public int health;
    public int coverBonus;
    public int armor;

    public int weaponDamage;
    public int weaponDistance;
    public int weaponAccuracy;
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
            weaponDamage = 10;
            weaponAccuracy = 26;
            armor = 10;
        }
        else if(charClass == "Heavy")
        {
            ammo = 12;
            remainingAmmo = ammo;
            fireRate = 3;
            weaponDistance = 8;
            weaponDamage = 15;
            weaponAccuracy = 50;
            armor = 15;
        }
        else if(charClass == "Ranger")
        {
            ammo = 12;
            remainingAmmo = ammo;
            fireRate = 3;
            weaponDistance = 12;
            weaponDamage = 10;
            weaponAccuracy = 10;
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
        return remainingAmmo;
    }

    int GetHealth()
    {
        return health;
    }

    public int AttackDamage(int weapAccuracy)
    {
        //int tempAcc = 
        //attack = accuracy - distanceDecrease;
        int tempChance = Random.Range(0, weaponAccuracy);
        if (tempChance <= weapAccuracy)
        {
            if (weapAccuracy < weaponAccuracy)
            {
                attack = weaponDamage / weapAccuracy;
                return attack;
            }
            else
            {
                attack = weaponDamage;
                return attack;
            }
        }
        else
        {
            return 0;
        }
    }

    public int FireGun()
    {
        remainingAmmo -= fireRate;
        return remainingAmmo;
    }

    public void ReloadGun()
    {
        remainingAmmo = ammo;
    }

    public int TakeDamage(int damage)
    {
        health -= damage;
        return health;
    }

    public int CalculateHit(Vector3 enemyDistance)
    {
        if(Vector3.Distance(gameObject.transform.position, enemyDistance) > weaponDistance)
        {
            accuracy = weaponAccuracy/2;
            return accuracy;
        }
        else
        {
            accuracy = weaponAccuracy;
            return accuracy;
        }
    }

}
