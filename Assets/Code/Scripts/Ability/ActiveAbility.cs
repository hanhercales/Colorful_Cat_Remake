using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Active Ability")]
public class ActiveAbility : Ability
{
    public Effect effect;

    public void Activate(GameObject target, GameObject caster)
    {
        var effectManager = target.GetComponent<EffectManager>();
        if (effectManager != null && effect != null)
        {
            effectManager.ApplyEffect(effect);
        }
    }
}
