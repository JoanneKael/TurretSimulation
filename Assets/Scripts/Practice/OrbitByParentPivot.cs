using UnityEngine;

public class OrbitByParentPivot : MonoBehaviour
{
    public Transform orbitPivot;
    public float orbitSpeed = 30f;

    private void Update()
    {
        if (orbitPivot == null) return;

        orbitPivot.Rotate(Vector3.up, orbitSpeed * Time.deltaTime, Space.Self);
    }
}
