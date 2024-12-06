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
    public float velocidade = 7f;
    private bool isWaiting;

    public float ajusteDirecao = 2f;  // Força do ajuste automático da direção
    public LayerMask pistaLayer;  // Layer que representa a pista

    [SerializeField]
    private Material[] carMaterials;
    private Renderer carRenderer;

    public bool debug = false;


    private Quaternion starting_rotation;
    private bool crava_x;

    // Start is called before the first frame update
    void Start(){
        starting_rotation = transform.rotation;

        rb = gameObject.AddComponent<Rigidbody>();
        boxCollider = gameObject.AddComponent<BoxCollider>();
        movement = true;
        isWaiting = false;

        carRenderer = GetComponent<Renderer>();

        if (carMaterials.Length > 0)
        {
            int randomIndex = Random.Range(0, carMaterials.Length);
            carRenderer.material = carMaterials[randomIndex];
        }
        else
        {
            Debug.LogWarning("Nenhum material foi atribuído ao array carMaterials.");
        }
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
    void AjustarDirecao(){
        transform.rotation = starting_rotation;
    
    }



    void dealWithRayCast(Ray ray, RaycastHit hit){
        GameObject hitObject = hit.collider.gameObject;

        if (hitObject.CompareTag("red_light") || hitObject.CompareTag("yellow_light") || hitObject.CompareTag("green_light")){
            float carRotationY = transform.eulerAngles.y;
            float planeRotationY = hitObject.transform.eulerAngles.y;

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(carRotationY, planeRotationY));

            float oppositeMargin = 10f;

            if (debug){
                print("Angle difference: " + angleDifference);
            }

            // If the car and light are facing opposite directions (roughly 180 degrees apart)
            if (Mathf.Abs(angleDifference - 180f) <= oppositeMargin) {
                switch (hitObject.tag){
                    case "green_light":
                        movement = true; // Car can move if the light is green
                        if (debug)
                            print("Green light: Car continues moving.");
                        break;
                    case "yellow_light":
                        movement = false; // Car should stop if the light is yellow
                        if (debug)
                            print("Yellow light: Car stops.");
                        break;
                    case "red_light":
                        movement = false; // Car should stop if the light is red
                        if (debug)
                            print("Red light: Car stops.");
                        break;
                }
            }
        }
        else if (hit.collider.CompareTag("carro") && hit.distance < 5){
            movement = false;
            isWaiting = false;
            StopAllCoroutines();
        }
        else if (hit.collider.CompareTag("Finish")){
            teleportToSpawn(hit);
        }
    }

    void Update(){
        AjustarDirecao();

        Vector3 origem = transform.position + new Vector3(0, 1, 0) + transform.forward;
        Vector3 direcao = transform.forward * raycastDistance;

        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Ray ray = new Ray(origem, direcao);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out hit) && hit.distance < raycastDistance)
        {
            dealWithRayCast(ray, hit);
        }
        else
        {
            // inicia a corrotina de delay apenas uma vez, se o carro nao estiver esperando
            if (!isWaiting && !movement)
            {
                StartCoroutine(DelayBeforeMoving());
            }
        }

        // aplica movimento
        if (movement && !isWaiting)
            rb.velocity = transform.forward * velocidade;
        else
            rb.velocity = Vector3.zero;

        
    }

    IEnumerator DelayBeforeMoving(){
        isWaiting = true;
        yield return new WaitForSeconds(1.2f); // Espera 1 segundo
        movement = true;
        isWaiting = false;
    }
}
