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
            // Efeito visual ou som para indicar o checkpoint alcançado
            print("Checkpoint alcançado!");

            // Procura o objeto com a tag "Objetivo"
            GameObject objetivo = GameObject.FindWithTag("Objetivo");
            
            // Se encontrar o objetivo, destrói a instância
            if (objetivo != null)
            {
                Destroy(objetivo);
                print("Objetivo destruído!");
            }

            // Chama o GameManager para completar o nível
            // GameManager.instance.CompleteLevel();
        }
    }
}
