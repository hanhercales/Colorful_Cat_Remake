using UnityEngine;

[CreateAssetMenu(fileName = "InstantHeal", menuName = "Effects/Instant Heal")]
public class InstantHeal : InstantEffect
{
    public override bool OnApply(GameObject target, GameObject source)
    {
        if (target.TryGetComponent(out Stats stats))
        {
            if (stats.IsHealthFull()) return false;
            stats.Heal(effectValue);
            return true;
        }
        return false;
    }

    public override void OnRemove(GameObject target, GameObject source){}
}
