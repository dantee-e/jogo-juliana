using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    private int collisions = 0;
    private bool canDetectCollision = true; // Flag para controlar o intervalo entre detecções

    void OnCollisionEnter(Collision c)
    {
        if (canDetectCollision && (c.gameObject.tag == "Ambiente" || c.gameObject.tag == "carro"))
        {
            collisions++;
            print("Colidiu com o ambiente: " + c.gameObject.tag + " Colisões: " + collisions);
            
            // Inicia o cooldown para detectar a próxima colisão
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        canDetectCollision = false; // Bloqueia novas detecções
        yield return new WaitForSeconds(1f); // Aguarda 1 segundo
        canDetectCollision = true; // Permite novas detecções
    }
}
