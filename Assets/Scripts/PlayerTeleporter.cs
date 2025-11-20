using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{

    public void TeleportPlayerTo(Transform target)
    {
        if (target == null) return;

        transform.position = target.position;

        transform.rotation = Quaternion.Euler(0, target.rotation.eulerAngles.y, 0);
    }
}