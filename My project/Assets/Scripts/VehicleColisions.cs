using UnityEngine;

public class CarCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o jogador!");
            // Aqui você pode adicionar efeitos de colisão, como som ou partículas
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Colidiu com um obstáculo!");
            // Ação a ser realizada ao colidir com um obstáculo
        }
    }
}
