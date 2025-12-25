using UnityEngine;

[CreateAssetMenu(fileName = "Slow", menuName = "Effects/Slow")]
public class Slow : OvertimeEffect
{
    public float speed;
    public float currentSpeed;
    
    public override bool OnApply(GameObject target, GameObject source)
    {
        if(target.TryGetComponent(out Movement moveCtrl))
        {
            currentSpeed = moveCtrl.GetMoveSpeed();
            moveCtrl.SetSpeed(speed);
            return true;
        }
        return false;
    }

    public override void OnTick(GameObject target) {}

    public override void OnRemove(GameObject target, GameObject source)
    {
        if(target.TryGetComponent(out Movement moveCtrl))
        {
            moveCtrl.SetSpeed(speed);
        }
    }
}
