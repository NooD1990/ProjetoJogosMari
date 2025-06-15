using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float visionRange = 5f;
    public float visionAngle = 60f;
    public Transform player;
    public Animator animator;

    public GameObject gameOverPanel;  // Arraste o painel Game Over no Inspector

    private Vector3 targetPoint;
    private bool goingToPointB = true;
    private bool isGameOver = false;

    void Start()
    {
        targetPoint = pointB.position;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;

        // Trava e esconde o cursor durante o jogo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isGameOver) return;

        Patrol();
        CheckPlayerInVision();
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);

        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        Vector3 direction = targetPoint - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            goingToPointB = !goingToPointB;
            targetPoint = goingToPointB ? pointB.position : pointA.position;
        }
    }

    void CheckPlayerInVision()
    {
        if (isGameOver) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= visionRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= visionAngle / 2f)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Jogador foi visto! Game Over!");
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        // Liberar cursor para poder clicar no UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        isGameOver = false;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;

        // Trava e esconde o cursor para voltar ao jogo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
