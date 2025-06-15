using UnityEngine;
using UnityEngine.SceneManagement;  // Para recarregar cena ou trocar de cena

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float visionRange = 5f;
    public float visionAngle = 60f;
    public Transform player;
    public Animator animator;

    private Vector3 targetPoint;
    private bool goingToPointB = true;

    void Start()
    {
        targetPoint = pointB.position;
    }

    void Update()
    {
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
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= visionRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= visionAngle / 2f)
            {
                Debug.Log("Jogador foi visto! Game Over!");
                GameOver();
            }
        }
    }

    void GameOver()
    {
        // Exemplo: reinicia a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Se quiser, pode colocar outras ações aqui:
        // - Abrir uma tela de "Você perdeu"
        // - Parar o jogo
        // - Mostrar uma animação de morte
    }
}
