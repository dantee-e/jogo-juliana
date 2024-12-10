using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCarSound : MonoBehaviour
{
    AudioSource audioSource;
    public float minPitch = 0.05f;
    private float pitchFromCar;

    private PlayerMovement playerMovement; // Referência automática ao PlayerMovement

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = minPitch;

        // Obtém o PlayerMovement automaticamente
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement não foi encontrado no mesmo GameObject. Certifique-se de que o script está presente.");
        }
    }

    void Update()
    {
        if (playerMovement != null)
        {
            pitchFromCar = playerMovement.carCurrentSpeed;
            audioSource.pitch = Mathf.Max(pitchFromCar, minPitch);
        }
    }
}
