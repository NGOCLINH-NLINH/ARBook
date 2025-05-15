using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DinoController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float rotationSpeed = 10f;

    [Header("Input")]
    public FixedJoystick joystick;
    public Camera mainCamera;

    [Header("Animation")]
    public string speedParameterName = "Speed";

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.5f;
    public float minScale = 0.2f;
    public float maxScale = 3f;

    private Rigidbody rb;
    private Animator animator;
    private int speedParameterHash;
    private Collider dinoCollider; //them

    // Variables to store initial state for scale correction (them)
    private float initialBottomY; // World Y position of the collider's bottom at start
    private float initialScaleY;  // Initial Y scale of the transform
    private float initialDistPivotToBottom; // Initial distance from pivot Y to collider bottom Y

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dinoCollider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();

        if (joystick == null)
        {
            joystick = FindObjectOfType<FixedJoystick>();
            if (joystick == null)
            {
                Debug.LogError("FixedJoystick not found in the scene.");
                enabled = false;
                return;
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (animator == null)
        {
            Debug.LogError("Animator not found.");
        }
        else
        {
            speedParameterHash = Animator.StringToHash(speedParameterName);
        }

        //them
        if (dinoCollider == null)
        {
             Debug.LogError("Collider not found on " + gameObject.name + ". Cannot correct vertical position during scaling.");
             // You might want to disable the script or just the zoom part if collider is essential
             // enabled = false; 
             // return;
        }
        else
        {
            // Store initial state for vertical positioning correction during scaling
            initialScaleY = transform.localScale.y;
            initialBottomY = dinoCollider.bounds.min.y; // Get the initial world Y of the collider's bottom
            initialDistPivotToBottom = transform.position.y - initialBottomY; // Calculate initial distance from pivot Y to bottom Y
        }

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        HandleZoom();
    }

    private void HandleMovement()
    {
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        Vector3 move = new Vector3(moveHorizontal, 0f, moveVertical);
        float speed = move.magnitude;

        if (animator != null)
        {
            animator.SetFloat(speedParameterHash, speed);
        }

        if (speed > 0.1f)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 relativeMove = (cameraForward * move.z + cameraRight * move.x).normalized;

            // Vector3 newPosition = rb.position + relativeMove * moveSpeed * Time.fixedDeltaTime;
            // float scaleFactor = transform.localScale.magnitude / Mathf.Sqrt(3f); // Trung bình scale
            float scaleFactor = transform.localScale.y; // Or use magnitude / sqrt(3f) if not uniform
            float scaledSpeed = moveSpeed * scaleFactor;

            Vector3 newPosition = rb.position + relativeMove * scaledSpeed * Time.fixedDeltaTime;

            rb.MovePosition(newPosition);

            if (relativeMove.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(relativeMove);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
            }
        }
    }

    private void HandleZoom()
    {
        //them
        Vector3 currentScale = transform.localScale;
        Vector3 potentialNewScale = currentScale;
#if UNITY_EDITOR || UNITY_STANDALONE
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            potentialNewScale = transform.localScale + Vector3.one * scroll * zoomSpeed;
        }
#else
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            potentialNewScale = transform.localScale + Vector3.one * difference * zoomSpeed * 0.01f;
        }
#endif
        Vector3 clampedNewScale = ClampScale(potentialNewScale);
        // Apply the new scale if it changed
        if (clampedNewScale != currentScale)
        {
            // Áp dụng scale mới
            transform.localScale = clampedNewScale;

            // --- Correct the vertical position after scaling ---
            // Đảm bảo dinoCollider tồn tại và initialScaleY đã được lưu (khác 0)
            if (dinoCollider != null && initialScaleY != 0)
            {
                float currentScaleY = transform.localScale.y;
                // Tính toán vị trí Y mới cần cho pivot
                float requiredPivotY = initialBottomY + initialDistPivotToBottom * (currentScaleY / initialScaleY);

                // Cập nhật vị trí Y của transform
                transform.position = new Vector3(transform.position.x, requiredPivotY, transform.position.z);
            }
            // --- End of vertical position correction ---
        }
    }

    private Vector3 ClampScale(Vector3 scale)
    {
        float clampedX = Mathf.Clamp(scale.x, minScale, maxScale);
        float clampedY = Mathf.Clamp(scale.y, minScale, maxScale);
        float clampedZ = Mathf.Clamp(scale.z, minScale, maxScale);
        // return new Vector3(clampedX, clampedY, clampedZ);
        // Assuming uniform scaling, clamp all axes based on one (e.g., Y)
        float uniformClamped = Mathf.Clamp(scale.y, minScale, maxScale);
        return new Vector3(uniformClamped, uniformClamped, uniformClamped);
    }
}
