using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    public ARFaceManager faceManager;
    public QuestionManager questionManager;

    void Update()
    {
        if (questionManager == null)
            questionManager = FindObjectOfType<QuestionManager>();

        if (questionManager == null) return;

        // Comprovar si el panel de preguntes està actiu
        bool questionPanelActive = questionManager.questionPanel != null && questionManager.questionPanel.activeSelf;

        if (faceManager != null && faceManager.trackables.count > 0 && !questionPanelActive)
        {
            if (infoPanel != null && !infoPanel.activeSelf)
                infoPanel.SetActive(true);
        }
        else
        {
            if (infoPanel != null && infoPanel.activeSelf)
                infoPanel.SetActive(false);
        }
    }
}