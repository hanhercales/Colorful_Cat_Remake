using UnityEngine;

public class Stats : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public void CheckHealth()
    {
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
