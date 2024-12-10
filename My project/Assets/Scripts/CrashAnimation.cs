using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrashAnimation : MonoBehaviour
{
    public UnityEvent sinalSair;

    public void Sair(){
        sinalSair?.Invoke();
    }
}
