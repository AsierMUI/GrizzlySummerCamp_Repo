using UnityEngine;

public class HookController : MonoBehaviour
{
    public Transform hook;
    public Transform topPivot;
    public Transform bottomPivot;

    public float hookPosition;
    public float hookSize = 0.1f;

    public float hookPullPower = 0.01f;
    public float hookGravityPower = 0.005f;
    private float hookPullVelocity;

    public void ResetHook()
    {
        hookPosition = 0.5f;
        hookPullVelocity = 0f;
    }

    public void Tick()
    {
        if (Input.GetMouseButton(0))
            hookPullVelocity += hookPullPower * Time.deltaTime;

        hookPullVelocity -= hookGravityPower * Time.deltaTime;
        hookPosition += hookPullVelocity;

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);

        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }
}
