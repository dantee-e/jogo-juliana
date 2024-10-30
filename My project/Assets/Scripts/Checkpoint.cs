using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no checkpoint é o player
        if (other.gameObject.tag == "Jogador")
        {
            // Você pode adicionar um efeito visual ou som aqui para indicar que o checkpoint foi ativado
            // Debug.Log("Checkpoint alcançado!");
            print("Checkpoint alcançado!");

            // Chama o GameManager para completar o nível
            // GameManager.instance.CompleteLevel();
        }
    }
}
