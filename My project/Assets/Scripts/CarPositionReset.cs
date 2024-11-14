using UnityEngine;

public class CarPositionReset : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float flipTimer = 0f;
    private float flipThreshold = 5f; // Tempo em segundos para restaurar posição
    private float angleThreshold = 60f; // Ângulo máximo de inclinação para considerar tombado

    private void Start()
    {
        // Armazena a posição e rotação iniciais do carro
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Verifica se o carro está tombado ou de ponta cabeça
        if (IsFlipped())
        {
            // Incrementa o timer de "tombado"
            flipTimer += Time.deltaTime;

            // Se passou do limite, restaura a posição e rotação iniciais
            if (flipTimer >= flipThreshold)
            {
                ResetToInitialPosition();
            }
        }
        else
        {
            // Se o carro está em posição normal, reinicia o timer
            flipTimer = 0f;
        }
    }

    private bool IsFlipped()
    {
        // Verifica o ângulo entre o vetor "up" do carro e o vetor "up" global
        float angle = Vector3.Angle(transform.up, Vector3.up);
        return angle > angleThreshold;
    }

    private void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        flipTimer = 0f; // Reseta o timer após a restauração
    }
}