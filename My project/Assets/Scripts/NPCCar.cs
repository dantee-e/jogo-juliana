using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCar : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxCollider;

    // Distância do raycast
    public float raycastDistance = 10f;
    bool movement;
    public float velocidade = 5f;
    private bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        boxCollider = gameObject.AddComponent<BoxCollider>();
        //rb.isKinematic = true;
        movement = true;
        isWaiting = false;
    }

    void teleportToSpawn(RaycastHit hit){
        GameObject plano = hit.collider.gameObject;
        if (plano.transform.parent != null)
        {
            Transform parent = plano.transform.parent;
            Transform spawnPoint = parent.Find("SpawnPoint");
            if (spawnPoint == null){
                print("o spawn point nao foi encontrado");
                return;
            }
            rb.position = spawnPoint.position;
        }
        return;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 origem = transform.position + new Vector3(0, 1, 0) + transform.forward;
        Vector3 direcao = transform.forward * raycastDistance;

        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Ray ray = new Ray(origem, direcao);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out hit) && hit.distance < raycastDistance)
        {
            if ((hit.collider.CompareTag("carro") || hit.collider.CompareTag("red_light"))
                && hit.distance < 5 )
            {
                movement = false;
                isWaiting = false;
                StopAllCoroutines();
            }
            else if (hit.collider.CompareTag("green_light"))
            {
                print("luz verde");
                movement = true;
            }
            else if (hit.collider.CompareTag("yellow_light"))
            {
                print("luz amarela");
                if (hit.distance > 5)
                    movement = false;
            }
            else if (hit.collider.CompareTag("Finish"))
            {
                print("teleporting");
                teleportToSpawn(hit);
            }
        }
        else
        {
            // Inicia a corrotina de delay apenas uma vez, se o carro não estiver esperando
            if (!isWaiting && !movement)
            {
                StartCoroutine(DelayBeforeMoving());
            }
        }

        // Aplica movimento
        if (movement && !isWaiting)
            rb.velocity = transform.forward * velocidade;
        else
            rb.velocity = Vector3.zero;
    }

    IEnumerator DelayBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1.2f); // Espera 1 segundo
        movement = true;
        isWaiting = false;
    }
}
