using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start no jogo
    public void Jogar() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Fechar o jogo
    public void Sair() {
        Application.Quit();
        Debug.Log("Jogador decidiu quitar");
    }
}
