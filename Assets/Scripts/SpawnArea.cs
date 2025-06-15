using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject victoryScreen;  // Arraste aqui o painel de vitória (UI) no Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.hasSupply)
            {
                victoryScreen.SetActive(true);  // Mostra a tela de vitória
                Time.timeScale = 0f;            // Pausa o jogo (opcional)
            }
        }
    }
}
