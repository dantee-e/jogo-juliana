using System.Collections;
using System.Collections.Generic;
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
    void ChangeTag(string newTag)
    {
        // Verifica se a tag que você deseja atribuir existe antes de aplicar
        if (newTag != null && !string.IsNullOrEmpty(newTag) && gameObject != null)
        {
            gameObject.tag = newTag;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
            if (tag == "green_light")
            {
                yield return new WaitForSeconds(2*intervalo);
                ChangeTag("yellow_light");
            }
            else if (tag == "yellow_light")
            {
                yield return new WaitForSeconds(intervalo);
                ChangeTag("red_light");
            }
            else if (tag == "red_light")
            {
                yield return new WaitForSeconds(3*intervalo);
                ChangeTag("green_light");
            };
        }
    }

}
