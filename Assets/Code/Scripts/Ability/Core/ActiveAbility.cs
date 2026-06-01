using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActiveAbility", menuName = "Ability/ActiveAbility")]
public class ActiveAbility : Ability
{
    public float cooldownTime;
    //public float castTime;
    
    public List<Effect>  effectsToApply;

    public virtual void Activate(GameObject source, GameObject target)
    {
        GameObject actualTarget = target != null  ? target : source;

        if (actualTarget.TryGetComponent(out EffectHandler handler))
        {
            foreach (Effect effect in effectsToApply)
            {
                handler.AddEffect(effect, source);
            }
        }
    }
}
