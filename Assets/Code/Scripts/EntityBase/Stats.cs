using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        CheckHealth();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        CheckHealth();
    }
    
    public void CheckHealth()
    {
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
        if(currentHealth <= 0)
            currentHealth = 0;
    }

    public bool IsHealthFull()
    {
        return currentHealth == maxHealth;
    }
}
