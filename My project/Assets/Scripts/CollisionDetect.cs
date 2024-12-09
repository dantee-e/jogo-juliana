using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    private bool canDetectCollision = true; // pro cooldown

    private int deducoes = 0;
    public int tempoRestante = 120;

    public int max_deducoes = 10;

    GameObject hud;
    GameObject textTempo, textPontos;

    // retorna true se o player ainda tem pontos
    bool changePoints(int deducoes){
        if (textPontos!=null){
            textPontos.GetComponent<TMPro.TextMeshProUGUI>().text = "Pontos: " + ((max_deducoes-deducoes)*10).ToString();
        }
        if (deducoes != 0)
            return true;
        return false;
    }

    void Start(){
        hud = gameObject.transform.Find("HUD").gameObject;
        textTempo = hud.transform.Find("Tempo").gameObject;
        textPontos = hud.transform.Find("Pontuacao").gameObject;

        StartCoroutine(updateTime());
    }

    void OnCollisionEnter(Collision c)
    {
        if (canDetectCollision && (c.gameObject.tag == "Ambiente" || c.gameObject.tag == "carro"))
        {
            deducoes++;
            print("Colidiu com o ambiente: " + c.gameObject.tag + " Colisões: " + deducoes);

            changePoints(deducoes);
            
            // cooldown antes da prox colisao
            StartCoroutine(Cooldown());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Objetivo")
            tempoRestante = 90;
        else if (other.gameObject.tag == "red_light"){
            float carRotationY = transform.eulerAngles.y;
            float planeRotationY = other.transform.eulerAngles.y;

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(carRotationY, planeRotationY));

            if (Mathf.Abs(angleDifference - 180f) < 90f){
                print("passou no vermelho safado");
                deducoes++;
                changePoints(deducoes);
            }
        }
    }

    private bool isRunning;
    private IEnumerator updateTime()
    {
        isRunning = true;
        while (isRunning)
        {
            tempoRestante -= 1;
            textTempo.GetComponent<TMPro.TextMeshProUGUI>().text = "Tempo Restante: " + tempoRestante.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Cooldown()
    {
        canDetectCollision = false;
        yield return new WaitForSeconds(1f);
        canDetectCollision = true;
    }
}
