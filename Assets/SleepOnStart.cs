using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SleepOnStart : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.Sleep();
    }
}