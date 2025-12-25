using UnityEngine;

[CreateAssetMenu(fileName = "PassiveHeal", menuName = "Effects/Passive Heal")]
public class PassiveHeal : OvertimeEffect
{
    public float healAmount;

    public override bool OnApply(GameObject target, GameObject source)
    {
        return true;
    }

    public override void OnTick(GameObject target)
    {
        if(target.TryGetComponent(out Stats stats))
        {
            stats.Heal(healAmount);
            //Heal Flashing
        }
    }

    public override void OnRemove(GameObject target, GameObject source) {}
}
