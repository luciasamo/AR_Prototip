using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public Question[] questions;

    public TextMeshProUGUI questionText;
    public TextMeshProUGUI resultText;
    public GameObject resultPanel;

    public MasksController masksController;

    private int currentQuestion = 0;

    void Start()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        Question q = questions[currentQuestion];

        questionText.text = q.questionText;
        resultPanel.SetActive(false);

        masksController.ShowMask(q.maskIndex);
    }

    public void Answer(bool userAnswer)
    {
        Question q = questions[currentQuestion];
        resultPanel.SetActive(true);

        if (userAnswer == q.correctAnswer)
        {
            resultText.text = "Correcto\n\n" + q.explanation;
        }
        else
        {
            resultText.text = "Incorrecto\n\n" + q.explanation;
        }
    }

    public void Next()
    {
        currentQuestion++;

        if (currentQuestion >= questions.Length)
            currentQuestion = 0;

        LoadQuestion();
    }

    public void Previous()
    {
        currentQuestion--;

        if (currentQuestion < 0)
            currentQuestion = questions.Length - 1;

        LoadQuestion();
    }
}