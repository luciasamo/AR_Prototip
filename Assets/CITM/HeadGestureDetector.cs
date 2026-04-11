using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;

public class HeadGestureDetector : MonoBehaviour
{
    [Header("Detección")]
    public float thresholdAngle = 3f;      // Grados de giro para activar respuesta (reducido a 20)
    public float cooldownTime = 1.5f;       // Evita detecciones múltiples

    private ARFaceManager faceManager;
    private float lastGestureTime = -999f;
    private bool gestureInProgress = false;
    private float initialYaw;               // Guardamos solo el ángulo yaw
    private float gestureStartTime;
    private DebugLogger debugLogger;
    private float lastDebugLogTime = 0f;
    private const float debugLogInterval = 1f;

    void Start()
    {
        // Buscar DebugLogger
        debugLogger = FindObjectOfType<DebugLogger>();
        if (debugLogger == null)
            Debug.LogWarning("HeadGestureDetector: No se encontró DebugLogger");

        // Buscar ARFaceManager
        faceManager = FindFirstObjectByType<ARFaceManager>();
        if (faceManager == null)
        {
            string msg = "ERROR: No se encontró ARFaceManager";
            Debug.LogError(msg);
            if (debugLogger) debugLogger?.AddMessage(msg);
        }
        else
        {
            if (debugLogger) debugLogger.AddMessage("ARFaceManager encontrado");
        }
    }

    void Update()
    {
        // Log periódico de estado general
        if (Time.time - lastDebugLogTime > debugLogInterval)
        {
            lastDebugLogTime = Time.time;
            if (faceManager != null)
            {
                int faceCount = faceManager.trackables.count;
                bool waiting = (QuestionManager.instance != null && QuestionManager.instance.IsWaitingForAnswer());
                string msg = $"Caras: {faceCount} | Esperando respuesta: {waiting} | Gesture: {(gestureInProgress ? "en progreso" : "inactivo")}";
                Debug.Log(msg);
                if (debugLogger) debugLogger?.AddMessage(msg);
            }
        }

        if (faceManager == null || faceManager.trackables.count == 0) return;

        if (QuestionManager.instance == null || !QuestionManager.instance.IsWaitingForAnswer())
        {
            gestureInProgress = false;
            return;
        }

        ARFace face = GetFirstFace();
        if (face == null) return;

        // Mostrar el yaw actual y el delta cada cierto tiempo
        if (Time.frameCount % 60 == 0 && debugLogger != null)
        {
            float yaw = NormalizeAngle(face.transform.rotation.eulerAngles.y);
            float delta = gestureInProgress ? Mathf.DeltaAngle(initialYaw, yaw) : 0;
            debugLogger.AddMessage($"Yaw: {yaw:F1}° | Delta: {delta:F1}° (umbral {thresholdAngle}°)");
        }

        DetectHeadTurn(face.transform.rotation);
    }

    private ARFace GetFirstFace()
    {
        foreach (var face in faceManager.trackables)
            return face;
        return null;
    }

    void DetectHeadTurn(Quaternion currentRotation)
    {
        if (Time.time - lastGestureTime < cooldownTime) return;

        float currentYaw = NormalizeAngle(currentRotation.eulerAngles.y);

        if (!gestureInProgress)
        {
            // Iniciamos un nuevo gesto
            initialYaw = currentYaw;
            gestureInProgress = true;
            gestureStartTime = Time.time;
            if (debugLogger) debugLogger.AddMessage($"▶ Inicio gesto (ángulo inicial {initialYaw:F1}°)");
        }
        else
        {
            float deltaYaw = Mathf.DeltaAngle(initialYaw, currentYaw);
            // Comprobar si se ha alcanzado el umbral
            if (Mathf.Abs(deltaYaw) > thresholdAngle)
            {
                string respuesta = deltaYaw > 0 ? "SÍ (derecha)" : "NO (izquierda)";
                if (debugLogger) debugLogger.AddMessage($"✅ Gesto completado: {respuesta} (delta={deltaYaw:F1}°)");

                if (deltaYaw > 0)
                    QuestionManager.instance.OnAnswerYes();
                else
                    QuestionManager.instance.OnAnswerNo();

                lastGestureTime = Time.time;
                gestureInProgress = false;
            }
            else if (Time.time - gestureStartTime > 2.5f && Mathf.Abs(deltaYaw) < 8f)
            {
                // Cancelar si ha pasado mucho tiempo sin movimiento significativo
                if (debugLogger) debugLogger.AddMessage($"❌ Gesto cancelado (tiempo sin alcanzar umbral, delta={deltaYaw:F1}°)");
                gestureInProgress = false;
            }
        }
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}