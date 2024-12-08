using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float tempoRestante; // Variável pública para controle do tempo restante
    private int contadorMovimentos = 0; // Contador de mudanças de posição
    private Vector3[] posicoes; // Array de posições predefinidas para o objeto
    private GameObject objetivo;

    void Start()
    {
        // Define posições predefinidas no mapa (adicione conforme necessário)
        posicoes = new Vector3[] {
            new Vector3(240, 333, -3815),
            new Vector3(-68, 333, -2773),
            new Vector3(-230, 333, 1594)
        };

        // Tempo inicial configurado (ajustável no Inspector ou em outro script)
        tempoRestante = 120f;

        // Procura o objetivo no início
        objetivo = GameObject.FindWithTag("Objetivo");

        // Move o objetivo para a primeira posição
        if (objetivo != null)
        {
            objetivo.transform.position = posicoes[0];
        }
    }

    void Update()
    {
        // Reseta o tempo quando o objetivo muda de posição
        if (tempoRestante > 0)
        {
            tempoRestante -= Time.deltaTime;
            // print("Tempo restante: " + tempoRestante);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no checkpoint é o player
        if (other.gameObject.tag == "Jogador")
        {
            // Verifica se há mais posições para mover o objetivo
            if (contadorMovimentos < posicoes.Length - 1 && objetivo != null)
            {
               contadorMovimentos++; // Incrementa antes de mover
               objetivo.transform.position = posicoes[contadorMovimentos]; // Move o objetivo
                tempoRestante = 90f; // Reseta o tempo

               print($"Objetivo movido para a posição {contadorMovimentos}!");
            }
            else if (contadorMovimentos >= posicoes.Length - 1 && objetivo != null)
            {
                Destroy(objetivo); // Destroi o objetivo após a última posição
                print("Objetivo destruído após 3 movimentos!");
            }
        }
    }

}