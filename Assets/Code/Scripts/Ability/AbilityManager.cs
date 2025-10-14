using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance { get; private set; }

    private HashSet<AbilityID> unlockedAbilities = new HashSet<AbilityID>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UnlockAbility(Ability abilityToUnlock)
    {
        if(abilityToUnlock != null && !unlockedAbilities.Contains(abilityToUnlock.abilityID))
        {
            unlockedAbilities.Add(abilityToUnlock.abilityID);
        }
    }
    
    public void LockAbility(Ability abilityToLock)
     {
         if(abilityToLock != null && unlockedAbilities.Contains(abilityToLock.abilityID))
         {
             unlockedAbilities.Remove(abilityToLock.abilityID);
         }
     }

    public bool IsAbilityUnlocked(AbilityID idToCheck)
    {
        return unlockedAbilities.Contains(idToCheck);
    }
}
