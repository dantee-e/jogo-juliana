using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    private Transform objetivo;
    public Vector3 rotationOffset; // Permite ajustar manualmente a compensação no Inspector

    void Start()
    {
        // Encontrar o GameObject com a tag "Objetivo"
        GameObject objetivoObj = GameObject.FindGameObjectWithTag("Objetivo");

        if (objetivoObj != null)
        {
            objetivo = objetivoObj.transform;
        }
        else
        {
            Debug.LogError("Nenhum GameObject com a tag 'Objetivo' foi encontrado na cena.");
        }

        // Definir um valor padrão para o offset caso não seja configurado
        if (rotationOffset == Vector3.zero)
        {
            rotationOffset = new Vector3(90, 0, 0); // Ajustar conforme necessário
        }
    }

    void Update()
    {
        if (objetivo != null)
        {
            // Calcular a direção do objetivo
            Vector3 directionToTarget = objetivo.position - transform.position;

            // Manter a rotação no plano XZ (ignorar a inclinação no eixo Y)
            directionToTarget.y = 0;

            // Verificar se a direção é válida para evitar problemas com vetores nulos
            if (directionToTarget.sqrMagnitude > 0.01f)
            {
                // Calcular a rotação necessária para apontar para o objetivo
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                // Adicionar a compensação de rotação
                targetRotation *= Quaternion.Euler(rotationOffset);

                // Aplicar a rotação suavemente (opcional)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
