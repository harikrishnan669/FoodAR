using UnityEngine;
using UnityEngine.UI;

public class BackButtonHandler : MonoBehaviour
{
    public GameObject[] panels; 
    private int currentPanelIndex = 0;

    void Start()
    {
        
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            HandleBackButton();
        }
    }

    public void HandleBackButton()
    {
        if (currentPanelIndex > 0)
        {
            
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            panels[currentPanelIndex].SetActive(true);
        }
        else
        {
            
            Application.Quit();
        }
    }

    public void GoToNextPanel(int panelIndex)
    {
        if (panelIndex >= 0 && panelIndex < panels.Length)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex = panelIndex;
            panels[currentPanelIndex].SetActive(true);
        }
    }
}
