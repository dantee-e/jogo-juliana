using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public UnityEvent sinalJogar;
    public UnityEvent sinalSair;

    public void sendSinal(string sinal)
    {
        switch(sinal){
            case "jogar":
                sinalJogar?.Invoke();
                break;
            case "sair":
                sinalSair?.Invoke();
                break;
        }
    }

    public void Jogar() {
        sendSinal("jogar");
    }

    // Fechar o jogo
    public void Sair() {
        sendSinal("sair");
    }
}
