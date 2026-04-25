using UnityEngine;

public abstract class PassiveAbility : Ability
{
    public abstract void OnEquip(GameObject player);
    public abstract void OnUnequip(GameObject player);
}
