using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    public SpinAgentPlayer SpinPlayer;
    [Header("Sling Power")]
    public float SlingLengthMax;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference mousePositionAction;
    [SerializeField] private InputActionReference mouseButtonAction;

    private Vector2 slingStartPoint;
    private Vector2 slingEndPoint;
    private Vector2 slingVector;

    private bool isDragging;

    private void OnEnable()
    {
        mouseButtonAction.action.started += SlingStart;
        mouseButtonAction.action.canceled += SlingEnd;
    }
    private void OnDisable()
    {
        mouseButtonAction.action.started -= SlingStart;
        mouseButtonAction.action.canceled -= SlingEnd;
    }

    private void SlingStart(InputAction.CallbackContext context) 
    {
        isDragging = true;

        slingStartPoint = GetMousePosition();
    }
    private void SlingEnd(InputAction.CallbackContext context) 
    {
        if (!isDragging)
            return;

        isDragging = false;

        slingEndPoint = GetMousePosition();

        SlingCalculate();
    }
    private Vector2 GetMousePosition()
    {
        Vector2 screenPos = mousePositionAction.action.ReadValue<Vector2>();
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private void SlingCalculate() 
    {
        slingVector = slingStartPoint - slingEndPoint;
        slingVector = Vector2.ClampMagnitude(slingVector, SlingLengthMax);

        SpinPlayer.Dash(slingVector.normalized, slingVector.magnitude / SlingLengthMax);
    }
}
