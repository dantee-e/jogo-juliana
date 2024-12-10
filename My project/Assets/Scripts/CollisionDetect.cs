using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetect : MonoBehaviour
{
    private bool canDetectCollision = true; // pro cooldown

    private int deducoes = 0;
    public int tempoInicio = 120;
    public int acrescimoTempoObjetivo = 90;

    public int max_deducoes = 3;

    GameObject hud;
    GameObject textTempo, textPontos;

    public UnityEvent noPoints;
    public UnityEvent noTime;

    // retorna true se o player ainda tem pontos
    bool changePoints(int deducoes){
        if (textPontos!=null){
            textPontos.GetComponent<TMPro.TextMeshProUGUI>().text = "Pontos: " + ((max_deducoes-deducoes)*10).ToString();
        }
        if (deducoes != max_deducoes)
            return true;
        return false;
    }

    void Start(){
        hud = gameObject.transform.Find("HUD").gameObject;
        textTempo = hud.transform.Find("Tempo").gameObject;
        textPontos = hud.transform.Find("Pontuacao").gameObject;

        StartCoroutine(updateTime());
    }

    void OnCollisionEnter(Collision c){
        if (canDetectCollision && (c.gameObject.tag == "Ambiente" || c.gameObject.tag == "carro"))
        {
            deducoes++;

            // se zerou os pontos
            if (!changePoints(deducoes)){
                noPoints?.Invoke();
            }
            
            // cooldown antes da prox colisao
            StartCoroutine(Cooldown());
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Objetivo")
            tempoInicio = acrescimoTempoObjetivo;
        else if (other.gameObject.tag == "red_light"){
            float carRotationY = transform.eulerAngles.y;
            float planeRotationY = other.transform.eulerAngles.y;

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(carRotationY, planeRotationY));

            if (Mathf.Abs(angleDifference - 180f) < 90f){
                deducoes++;
                changePoints(deducoes);
            }
        }
    }

    private bool isRunning;
    private IEnumerator updateTime(){
        isRunning = true;
        while (isRunning)
        {
            //Se acabou o tempo
            if (tempoInicio==0){
                noTime?.Invoke();
            }

            tempoInicio -= 1;
            textTempo.GetComponent<TMPro.TextMeshProUGUI>().text = "Tempo Restante: " + tempoInicio.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Cooldown(){
        canDetectCollision = false;
        yield return new WaitForSeconds(1f);
        canDetectCollision = true;
    }
}
