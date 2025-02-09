using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class PowerUp : MonoBehaviour
{
    private Rigidbody rb;
    private float newZPos;
    public float rotateDegreesPerSecond = 45f;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void Update()
    {
        transform.Rotate(new Vector3(rotateDegreesPerSecond, rotateDegreesPerSecond, rotateDegreesPerSecond) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("PowerUp triggered");
        audioManager.Play("metalClang");
        if (other.CompareTag("Ball"))
        {
            
            if (gameObject.CompareTag("FlipZ"))
            {
                rb = other.GameObject().GetComponent<Rigidbody>();
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z * -1);
            }
            else if (gameObject.CompareTag("SlowBall"))
            {
                Ball ball = other.GetComponent<Ball>();
                ball.ballSpeed -= 4;
                if (ball.ballSpeed < 4)
                    ball.ballSpeed = 4;
            }
            gameObject.SetActive(false);
            Invoke(nameof(PowerUpRespawn),7f);
        }
    }

    private void PowerUpRespawn()
    {
        Random random = new Random();
        int rand = random.Next(-6, 6);
        transform.position = new Vector3(transform.position.x, transform.position.y, rand);
        gameObject.SetActive(true);
    }
}
