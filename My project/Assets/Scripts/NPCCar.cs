using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCar : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxCollider;

    // Distância do raycast
    public float raycastDistance = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        boxCollider = gameObject.AddComponent<BoxCollider>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origem = transform.position + transform.forward * 1.5f; // Move o ponto de origem um pouco à frente do carro
        Vector3 direcao = transform.forward;

        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Ray ray = new Ray(transform.position +new Vector3(0, 0.3f, 0), transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction*10);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Objeto detectado: " + hit.collider.name);

            // Aqui você pode adicionar lógica, como parar o carro ao detectar um obstáculo
            if (hit.collider.CompareTag("green_light"))
            {
                print("green light");
            }
            else if (hit.collider.CompareTag("yellow_light"))
            {
                print("yellow light");
            }
            else if (hit.collider.CompareTag("red_light"))
            {
                print("red light");
            }
        }
    }
}
