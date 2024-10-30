using System.Collections;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    float intervalo = 5f;
    public bool vert_horiz;
    public int currentState;
    public enum State
    {
        Red,
        Green,
        Yellow
    };

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

        if (vert_horiz)
            ChangeTag("red_light");
        else
            ChangeTag("green_light");

        StartCoroutine(ChangeStateLight());
    }

    IEnumerator ChangeStateLight()
    {
        while (true)
        {
            if (planoFilho.tag == "green_light")
            {
                yield return new WaitForSeconds(2 * intervalo);
                ChangeTag("yellow_light");
            }
            else if (planoFilho.tag == "yellow_light")
            {
                yield return new WaitForSeconds(intervalo);
                ChangeTag("red_light");
            }
            else if (planoFilho.tag == "red_light")
            {
                yield return new WaitForSeconds(3 * intervalo);
                ChangeTag("green_light");
            }
        }
    }
}
