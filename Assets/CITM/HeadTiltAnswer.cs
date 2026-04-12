using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HeadTiltAnswer : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ARFaceManager faceManager;
    [SerializeField] private QuestionManager questionManager;

    [Header("Configuración de inclinación")]
    [SerializeField] private float tiltThreshold = 15f;
    [SerializeField] private float neutralThreshold = 8f;

    [Header("Asignación de respuestas")]
    [SerializeField] private bool rightTiltMeansTrue = true;

    [Header("Control")]
    [SerializeField] private float answerCooldown = 1f;

    private ARFace trackedFace;
    private bool canAnswer = true;
    private float lastAnswerTime = -999f;

    void Awake()
    {
        if (faceManager == null)
            faceManager = FindObjectOfType<ARFaceManager>();

        if (questionManager == null)
            questionManager = FindObjectOfType<QuestionManager>();
    }

    void OnEnable()
    {
        if (faceManager != null)
            faceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        if (faceManager != null)
            faceManager.facesChanged -= OnFacesChanged;
    }

    void Update()
    {
        if (trackedFace == null || questionManager == null)
            return;

        // Si ya se ha respondido la pregunta y está visible el panel de resultado,
        // no volver a responder hasta pasar a la siguiente.
        if (questionManager.resultPanel != null && questionManager.resultPanel.activeSelf)
            return;

        float zAngle = NormalizeAngle(trackedFace.transform.eulerAngles.z);

        // Esperar a que la cabeza vuelva al centro antes de permitir otra respuesta
        if (Mathf.Abs(zAngle) < neutralThreshold)
        {
            canAnswer = true;
            return;
        }

        if (!canAnswer)
            return;

        if (Time.time - lastAnswerTime < answerCooldown)
            return;

        if (zAngle >= tiltThreshold)
        {
            bool userAnswer = rightTiltMeansTrue ? true : false;
            questionManager.Answer(userAnswer);
            RegisterAnswer();
        }
        else if (zAngle <= -tiltThreshold)
        {
            bool userAnswer = rightTiltMeansTrue ? false : true;
            questionManager.Answer(userAnswer);
            RegisterAnswer();
        }
    }

    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added != null && args.added.Count > 0 && trackedFace == null)
        {
            trackedFace = args.added[0];
        }

        if (args.removed != null && args.removed.Count > 0)
        {
            for (int i = 0; i < args.removed.Count; i++)
            {
                if (args.removed[i] == trackedFace)
                {
                    trackedFace = null;

                    if (faceManager != null)
                    {
                        foreach (var face in faceManager.trackables)
                        {
                            trackedFace = face;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void RegisterAnswer()
    {
        canAnswer = false;
        lastAnswerTime = Time.time;
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}