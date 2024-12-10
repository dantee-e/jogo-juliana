using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour{
    public UnityEvent sinalJogar;
    public UnityEvent sinalSair;

    // iniciar jogo
    public void Jogar(){
        sinalJogar?.Invoke();
    }

    // sair do jogo
    public void Sair(){
        sinalSair?.Invoke();
    }
}
