using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{

    public Camera cam;

    public Transform ballTransform;

    // Update is called once per frame
    void Update()
    {
        cam.transform.LookAt(ballTransform.position);
    }
}
