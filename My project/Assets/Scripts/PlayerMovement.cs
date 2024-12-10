using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] float acceleration = 500f;
    [SerializeField] float breakingForce = 1000f;
    [SerializeField] float maxTurnAngle = 15f;

    public float alcool_no_sangue = 0.0f;

    private Rigidbody rb;

    Transform hud;
    Transform instrucoes;
    Transform acceptButton;

    GameObject textTempo;

    float currentAcceleration = 0;
    float currentBreakForce = 0;
    float currentTurnAngle = 0;

    public UnityEvent noTime;
    public int tempo = 120;
    public int acrescimoTempoObjetivo = 90;

    int objetivoNum = 0;



    public void hideTutorial(){
        // define o texto como nada
        if (instrucoes != null){
            var textMeshPro = instrucoes.GetComponent<TMPro.TextMeshProUGUI>();
            if (textMeshPro != null){
                textMeshPro.text = "";
            }
        }
        // desativa o butao
        if (acceptButton != null){
            var button = acceptButton.GetComponent<UnityEngine.UI.Button>();
            if (button != null){
                button.interactable = false;

                // oculta o texto do butao
                var textComponent = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                    textComponent.gameObject.SetActive(false);

                
            }
        }
    }

    void newTutorial(string text){
        // define o texto como as novas instrucoes
        if (instrucoes != null){
            var textMeshPro = instrucoes.GetComponent<TMPro.TextMeshProUGUI>();
            if (textMeshPro != null){
                textMeshPro.text = text;
            }
        }
        // ativa o butao
        if (acceptButton != null){
            var button = acceptButton.GetComponent<UnityEngine.UI.Button>();
            if (button != null){
                button.interactable = true;

                var textComponent = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                    textComponent.gameObject.SetActive(true);
            }
        }
    }

    public void chegouObjetivo(){
        tempo = 90;
        newTutorial("checkpoint numero " + tempo.ToString());
    }


    private bool isRunning;
    private IEnumerator updateTime(){
        isRunning = true;
        while (isRunning)
        {
            // se acabou o tempo
            if (tempo==0){
                print("acabou tempo");
                noTime?.Invoke();
            }

            tempo -= 1;
            textTempo.GetComponent<TMPro.TextMeshProUGUI>().text = "Tempo Restante: " + tempo.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
 

    // Start is called before the first frame update
    void Start() {
        hud = gameObject.transform.Find("HUD");
        textTempo = hud.gameObject.transform.Find("Tempo").gameObject;
        

        StartCoroutine(updateTime());

        foreach (Transform child in transform) {
            if (child.name == "HUD") {
                hud = child;
            }
        }
        foreach (Transform child in hud) {
            if (child.name == "Instrucoes") {
                instrucoes = child;
            }
            if (child.name == "Button") {
                acceptButton = child;
            }
        }


        rb = GetComponent<Rigidbody>();
    }


    IEnumerator updateMovement(float currentAcceleration, float currentBreakForce, float currentTurnAngle){
        yield return new WaitForSeconds(alcool_no_sangue); // mudar isso para definir o quao doido ta 
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;


        
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }

    // Update is called once per frame
    void Update(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if(verticalInput>0){
            currentBreakForce = 0;
            currentAcceleration = acceleration * verticalInput;
        }
        else if(verticalInput<0){
            currentAcceleration = 0;
            currentBreakForce = -breakingForce * verticalInput;
        }
        else{
            currentAcceleration = currentBreakForce = 0;    
        }
        currentTurnAngle = maxTurnAngle * horizontalInput;
            
        StartCoroutine(updateMovement(currentAcceleration, currentBreakForce, currentTurnAngle));
    }


    
}
