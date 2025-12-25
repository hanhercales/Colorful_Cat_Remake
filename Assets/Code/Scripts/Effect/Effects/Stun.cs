using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "Effects/Stun")]
public class Stun : OvertimeEffect
{
    public override bool OnApply(GameObject target, GameObject source)
    {
        if(target.TryGetComponent(out Movement moveCtrl))
        {
            moveCtrl.SetMoveable(false);
            return true;
        }
        return false;
    }

    public override void OnTick(GameObject target) {}

    public override void OnRemove(GameObject target, GameObject source)
    {
        if(target.TryGetComponent(out Movement moveCtrl))
        {
            moveCtrl.SetMoveable(true);
        }
    }
}
