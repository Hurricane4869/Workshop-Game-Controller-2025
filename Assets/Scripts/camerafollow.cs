using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referensi ke transform pemain
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Offset posisi kamera

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
