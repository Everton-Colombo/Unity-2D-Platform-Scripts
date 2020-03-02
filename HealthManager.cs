using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HealthManager : MonoBehaviour
{
    public bool isDead = false;
    public float maxHealth;

    [SerializeField] private float health;

    [Range(0, 1)] public float defense;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        float damageToTake = amount * (1 - defense);
        if (health > damageToTake)
            health -= damageToTake;
        else
        {
            health = 0;
            isDead = true;
            Debug.Log("Entity is dead");
        }
    }

    public void Heal(float amount)
    {
        if (health + amount <= maxHealth)
        {
            health += amount;
        }
        else
            health = maxHealth;
    }
}
