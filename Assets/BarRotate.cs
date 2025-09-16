using UnityEngine;

public class BarRotate : MonoBehaviour
{

    public float rotationSpeed = 100f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
