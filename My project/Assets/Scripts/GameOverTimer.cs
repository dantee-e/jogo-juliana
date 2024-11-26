using UnityEngine;

public class GameOverTimer : MonoBehaviour
{
    public float timeLimit = 60f; // Tempo limite em segundos para alcançar o objetivo
    private float timer = 0f;
    private bool gameEnded = false;

    private void Update()
    {
        // Se o jogo não acabou, incrementa o timer
        if (!gameEnded)
        {
            timer += Time.deltaTime;

            // Verifica se o tempo limite foi alcançado
            if (timer >= timeLimit)
            {
                GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se o carro colidir com o objetivo, encerra o jogo
        if (other.CompareTag("Objetivo"))
        {
            Debug.Log("Objetivo alcançado!");
            gameEnded = true;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Tempo esgotado.");
        gameEnded = true;
    }
}