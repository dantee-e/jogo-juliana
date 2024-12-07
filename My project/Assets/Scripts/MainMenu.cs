using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour{
    public UnityEvent sinalJogar;
    public UnityEvent sinalSair;

    public void sendSinal(string sinal){
        Debug.Log("Received signal: " + sinal); // Debug message to see if the signal is being passed
        switch (sinal){
            case "jogar":
                sinalJogar?.Invoke();
                break;
            case "sair":
                sinalSair?.Invoke();
                break;
        }
    }

    public void Jogar(){
        sendSinal("jogar");
    }

    // Fechar o jogo
    public void Sair(){
        sendSinal("sair");
    }
}
