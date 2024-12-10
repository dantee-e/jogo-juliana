using System.Collections;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    float intervalo = 7f;
    public bool vert_horiz;
    public int currentState;
    

    [SerializeField] Light luzVermelha;
    [SerializeField] Light luzAmarela;
    [SerializeField] Light luzVerde;

    public enum luzAtiva {
        Vermelha,
        Amarela,
        Verde
    };

    luzAtiva estado;

    public bool debug;


    // Referência ao objeto filho que representa a luz do semáforo
    private GameObject planoFilho;

    void ChangeTag(string newTag)
    {
        // Verifica se a referência do filho e a tag são válidas antes de aplicar
        if (planoFilho != null && !string.IsNullOrEmpty(newTag))
        {
            planoFilho.tag = newTag;
        }
    }

    void Start()
    {
        // Atribui o objeto filho específico, por exemplo "luzAmarela"
        planoFilho = transform.Find("Plane").gameObject;
        planoFilho.GetComponent<Renderer>().enabled = false;

        Transform parentTransform = transform.parent;

        if (parentTransform.tag == "column"){
            estado = luzAtiva.Vermelha;
            ChangeTag("red_light");
        }
        else if (parentTransform.tag == "row"){
            estado = luzAtiva.Verde;
            ChangeTag("green_light");
        }
/*
        if (vert_horiz){
            estado = luzAtiva.Vermelha;
            ChangeTag("red_light");
        }
            
        else{
            estado = luzAtiva.Verde;
            ChangeTag("green_light");
        }
          */  

        StartCoroutine(ChangeStateLight()); 
    }

    void acenderLuzes(){
        switch (estado)
        {
            case luzAtiva.Verde:
                if (debug)
                    print("Luz ta verde");
                luzVerde.enabled = true;
                luzAmarela.enabled = false;
                luzVermelha.enabled = false;
                break;

            case luzAtiva.Amarela:
                if (debug)
                    print("Luz ta amarela");
                luzVerde.enabled = false;
                luzAmarela.enabled = true;
                luzVermelha.enabled = false;
                break;

            case luzAtiva.Vermelha:
                if (debug)
                    print("Luz ta vermelha");
                luzVerde.enabled = false;
                luzAmarela.enabled = false;
                luzVermelha.enabled = true;
                break;
        }
    }



    IEnumerator ChangeStateLight()
    {
        while (true)
        {
            acenderLuzes();
            switch (estado){
                case luzAtiva.Verde:
                    yield return new WaitForSeconds(2 * intervalo);
                    ChangeTag("yellow_light");
                    estado = luzAtiva.Amarela;
                    break;

                case luzAtiva.Amarela:
                    yield return new WaitForSeconds(intervalo);
                    ChangeTag("red_light");
                    estado = luzAtiva.Vermelha;
                    break;
                
                case luzAtiva.Vermelha:
                    yield return new WaitForSeconds(3 * intervalo);
                    ChangeTag("green_light");
                    estado = luzAtiva.Verde;
                    break;
            }
        }
    }
}
