using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeAnimation : MonoBehaviour
{
    public UnityEvent sinalSair;

    public void Sair(){
        sinalSair?.Invoke();
    }
}
