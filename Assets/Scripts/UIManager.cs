using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vuforia;

public class ARUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainUI;
    public GameObject arPanel1;
    public GameObject arPanel2;
    public GameObject reviewPanel;
    public GameObject thankYouPanel;
    public GameObject foodInfoPanel;
    public GameObject foodInfoPanel2;
    public GameObject exitConfirmationPanel;
    public GameObject foodMenuPanel;
    public TextMeshProUGUI foodInfoText;
    public TextMeshProUGUI foodInfoText2;

    [Header("AR Setup")]
    public GameObject arCamera;
    public GameObject foodModelsContainer;
    public GameObject[] foodModels;
    private ObserverBehaviour observerBehaviour;

    [Header("UI Buttons")]
    public Button scanButton, nextButton, previousButton, foodInfoButton, homeButton, exitButton;
    public Button reviewButton, rotateButton, yesExitButton, noExitButton, menuButton, backButton;

    private int currentModelIndex = 0;
    private bool isFoodInfoVisible = false;
    private bool isModelVisible = false;

    private string[] foodDescriptions =
    {
        " Kappa & Fish Curry\r\nDescription: A traditional Kerala dish with steamed tapioca and spicy fish curry.\r\nIngredients: Tapioca,fish (mackerel/sardines),coconut, turmeric,chili,tamarind,curry leaves.\r\nPrice: Rs 250/-\r\n",
        " Idli & Chutney\r\nDescription: Soft steamed rice cakes served with coconut chutney.\r\nIngredients: Rice,urad dal(black gram),fenugreek seeds,coconut,green chili,ginger,curry leaves,mustard seeds.\r\nPrice: Rs 50/-\r\n\r\n",
        " Kerala Ghee Roast Dosa\r\nDescription: Crispy Kerala-style dosa roasted in ghee and coconut oil, served with coconut chutney and fish/vegetable sambar.\r\nIngredients: Dosa batter(rice,urad dal,coconut,fenugreek),ghee,coconut oil,spices(red chili,curry leaves,mustard seeds).\r\nPrice: Rs 120/-\r\n",
        " Chappathi & Curry\r\nDescription: Soft whole wheat flatbread served with mixed vegetable curry.\r\nIngredients: Whole wheat chappathi, mixed vegetable curry (potatoes, carrots, peas), coconut milk, spices (turmeric, coriander).\r\nPrice: Rs 70/-\r\n",
        " Puttu & Kadala Curry\r\nDescription: Steamed rice cakes with coconut served with spiced black chickpea curry.\r\nIngredients: Rice flour, coconut, salt, black chickpeas, onions, tomatoes, spices (turmeric, chili, coriander, mustard seeds), tamarind, curry leaves.\r\nPrice: Rs 90/-\r\n\r\n",
        " Chicken Biriyani\r\nDescription: Fragrant basmati rice cooked with tender chicken and aromatic spices.\r\nIngredients: Basmati rice, chicken, onions, tomatoes, yogurt, mint, coriander, spices (cumin, cardamom, cloves).\r\nPrice: Rs 190/-\r\n"
    };

    void Start()
    {
        ShowPanel(mainUI);

        scanButton.onClick.AddListener(OnScanButtonClicked);
        nextButton.onClick.AddListener(NextModel);
        previousButton.onClick.AddListener(PreviousModel);
        foodInfoButton.onClick.AddListener(ToggleFoodInfo);
        homeButton.onClick.AddListener(OnHomeButtonClicked);
        exitButton.onClick.AddListener(ShowExitConfirmation);
        yesExitButton.onClick.AddListener(ExitApplication);
        noExitButton.onClick.AddListener(CloseExitPopup);
        reviewButton.onClick.AddListener(OnReviewButtonClicked);
        rotateButton.onClick.AddListener(ToggleRotation);
        menuButton.onClick.AddListener(ShowFoodMenu);
        foodInfoPanel.SetActive(false);
        foodInfoPanel2.SetActive(false);

        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        for (int i = 0; i < foodModels.Length; i++)
        {
            foodModels[i].SetActive(i == 0);
        }

        foodInfoPanel.SetActive(false);
        exitConfirmationPanel.SetActive(false);
        foodMenuPanel.SetActive(false);
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED)
        {
            Debug.Log("🔹 Target Found! Showing First Model...");
            isModelVisible = true;
            foodModelsContainer.SetActive(true);

            for (int i = 0; i < foodModels.Length; i++)
            {
                foodModels[i].SetActive(i == currentModelIndex);
            }

            if (isFoodInfoVisible)
            {
                if (arPanel1.activeSelf)
                    foodInfoPanel.SetActive(true);
                else if (arPanel2.activeSelf)
                    foodInfoPanel2.SetActive(true);
            }
        }
        else
        {
            Debug.Log("🔹 Target Lost! Hiding Model & Food Info...");
            isModelVisible = false;
            foodModelsContainer.SetActive(false);

            
            foodInfoPanel.SetActive(false);
            foodInfoPanel2.SetActive(false);
        }
    }


    public void ToggleFoodInfo()
    {
        if (foodInfoPanel == null || foodInfoText == null || foodInfoPanel2 == null || foodInfoText2 == null)
        {
            Debug.LogError("❌ One of the Food Info Panels or Text elements is NULL! Check the Inspector.");
            return;
        }

        isFoodInfoVisible = !isFoodInfoVisible;  

        
        foodInfoPanel.SetActive(false);
        foodInfoPanel2.SetActive(false);

        if (isFoodInfoVisible && currentModelIndex >= 0 && currentModelIndex < foodDescriptions.Length)
        {
            if (arPanel1.activeSelf)
            {
                Debug.Log("Showing Food Info in AR Panel 1");
                foodInfoPanel.SetActive(true);
                foodInfoText.text = foodDescriptions[currentModelIndex];
            }
            else if (arPanel2.activeSelf)
            {
                Debug.Log("Showing Food Info in AR Panel 2");
                foodInfoPanel2.SetActive(true);
                foodInfoText2.text = foodDescriptions[currentModelIndex];
            }
        }
    }




    void NextModel()
    {
        currentModelIndex = (currentModelIndex + 1) % foodModels.Length;
        UpdateModelVisibility();
    }

    void PreviousModel()
    {
        currentModelIndex = (currentModelIndex - 1 + foodModels.Length) % foodModels.Length;
        UpdateModelVisibility();
    }

    void UpdateModelVisibility()
    {
        for (int i = 0; i < foodModels.Length; i++)
        {
            foodModels[i].SetActive(i == currentModelIndex);
        }

        if (foodInfoText != null && currentModelIndex >= 0 && currentModelIndex < foodDescriptions.Length)
        {
            foodInfoText.text = foodDescriptions[currentModelIndex];
        }
        if (foodInfoText2 != null && currentModelIndex >= 0 && currentModelIndex < foodDescriptions.Length)
        {
            foodInfoText2.text = foodDescriptions[currentModelIndex];
        }
    }


    public void ToggleRotation()
    {
        if (foodModels[currentModelIndex] != null)
        {
            ModelRotator rotator = foodModels[currentModelIndex].GetComponent<ModelRotator>();

            if (rotator != null)
            {
                if (rotator.IsRotating)
                {
                    rotator.StopRotation();
                    Debug.Log(" Stopping Rotation for Model: " + currentModelIndex);
                }
                else
                {
                    rotator.StartRotation();
                    Debug.Log(" Starting Rotation for Model: " + currentModelIndex);
                }
            }
            else
            {
                Debug.LogError(" No ModelRotator script found on the model.");
            }
        }
        else
        {
            Debug.LogError(" No model found at index: " + currentModelIndex);
        }
    }


    public void OnScanButtonClicked()
    {
        Debug.Log("Scan button clicked!");
        ShowPanel(arPanel1);

        if (arCamera)
        {
            arCamera.SetActive(true);
            Camera arCam = arCamera.GetComponent<Camera>();
            if (arCam)
                arCam.enabled = true;
            else
                Debug.LogError("AR Camera does not have a Camera component.");
        }
        else
        {
            Debug.LogError("AR Camera is NOT assigned!");
        }

        
        for (int i = 0; i < foodModels.Length; i++)
        {
            foodModels[i].SetActive(i == currentModelIndex);
        }

        Debug.Log("🔹 Only Model " + currentModelIndex + " is now visible.");
    }


    public void GoBackToMenu()
    {
        ShowPanel(foodMenuPanel);  
        arPanel2.SetActive(false); 
    }


    public void OnHomeButtonClicked()
    {
        Debug.Log("Home button clicked!");
        ShowPanel(mainUI);
    }

    public void OnReviewButtonClicked()
    {
        Debug.Log("Review button clicked!");
        ShowPanel(reviewPanel);
    }

    public void ShowExitConfirmation()
    {
        exitConfirmationPanel.SetActive(true);
    }

    public void ExitApplication()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CloseExitPopup()
    {
        exitConfirmationPanel.SetActive(false);
    }

    public void ShowFoodMenu()
    {
        ShowPanel(foodMenuPanel);
    }

    public void LoadFoodItem(int index)
    {
        Debug.Log("Loading food item: " + index);
        currentModelIndex = index;
        ShowPanel(arPanel2);
        UpdateModelVisibility();

        if (foodInfoPanel2 != null && foodInfoText2 != null)
        {
            foodInfoText2.text = foodDescriptions[currentModelIndex];
            foodInfoPanel2.SetActive(false);
        }

        
        for (int i = 0; i < foodModels.Length; i++)
        {
            foodModels[i].SetActive(i == currentModelIndex);
        }
    }



    private void ShowPanel(GameObject panelToShow)
    {
        if (mainUI) mainUI.SetActive(false);
        if (arPanel1) arPanel1.SetActive(false);
        if (arPanel2) arPanel2.SetActive(false);
        if (reviewPanel) reviewPanel.SetActive(false);
        if (thankYouPanel) thankYouPanel.SetActive(false);
        if (exitConfirmationPanel) exitConfirmationPanel.SetActive(false);
        if (foodMenuPanel) foodMenuPanel.SetActive(false);

        if (panelToShow)
        {
            panelToShow.SetActive(true);
        }
    }
}
