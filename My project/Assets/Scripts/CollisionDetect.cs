using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetect : MonoBehaviour
{
    private bool canDetectCollision = true; // pro cooldown

    private int deducoes = 0;
    

    public int max_deducoes = 10;

    GameObject hud;
    GameObject textPontos;

    public UnityEvent noPoints;
    

    PlayerMovement playerMovementScript;

    private AudioSource collisionSound;

    void Start(){
        hud = gameObject.transform.Find("HUD").gameObject;
        textPontos = hud.transform.Find("Pontuacao").gameObject;

        playerMovementScript = gameObject.GetComponent<PlayerMovement>();


        // Configurando o AudioSource de índice 3
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length > 2)
        {
            collisionSound = audioSources[2];
        }
        else
        {
            Debug.LogWarning("Não há AudioSource de índice 2 no GameObject!");
        }
    }

    // retorna true se o player ainda tem pontos
    bool changePoints(int deducoes){
        if (textPontos != null){
            textPontos.GetComponent<TMPro.TextMeshProUGUI>().text = "Pontos: " + ((max_deducoes - deducoes) * 10).ToString();
        }
        if (deducoes != max_deducoes)
            return true;
        return false;
    }

    

    void OnCollisionEnter(Collision c){
        if (canDetectCollision && (c.gameObject.tag == "Ambiente" || c.gameObject.tag == "carro")){
            deducoes++;

            // Toca o som de colisão
            if (collisionSound != null){
                collisionSound.Play();
            }
            // se zerou os pontos
            if (!changePoints(deducoes)){
                noPoints?.Invoke();
            }

            // cooldown antes da prox colisao
            StartCoroutine(Cooldown());
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Objetivo"){
            playerMovementScript.chegouObjetivo();
        }

        else if (other.gameObject.tag == "red_light")
        {
            float carRotationY = transform.eulerAngles.y;
            float planeRotationY = other.transform.eulerAngles.y;

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(carRotationY, planeRotationY));

            if (Mathf.Abs(angleDifference - 180f) < 90f){
                deducoes++;
                if (!changePoints(deducoes)){
                    noPoints?.Invoke();
                }
            }
        }
    }

    

    private IEnumerator Cooldown()
    {
        canDetectCollision = false;
        yield return new WaitForSeconds(1f);
        canDetectCollision = true;
    }
}
