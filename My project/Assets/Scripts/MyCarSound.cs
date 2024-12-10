using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCarSound : MonoBehaviour
{
    private AudioSource[] audioSources;
    public int engineSoundIndex = 0; // Índice do som do motor
    public float minPitch = 0.05f;
    private float pitchFromCar;

    public PlayerMovement playerMovement; // Referência ao PlayerMovement

    void Start()
    {
        audioSources = GetComponents<AudioSource>();

        if (audioSources.Length == 0)
        {
            Debug.LogError("Nenhum AudioSource encontrado neste GameObject.");
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement não está atribuído ao MyCarSound. Atribua o componente na inspeção.");
        }
    }

    void Update()
    {
        if (playerMovement != null && audioSources.Length > engineSoundIndex)
        {
            pitchFromCar = playerMovement.carCurrentSpeed;
            audioSources[engineSoundIndex].pitch = Mathf.Max(pitchFromCar, minPitch);
        }
    }
}
