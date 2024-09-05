using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class semaforo : MonoBehaviour
{
    public bool eixo = false;
    Transform detectorPlayer, luzVermelha, luzAmarela, luzVerde;
    // Start is called before the first frame update


    IEnumerator changeState(){
        int estado; // 1 - vermelho // 2 - amarelo // 3 - verde
        if (eixo)
            estado = 3;
        else
            estado = 1;

        while(true){
            if (estado == 3){ // se ta verde
                yield return new WaitForSeconds(8f); 
                estado = 2;
                yield return new WaitForSeconds(2f);
                estado = 1;
            }
            else if(estado == 1){ // se ta vermelho
                yield return new WaitForSeconds(10f);
                estado = 3;
            }
        }
    }

    void Start()
    {
        detectorPlayer = transform.Find("Plane");
        luzVermelha = transform.Find("luzVermelha");
        luzAmarela = transform.Find("luzAmarela");
        luzVerde = transform.Find("luzVerde");
        detectorPlayer.GetComponent<Renderer>().enabled = false;

        StartCoroutine(changeState());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
