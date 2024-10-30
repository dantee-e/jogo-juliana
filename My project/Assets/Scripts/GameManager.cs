using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject); // Mantém o GameManager em cenas futuras, se necessário
    //     }
    //     else
    //     {
    //         Destroy(gameObject); // Impede múltiplas instâncias
    //     }
    // }

    // Função para ser chamada quando o carro atingir o objetivo
    public void CompleteLevel()
    {
        // Debug.Log("Objetivo final atingido! Nível Completo!");
        print("Objetivo final atingido! Nível Completo!");
        // Aqui você pode adicionar qualquer ação, como carregar uma nova cena ou mostrar uma tela de vitória
        // Exemplo: SceneManager.LoadScene("NextLevel");
    }
}

