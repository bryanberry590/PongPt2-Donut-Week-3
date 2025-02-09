using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    private Vector3 ballVelocity;
    public float ballSpeed = 7f;
    private Rigidbody rb;
    private float prevFrameX;
    private float prevFrameZ;
    private float startXVelocity = -1;
    private GameObject paddle1;
    private GameObject paddle2;
    
    // text for score
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI restartButton;
    private GameObject buttonImg;
    
    // Audiomanager Reference
    private AudioManager audioManager;
        
    // goal/score variables
    private int leftScore;
    private int rightScore;
    
    public GameObject powerUpPrefab;
    public GameObject powerUp2Prefab;
    

    void Start()
    {
        Instantiate(powerUpPrefab, powerUpPrefab.transform.position, Quaternion.identity);
        Instantiate(powerUp2Prefab, powerUp2Prefab.transform.position, Quaternion.identity);
        buttonImg = GameObject.FindGameObjectWithTag("Button");
        buttonImg.SetActive(false);
        restartButton.enabled = false;
        winText.enabled = false;
        leftScore = 0;
        rightScore = 0;
        scoreText.text = leftScore.ToString() + " - " + rightScore.ToString();
        scoreText.color = Color.magenta;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(ballSpeed * startXVelocity, 0f, 0.0f); // the initial ball velocity, goes towards left paddle
        audioManager = FindFirstObjectByType<AudioManager>();
        paddle1 = GameObject.Find("LeftPaddle");
        paddle2 = GameObject.Find("RightPaddle");
        
    }
    
    void FixedUpdate()
    {
        prevFrameX = rb.linearVelocity.x;
        prevFrameZ = rb.linearVelocity.z;
        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed; // keeps the speed of the ball constant by multiplying the normalized velocity by ballspeed everyframe
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            gameObject.SetActive(false);
            Score(other.gameObject);
            // Debug.Log("ball scored");
        }
        else
        {
            Vector3 newVelocity = new Vector3(prevFrameX, 0f, prevFrameZ );
            if (other.gameObject.CompareTag("Wall"))
            {
                newVelocity.z *= -1;
                rb.linearVelocity = newVelocity.normalized * ballSpeed;
                audioManager.Play("wallHit");
            }

            if (other.gameObject.CompareTag("Player"))
            {
                //AudioSource.PlayClipAtPoint(hitmarkerSound, transform.position, 3f);
                audioManager.Play("PaddleHit");
                audioManager.PitchUp("PaddleHit", (ballSpeed * 0.1f)/2);
                newVelocity.x *= -1; // flips the x velocity of the ball

                // find the collision position Z relative to the paddles center
                float impactPositionZ = (transform.position.z - other.transform.position.z) / (other.collider.bounds.size.z / 2);
            
                // this will update the z velocity based on the impact pos and prev frame z velocity
                newVelocity.z = impactPositionZ * Mathf.Abs(prevFrameX); // this will scale the impact position z by the prev frames x velocity's abs value


                ballSpeed += 2f;
                rb.linearVelocity = newVelocity.normalized * ballSpeed;

            }
        }
    }

    void Score(GameObject goal)
    {
        if (goal.name == "LeftGoal")
        {
            startXVelocity = -1;
            rightScore++;
            scoreText.text = leftScore + " - " + rightScore;
            Debug.Log($"Right Scores! The current score is {leftScore} - {rightScore}");
        }
        else if (goal.name == "RightGoal")
        {
            leftScore++;
            startXVelocity = 1;
            scoreText.text = leftScore + " - " + rightScore;
            Debug.Log($"Left Scores! The current score is {leftScore} - {rightScore}");
        }

        if (leftScore > rightScore)
        {
            scoreText.color = Color.green;
        }
        else if (leftScore < rightScore)
        {
            scoreText.color = Color.yellow;
        }
        else
        {
            scoreText.color = Color.magenta;
        }
        
        audioManager.Reset("PaddleHit");
        WinGameCheck();
    }

    void WinGameCheck()
    {
        //if win, restart game, else call to reset ball to center and have ball go to the prev point winner
        if (leftScore == 11)
        {
            buttonImg.SetActive(true);
            winText.enabled = true;
            restartButton.enabled = true;
            winText.text = "Left Player Wins!";
            Debug.Log("Game Over, Left Wins!");
            CancelInvoke(nameof(SpawnBall));
        }
        else if (rightScore == 11)
        {
            buttonImg.SetActive(true);
            winText.enabled = true;
            restartButton.enabled = true;
            winText.text = "Right Player Wins!";
            Debug.Log("Game Over, Right Wins!");
            CancelInvoke(nameof(SpawnBall));
        }
        else
        {
            Invoke(nameof(SpawnBall), 1f);
        }
    }

    public void OnRestartButton()
    {
        //Debug.Log("Restarting Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    void SpawnBall()
    {
        //Debug.Log("Spawning Ball");
        gameObject.transform.position = new Vector3(10f, 0.5f, 0f);
        gameObject.SetActive(true);
        ballSpeed = 7f;
        rb.linearVelocity = new Vector3(startXVelocity, 0f, 0.0f);
        ResetPaddle();
    }

    void ResetPaddle()
    {
        paddle1.transform.position = new Vector3(1f, 0.6f, 0f);
        paddle2.transform.position = new Vector3(19f, 0.6f, 0f);
    }
}
