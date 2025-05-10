using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DinoController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("velocity of dino")]
    public float moveSpeed = 1f; //vi dang bi di lui nen dat tam speed am 

    [Tooltip("dino rotate velocity")]
    public float rotationSpeed = 7f;

    [Header("Input")]
    [Tooltip("do not find joystick")]
    public FixedJoystick joystick;

    [Header("Animation")] 
    [Tooltip("Float typed parameter's name dạng in Animator Controller to control speed")]
    public string speedParameterName = "Speed"; 

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Animator animator; 
    private int speedParameterHash; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); 

        if (joystick == null)
        {
            joystick = FindObjectOfType<FixedJoystick>();
            if (joystick == null)
            {
                Debug.LogError("error: did not find Fixed Joystick in the scene");
                enabled = false;
                return;
            }
            else
            {
                Debug.Log("Found Fixed Joystick automatically.");
            }
        }

        if (animator == null)
        {
             Debug.LogError("error: did not find Animator component on this GameObject or its children.");

        }
        else
        {
            speedParameterHash = Animator.StringToHash(speedParameterName);
        }


        rb.useGravity = false; 
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Debug.Log("DinoController successfully initialized on: " + gameObject.name);
    }

    void FixedUpdate()
    {

        float horizontalInput = joystick.Horizontal; //vi bi di lui
        float verticalInput = joystick.Vertical; //vi bi di lui

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput);

        float currentSpeed = moveDirection.magnitude; 

        if (animator != null)
        {
            animator.SetFloat(speedParameterHash, currentSpeed); 
        }

        if (currentSpeed > 0.1f) 
        {
            Vector3 normalizedDirection = moveDirection.normalized; 
            
            Vector3 moveOffset = normalizedDirection * (moveSpeed) * Time.fixedDeltaTime; //vi bi di lui nen co tru truoc moveSpeed
            Vector3 targetPosition = rb.position + moveOffset;
            rb.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(normalizedDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }
}