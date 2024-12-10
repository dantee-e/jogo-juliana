using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float currentAcceleration = 0;
    float currentBreakForce = 0;
    float currentTurnAngle = 0;




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

 

    // Start is called before the first frame update
    void Start()
    {
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
    void Update()
    {
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
