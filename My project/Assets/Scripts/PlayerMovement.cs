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
    public float carMaxSpeed = 100f;
    public float carCurrentSpeed = 0f;


    private Rigidbody rb;

    Transform hud;
    Transform instrucoes;
    Transform acceptButton;

    GameObject textTempo;

    float currentAcceleration = 0;
    float currentBreakForce = 0;
    float currentTurnAngle = 0;

    public UnityEvent ganhou;
    public UnityEvent noTime;

    int tempo = 400;
    int acrescimoTempoObjetivo = 400;

    int objetivoNum = 0;
    int totalNiveis = 3;

    string contexto = "É tarde de sábado, véspera de Natal. Você está saindo do Bar do Tatu, onde passou a tarde com a rapazeada. Depois ter se divertido, você se lembra de que não comprou o peru pra ceia de natal! Resta correr contra o tempo para garantir que a ceia de Natal com sua família aconteça.\nO trajeto não será fácil. Você tem três tarefas a cumprir antes de chegar em casa:\n1. Deixar Dante na casa dele\n2.Buscar o peru da ceia de Natal\n3. Chegar em casa a tempo e vivo, e provar que não foi comprar cigarro\n\nNo caminho, obstáculos e trânsito desafiarão suas habilidades. Cuidado! Cada colisão tira pontos de sua pontuação total. Se seus pontos chegarem a zero, seu carro entra em combustão e o Natal vira fumaça – literalmente.";

    string[] historia = {"Dante salta do carro com um sorriso agradecido no rosto. \'Valeu demais, meu parceiro! Você é o cara! Feliz Natal pra nós!\' Ele dá um último aceno antes de desaparecer pela porta de casa.\nNão há tempo a perder. O sol está se pondo, e o próximo destino é o Tauste, para pegar o peru.\nCorra! (Mas obedeça as leis de trânsito)",
    "O açougueiro do Tauste entrega o peru com um sorriso caloroso, contagiado pelo espírito natalino (ele vai sair mais cedo do trampo) 'Aqui está! O peru está impecável, pronto para sua ceia. Você coloca o peru no banco do passageiro, e passa o cinto nele, esquecendo-se de passar por si mesmo. A adrenalina aumenta, o relógio está correndo, e sua família espera por você. Agora é hora de ir para casa e salvar o Natal (que você mesmo quase estragou)!" 
    };

    int dificuldade;
    public void setDificuldade(int i){
        dificuldade = i;
        tempo = tempo - i*100;
        acrescimoTempoObjetivo = acrescimoTempoObjetivo - i*100;
        alcool_no_sangue = 0.2f*i;
        print("set dificuldade Player Movement = " + i.ToString());
    }

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
                if (textComponent != null){
                    textComponent.enabled = false;
                }
                
            }
        }
        Time.timeScale = 1;
    }

    void newTutorial(string text){
        Time.timeScale = 0;
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
                if (textComponent != null){
                    textComponent.enabled = true;
                }
                    
            }
        }
    }

    public void chegouObjetivo(){
        if (objetivoNum<totalNiveis-1)
            newTutorial(historia[objetivoNum]);
        tempo = acrescimoTempoObjetivo;
        objetivoNum++;
        if (objetivoNum == totalNiveis){
            ganhou?.Invoke();
        }
    }


    private bool isRunning;
    private IEnumerator updateTime(){
        isRunning = true;
        while (isRunning)
        {
            // se acabou o tempo
            if (tempo==0){
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

        newTutorial(contexto);


        rb = GetComponent<Rigidbody>();
    }

    IEnumerator updateMovement(float currentAcceleration, float currentBreakForce, float currentTurnAngle) {
        yield return new WaitForSeconds(alcool_no_sangue); // alterar para definir o efeito do álcool

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

    void FixedUpdate(){
        carCurrentSpeed = (rb.velocity.magnitude * 3.6f) / carMaxSpeed; // velocidade atual em relação à velocidade máxima
    }

    void Update(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0){
            currentBreakForce = 0;
            currentAcceleration = acceleration * verticalInput;
        }
        else if (verticalInput < 0){
            currentBreakForce = 0;
            currentAcceleration = acceleration * 2 * verticalInput;
            
        }
        else{
            currentAcceleration = currentBreakForce = 0;
        }
        currentTurnAngle = maxTurnAngle * horizontalInput;

        StartCoroutine(updateMovement(currentAcceleration, currentBreakForce, currentTurnAngle));
    }


    
}
