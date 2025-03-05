using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FeedbackManager : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField phoneField;
    public TMP_InputField reviewField;
    public GameObject thankYouPanel; 
    public GameObject mainUIpanel;

    private FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;  
        thankYouPanel.SetActive(false);  
    }

    public void SubmitFeedback()
    {
        string name = nameField.text.Trim();
        string phone = phoneField.text.Trim();
        string review = reviewField.text.Trim();

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(review))
        {
            Debug.LogError("All fields must be filled!");
            return;
        }

        
        Dictionary<string, object> feedbackData = new Dictionary<string, object>
        {
            { "Name", name },
            { "PhoneNumber", phone },
            { "Review", review }

        };

        
        db.Collection("Feedback").Document(phone).SetAsync(feedbackData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Feedback submitted successfully!");
                ShowThankYouPanel();
            }
            else
            {
                Debug.LogError("Error submitting feedback: " + task.Exception);
            }
        });
    }

    void ShowThankYouPanel()
    {
        thankYouPanel.SetActive(true);
    }
    public void OnHomeButtonClicked()
    {
        Debug.Log("Home button clicked!");
        mainUIpanel.SetActive(true);
    }
}
