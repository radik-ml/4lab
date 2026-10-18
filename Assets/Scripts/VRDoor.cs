
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(XRSimpleInteractable))]
public class VRDoor : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float springForce = 100f;
    [SerializeField] private float damping = 15f;

    private HingeJoint hinge;
    private XRSimpleInteractable interactable;
    private bool isOpen;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        interactable = GetComponent<XRSimpleInteractable>();

        JointSpring spring = hinge.spring;
        spring.spring = springForce;
        spring.damper = damping;
        spring.targetPosition = 0f;

        hinge.spring = spring;
        hinge.useSpring = true;
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelect);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelect);
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        isOpen = !isOpen;

        JointSpring spring = hinge.spring;
        spring.targetPosition = isOpen ? openAngle : 0f;

        hinge.spring = spring;
    }
}
