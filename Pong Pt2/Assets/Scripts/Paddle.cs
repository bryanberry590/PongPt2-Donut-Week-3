using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    public float paddleSpeed = 12f;
    private Transform paddleTransform;
    private float movementZ;
    private PlayerInput playerInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paddleTransform = GetComponent<Transform>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = paddleTransform.position + new Vector3(0f, 0f, movementZ * paddleSpeed * Time.deltaTime);
        newPosition.z = Mathf.Clamp(newPosition.z,-5.5f, 5.5f);
        paddleTransform.position = newPosition;
        //Debug.Log(gameObject.name + " Moving: " + movementZ);
    }
    
    public void OnMoveLeftPaddle(InputAction.CallbackContext context)
    {
        //Debug.Log("moving left paddle");
         movementZ = context.ReadValue<float>();
    }
    
    public void OnMoveRightPaddle(InputAction.CallbackContext context)
    {
        //Debug.Log("moving right paddle");
        movementZ = context.ReadValue<float>();
    }

}