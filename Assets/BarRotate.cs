using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BarRotate : MonoBehaviour
{
    public float moveSpeed = 5f;        // швидкість руху
    public float rotationSpeed = 100f;  // швидкість обертання
    private Rigidbody rb;

    private bool isTouchingBench = false; 

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Запам’ятовуємо стартову позицію й обертання
        startPos = rb.position;
        startRot = rb.rotation;
    }

    void Update()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 move = Vector3.zero;

        // Вгору
        if (Input.GetKey(KeyCode.W))
            move += Vector3.up * moveSpeed * Time.deltaTime;

        // Вниз (тільки якщо не торкаємось лавки)
        if (Input.GetKey(KeyCode.S) && !isTouchingBench)
            move += Vector3.down * moveSpeed * Time.deltaTime;

        // Вперед/назад відносно грифа
        if (Input.GetKey(KeyCode.D))
            move += transform.forward * moveSpeed * Time.deltaTime;   
        if (Input.GetKey(KeyCode.A))
            move -= transform.forward * moveSpeed * Time.deltaTime;   

        // Переміщення
        if (move != Vector3.zero)
            rb.MovePosition(rb.position + move);

        // Обертання навколо Y
        if (Input.GetKey(KeyCode.UpArrow))
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime));
        if (Input.GetKey(KeyCode.DownArrow))
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * -rotationSpeed * Time.deltaTime));
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
            // Повертаємо штангу на старт
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
