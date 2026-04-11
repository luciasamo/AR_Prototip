using UnityEngine;
using System.Collections.Generic;

public class MaskSwitcher : MonoBehaviour
{
    public static MaskSwitcher instance;

    [System.Serializable]
    public class MaskData
    {
        public GameObject maskObject;          // La máscara 3D (arrastrada desde la escena)
        public string maskName;
        [TextArea(2, 3)]
        public string questionText;            // Pregunta tipo "¿Esta máscara es veneciana?"
        public bool correctAnswerIsYes;        // true = Sí es correcto, false = No es correcto
        [TextArea(2, 3)]
        public string correctFeedback;         // "¡Correcto! ..."
        [TextArea(2, 3)]
        public string wrongFeedback;           // "No, era de médicos..."
        [TextArea(3, 5)]
        public string culturalInfo;            // Texto cultural largo
    }

    public List<MaskData> masks;
    private int currentMask = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Activar solo la máscara actual
        for (int i = 0; i < masks.Count; i++)
        {
            if (masks[i].maskObject != null)
                masks[i].maskObject.SetActive(i == currentMask);
        }

        // Notificar a QuestionManager qué máscara está activa
        if (QuestionManager.instance != null)
            QuestionManager.instance.SetCurrentMask(masks[currentMask]);
    }

    public void NextMask()
    {
        currentMask = (currentMask + 1) % masks.Count;
        ShowMask(currentMask);
    }

    public void PreviousMask()
    {
        currentMask--;
        if (currentMask < 0) currentMask = masks.Count - 1;
        ShowMask(currentMask);
    }

    void ShowMask(int index)
    {
        for (int i = 0; i < masks.Count; i++)
        {
            if (masks[i].maskObject != null)
                masks[i].maskObject.SetActive(i == index);
        }

        // Actualizar QuestionManager con la nueva máscara
        if (QuestionManager.instance != null)
            QuestionManager.instance.SetCurrentMask(masks[currentMask]);
    }
}