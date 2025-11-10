using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BarRotate : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;
    private bool isTouchingBench = false;
    private Vector3 startPos;
    private Quaternion startRot;

    // зчитувані значення
    private float inputVertical = 0f;   
    private float inputHorizontal = 0f; 
    private float inputRotate = 0f;     

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = rb.position;
        startRot = rb.rotation;
    }

    void Update()
    {
        inputVertical = 0f;
        if (Input.GetKey(KeyCode.W)) inputVertical += 1f;
        if (Input.GetKey(KeyCode.S) && !isTouchingBench) inputVertical -= 1f;

        inputHorizontal = 0f;
        if (Input.GetKey(KeyCode.D)) inputHorizontal += 1f;
        if (Input.GetKey(KeyCode.A)) inputHorizontal -= 1f;

        inputRotate = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) inputRotate = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) inputRotate = -1f;
    }

    void FixedUpdate()
    {
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        
        Vector3 forward = rb.rotation * Vector3.forward;
        forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized; 

        // Складаємо вектор руху
        Vector3 move = Vector3.zero;
        move += Vector3.up * inputVertical;     
        move += forward * inputHorizontal;       



        // Застосовуємо рух через MovePosition
        if (move != Vector3.zero)
        {
            Vector3 moveDelta = move * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDelta);
        }

        // Обертання навколо Y через MoveRotation
        if (Mathf.Abs(inputRotate) > 0f)
        {
            Quaternion deltaRot = Quaternion.Euler(Vector3.up * inputRotate * rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRot);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bench"))
        {
            isTouchingBench = true;
            Debug.Log("Штанга торкнулася лавки");
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            rb.MovePosition(startPos);
            rb.MoveRotation(startRot);
            Debug.Log("Штанга впала на підлогу! Повертаємо у стартову позицію");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bench"))
        {
            isTouchingBench = false;
        }
    }
}
