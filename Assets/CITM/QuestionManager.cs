using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject infoPanel;        // El panel de información actual que se muestra con la máscara
    public GameObject questionPanel;    // El nuevo panel que contendrá la pregunta

    [Header("Texto de Pregunta")]
    public TMPro.TextMeshProUGUI questionText; // Para mostrar la pregunta (usa TextMeshPro)
    // Alternativa: Si usas UI Text normal, cambia a: public UnityEngine.UI.Text questionText;

    private string currentQuestion = "¿Esta máscara es de la cultura Veneciana?";

    void Start()
    {
        // Aseguramos que al inicio la pregunta esté oculta
        if (questionPanel != null)
            questionPanel.SetActive(false);
    }

    // Método que llamará el botón "Preguntas"
    public void OnShowQuestionButtonPressed()
    {
        // 1. Ocultar el panel de información
        if (infoPanel != null)
            infoPanel.SetActive(false);

        // 2. Mostrar el panel de preguntas
        if (questionPanel != null)
            questionPanel.SetActive(true);

        // 3. Actualizar el texto de la pregunta (opcional, por si quieres cambiarla)
        if (questionText != null)
            questionText.text = currentQuestion;
    }

    // Método para el botón "SÍ"
    public void OnAnswerYes()
    {
        Debug.Log("Respuesta: SÍ");
        // Por ahora solo un log. Después puedes sumar puntos, desbloquear, etc.
        // Aquí puedes decidir si cierras el panel o haces algo más
        CloseQuestionPanel();
    }

    // Método para el botón "NO"
    public void OnAnswerNo()
    {
        Debug.Log("Respuesta: NO");
        // Por ahora solo un log.
        CloseQuestionPanel();
    }

    // Opcional: cerrar el panel de preguntas y quizás volver a mostrar info?
    private void CloseQuestionPanel()
    {
        if (questionPanel != null)
            questionPanel.SetActive(false);

        // Decide si quieres que el infoPanel vuelva a aparecer o no
        // if (infoPanel != null)
        //     infoPanel.SetActive(true);
    }
}