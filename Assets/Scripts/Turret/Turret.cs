using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform yawPivot;
    public Transform targetDrone;
    public float rotationSpeed = 20f;

    private void Update()
    {
        if(targetDrone == null) return;

        Vector3 direction = targetDrone.position - yawPivot.position;

        direction.y = 0f;

        if(direction.sqrMagnitude > 0.001)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            yawPivot.rotation = Quaternion.RotateTowards(yawPivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
