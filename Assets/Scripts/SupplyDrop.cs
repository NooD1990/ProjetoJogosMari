using UnityEngine;

public class SupplyDrop : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.hasSupply = true;  // Jogador pegou o item
                Destroy(gameObject);      // Some com o supply drop
            }
        }
    }
}
