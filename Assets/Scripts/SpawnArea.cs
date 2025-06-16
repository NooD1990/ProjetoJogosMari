using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject victoryScreen;  

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.hasSupply)
            {
                victoryScreen.SetActive(true);  
                Time.timeScale = 0f;            
            }
        }
    }
}
