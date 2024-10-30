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
    public float velocidade = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        boxCollider = gameObject.AddComponent<BoxCollider>();
        //rb.isKinematic = true;
        movement = true;
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

        Debug.DrawRay(ray.origin, ray.direction*raycastDistance);

        if (Physics.Raycast(ray, out hit))
        {
            print("Colidindo");
            // Aqui você pode adicionar lógica, como parar o carro ao detectar um obstáculo
            if (hit.collider.CompareTag("green_light"))
            {
                movement = true;
            }
            else if (hit.collider.CompareTag("yellow_light"))
            {
                if (hit.distance > 26)
                    movement = false;
                else
                    movement = true;
            }
            else if (hit.collider.CompareTag("red_light"))
            {
                print("Parando");
                movement = false;
            }
        }

        if (movement)
            rb.velocity = transform.forward * velocidade;
        else
            rb.velocity = Vector3.zero;
    }
}
