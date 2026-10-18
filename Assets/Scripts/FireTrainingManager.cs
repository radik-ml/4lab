
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FireTrainingManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private HingeJoint door;
    [SerializeField] private XRGrabInteractable extinguisher;
    [SerializeField] private Transform fireTarget;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private TMP_Text taskText;

    [Header("Settings")]
    [SerializeField] private float doorOpenAngle = 35f;
    [SerializeField] private float fireDistance = 2f;

    private readonly string[] tasks =
    {
        "Открыть дверь кабинета",
        "Взять огнетушитель",
        "Подойти к пожару",
        "Потушить пожар"
    };

    private int currentTask;
    private float initialDoorAngle;
    private bool activationRequested;

    private void Start()
    {
        if (door == null || extinguisher == null ||
            fireTarget == null || fireEffect == null ||
            taskText == null)
        {
            enabled = false;
            return;
        }

        initialDoorAngle = door.angle;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (extinguisher != null)
            extinguisher.activated.AddListener(OnActivate);
    }

    private void OnDisable()
    {
        if (extinguisher != null)
            extinguisher.activated.RemoveListener(OnActivate);
    }

    private void OnActivate(ActivateEventArgs args)
    {
        activationRequested = true;
    }

    private void Update()
    {
        switch (currentTask)
        {
            case 0:
                if (Mathf.Abs(Mathf.DeltaAngle(
                    initialDoorAngle, door.angle
                )) >= doorOpenAngle)
                    NextTask();
                break;

            case 1:
                if (extinguisher.isSelected)
                    NextTask();
                break;

            case 2:
                if (extinguisher.isSelected && IsNearFire())
                    NextTask();
                break;

            case 3:
                if (activationRequested &&
                    extinguisher.isSelected &&
                    IsNearFire())
                {
                    fireEffect.SetActive(false);
                    NextTask();
                }
                break;
        }

        activationRequested = false;
    }

    private bool IsNearFire()
    {
        return Vector3.Distance(
            extinguisher.transform.position,
            fireTarget.position
        ) <= fireDistance;
    }

    private void NextTask()
    {
        currentTask++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        string result = "<b>ПОЖАРНАЯ ТРЕНИРОВКА</b>\n\n";

        for (int i = 0; i < tasks.Length; i++)
        {
            if (i < currentTask)
            {
                result += "<color=#65DB9A>✓ </color>" +
                    "<color=#AAB7C5><s>" +
                    tasks[i] + "</s></color>\n\n";
            }
            else if (i == currentTask)
            {
                result += "<color=#FFBF73>● </color>" +
                    "<b>" + tasks[i] + "</b>\n\n";
            }
            else
            {
                result += "<color=#8A9AAD>○ " +
                    tasks[i] + "</color>\n\n";
            }
        }

        result += "Выполнено: " +
            Mathf.Min(currentTask, tasks.Length) +
            " из " + tasks.Length;

        if (currentTask >= tasks.Length)
        {
            result +=
                "\n\n<color=#65DB9A><b>" +
                "ТРЕНИРОВКА ЗАВЕРШЕНА!" +
                "</b></color>";
        }

        taskText.text = result;
    }
}
