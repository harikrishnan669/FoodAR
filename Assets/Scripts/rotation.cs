using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    public float rotationSpeed = 30f; 
    private bool isRotating = false;

    public bool IsRotating => isRotating; 

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    
    void OnDisable()
    {
        isRotating = false;
    }
}
