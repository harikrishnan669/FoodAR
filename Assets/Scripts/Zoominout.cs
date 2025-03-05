using UnityEngine;

public class PinchToZoom : MonoBehaviour
{
    public float zoomSpeed = 0.01f;  

    void Update()
    {
        if (Input.touchCount == 2) 
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            
            Zoom(deltaMagnitudeDiff * zoomSpeed);
        }
    }

    void Zoom(float scaleFactor)
    {
        float scaleMultiplier = 1 + scaleFactor; 
        transform.localScale *= scaleMultiplier; 
    }
}
