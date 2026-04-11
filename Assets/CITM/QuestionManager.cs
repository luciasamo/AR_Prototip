using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

    [Header("Paneles UI")]
    public GameObject questionPanel;
    public GameObject infoPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI infoText;

    [Header("Botones táctiles")]
    public Button yesButton;
    public Button noButton;

    private MaskSwitcher.MaskData currentMaskData;
    private bool waitingForAnswer = false;
    private bool showingInfo = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Debug.Log("QuestionManager inicializado");
    }

    void Start()
    {
        if (yesButton) yesButton.onClick.AddListener(OnAnswerYes);
        if (noButton) noButton.onClick.AddListener(OnAnswerNo);

        if (questionPanel) questionPanel.SetActive(false);
        if (infoPanel) infoPanel.SetActive(false);
    }

    public void SetCurrentMask(MaskSwitcher.MaskData maskData)
    {
        currentMaskData = maskData;
        waitingForAnswer = true;
        showingInfo = false;

        if (questionPanel) questionPanel.SetActive(true);
        if (infoPanel) infoPanel.SetActive(false);

        if (questionText && currentMaskData != null)
            questionText.text = currentMaskData.questionText;

        Debug.Log($"SetCurrentMask: waitingForAnswer={waitingForAnswer}, showingInfo={showingInfo}");
    }

    public void OnAnswerYes()
    {
        Debug.Log($"OnAnswerYes cridat! waitingForAnswer={waitingForAnswer}, showingInfo={showingInfo}");

        if (!waitingForAnswer || showingInfo)
        {
            Debug.Log("Resposta bloquejada");
            return;
        }
        EvaluateAnswer(true);
    }

    public void OnAnswerNo()
    {
        Debug.Log($"OnAnswerNo cridat! waitingForAnswer={waitingForAnswer}, showingInfo={showingInfo}");

        if (!waitingForAnswer || showingInfo)
        {
            Debug.Log("Resposta bloquejada");
            return;
        }
        EvaluateAnswer(false);
    }

    private void EvaluateAnswer(bool userSaidYes)
    {
        waitingForAnswer = false;
        showingInfo = true;

        bool isCorrect = (userSaidYes == currentMaskData.correctAnswerIsYes);
        string feedback = isCorrect ? currentMaskData.correctFeedback : currentMaskData.wrongFeedback;

        Debug.Log($"Resposta: {(userSaidYes ? "SI" : "NO")} | Correcte: {isCorrect}");

        if (infoPanel)
        {
            infoPanel.SetActive(true);
            if (infoText)
                infoText.text = feedback + "\n\n" + currentMaskData.culturalInfo;
        }
        if (questionPanel) questionPanel.SetActive(false);
    }

    public bool IsWaitingForAnswer()
    {
        return waitingForAnswer && !showingInfo;
    }

    public void ResetForNewMask()
    {
        waitingForAnswer = true;
        showingInfo = false;
        if (questionPanel) questionPanel.SetActive(true);
        if (infoPanel) infoPanel.SetActive(false);
        Debug.Log("QuestionManager reset per nova màscara");
    }
}