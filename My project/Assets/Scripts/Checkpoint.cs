using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int contadorMovimentos = 0; // Contador de mudanças de posição
    private Vector3[] posicoes; // Array de posições predefinidas para o objeto
    private GameObject objetivo;

    /*
    GameObject hud;
    GameObject textTempo, textPontos;

    // retorna true se o player ainda tem pontos
    bool changePoints(int pontos){
        if (textPontos!=null){
            textPontos.GetComponent<TMPro.TextMeshProUGUI>().text = "Pontos: " + (pontos*10).ToString();
        }
        if (pontos != 0)
            return true;
        return false;
    }

    void Start(){
        hud = gameObject.transform.Find("HUD").gameObject;
        textTempo = hud.transform.Find("Tempo").gameObject;
        textPontos = hud.transform.Find("Pontuacao").gameObject;
    }*/

    void Start()
    {
        // Define posições predefinidas no mapa (adicione conforme necessário)
        posicoes = new Vector3[] {
            new Vector3(240, 333, -3815),
            new Vector3(-68, 333, -2773),
            new Vector3(-230, 333, 1594)
        };

        

        // Procura o objetivo no início
        objetivo = GameObject.FindWithTag("Objetivo");

        // Move o objetivo para a primeira posição
        if (objetivo != null)
        {
            objetivo.transform.position = posicoes[0];
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