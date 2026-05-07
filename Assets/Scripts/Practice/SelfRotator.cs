using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public Vector3 rotationDirection = new Vector3(10f,20f,30f);
    Vector3 finalRotation;
    public Space rotationSpace = Space.Self;
    private void Start()
    {
        
    }

    private void Update()
    {
        finalRotation = rotationDirection * rotationSpeed;

        transform.Rotate(finalRotation * Time.deltaTime, rotationSpace);
    }
}