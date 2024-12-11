using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour{
    public UnityEvent sinalJogar;
    public UnityEvent sinalSair;

    public UnityEvent SetDificuldadeFacil;
    public UnityEvent SetDificuldadeNormal;
    public UnityEvent SetDificuldadeDificil;

    // iniciar jogo
    public void Jogar(){
        sinalJogar?.Invoke();
    }

    // sair do jogo
    public void Sair(){
        sinalSair?.Invoke();
    }

    public void setDificuldade(int dificuldade){
        switch (dificuldade){
            case 0:
                SetDificuldadeFacil?.Invoke();
                break;
            case 1:
                SetDificuldadeNormal?.Invoke();
                break;
            case 2:
                SetDificuldadeDificil?.Invoke();
                break;
        }
    }
}
