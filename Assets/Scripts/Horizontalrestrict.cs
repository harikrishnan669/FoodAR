using UnityEngine;
using UnityEngine.UI;

public class FixScrollPosition : MonoBehaviour
{
    public ScrollRect scrollRect;

    void Update()
    {
        Vector2 fixedPosition = scrollRect.content.anchoredPosition;
        fixedPosition.x = 0;
        scrollRect.content.anchoredPosition = fixedPosition;
    }
}
