using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HeadGestureDetector : MonoBehaviour
{
    [Header("Detección")]
    public float thresholdAngle = 15f;
    public float cooldownTime = 1.5f;
    public float maxGestureTime = 2.5f;

    private ARFaceManager faceManager;
    private float lastGestureTime = -999f;
    private bool gestureInProgress = false;
    private float initialYaw;
    private float gestureStartTime;
    private DebugLogger debugLogger;

    void Start()
    {
        debugLogger = FindObjectOfType<DebugLogger>();
        faceManager = FindFirstObjectByType<ARFaceManager>();

        AddDebugMessage("=== HeadGestureDetector INICIADO ===");

        if (faceManager == null)
            AddDebugMessage("❌ ERROR: No se encuentra ARFaceManager");
        else
            AddDebugMessage("✅ ARFaceManager encontrado");

        if (QuestionManager.instance == null)
            AddDebugMessage("⚠️ QuestionManager.instance es NULL");
        else
            AddDebugMessage("✅ QuestionManager encontrado");
    }

    void Update()
    {
        if (QuestionManager.instance == null) return;
        if (faceManager == null || faceManager.trackables.count == 0) return;

        if (!QuestionManager.instance.IsWaitingForAnswer()) return;

        ARFace face = GetFirstFace();
        if (face == null) return;

        float currentYaw = NormalizeAngle(face.transform.rotation.eulerAngles.y);
        DetectHeadTurn(currentYaw);
    }

    private ARFace GetFirstFace()
    {
        foreach (var face in faceManager.trackables)
            return face;
        return null;
    }

    void DetectHeadTurn(float currentYaw)
    {
        if (Time.time - lastGestureTime < cooldownTime) return;

        if (!gestureInProgress)
        {
            initialYaw = currentYaw;
            gestureInProgress = true;
            gestureStartTime = Time.time;
            AddDebugMessage($"▶ Inicio gesto - Yaw: {initialYaw:F1}°");
        }
        else
        {
            float deltaYaw = Mathf.DeltaAngle(initialYaw, currentYaw);

            if (Mathf.Abs(deltaYaw) > thresholdAngle)
            {
                AddDebugMessage($"🎯 Gesto COMPLETADO! Delta: {deltaYaw:F1}° -> {(deltaYaw > 0 ? "DERECHA (SÍ)" : "IZQUIERDA (NO)")}");

                if (deltaYaw > 0)
                    QuestionManager.instance.OnAnswerYes();
                else
                    QuestionManager.instance.OnAnswerNo();

                lastGestureTime = Time.time;
                gestureInProgress = false;
            }
            else if (Time.time - gestureStartTime > maxGestureTime)
            {
                AddDebugMessage($"❌ Gesto cancelado (timeout)");
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

    private void AddDebugMessage(string msg)
    {
        Debug.Log($"[HeadGesture] {msg}");
        if (debugLogger != null)
            debugLogger.AddMessage(msg);
    }
}