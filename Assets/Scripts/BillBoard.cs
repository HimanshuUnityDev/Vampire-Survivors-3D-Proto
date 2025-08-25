using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cam;

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main.transform;
        }
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
