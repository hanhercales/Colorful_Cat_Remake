using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    public void TakeDamage(float amount)
    {
        HpChange(-amount);
    }
    
    public void HpChange(float amount)
    {
        currentHealth += amount;
        CheckHealth();
    }
    public void CheckHealth()
    {
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
    
}
