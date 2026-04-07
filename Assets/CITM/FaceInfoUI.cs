using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    public ARFaceManager faceManager;

    // Referencia al QuestionManager para saber si las preguntas están activas
    public QuestionManager questionManager;

    void Update()
    {
        // Solo mostrar infoPanel si NO hay preguntas activas
        bool questionsActive = questionManager != null && questionManager.questionPanel != null && questionManager.questionPanel.activeSelf;

        if (faceManager.trackables.count > 0 && !questionsActive)
        {
            if (infoPanel != null && !infoPanel.activeSelf)
                infoPanel.SetActive(true);
        }
        else
        {
            if (infoPanel != null && infoPanel.activeSelf && !questionsActive)
                infoPanel.SetActive(false);
        }
    }
}