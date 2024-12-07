using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public string nameGameScene = "cidades_juncao";

    public string mainMenuSceneName = "MainMenuScene";
    public string mainMenuObjectName = "MainMenu";


    public void playScene(string sceneName){
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void unloadScene(string sceneName){
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void quit(){
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void startGame(){
        unloadScene(mainMenuSceneName);
        playScene(nameGameScene);
    }

    void Start(){
        playScene(mainMenuSceneName);

        SceneManager.sceneLoaded += OnMainMenuSceneLoaded;
    }

    private void OnMainMenuSceneLoaded(Scene scene, LoadSceneMode mode){
        if (scene.name == mainMenuSceneName){
            GameObject mainMenuObject = GameObject.Find(mainMenuObjectName);

            if (mainMenuObject != null){
                Debug.Log("MainMenu object found: " + mainMenuObject.name);

                // Get the MainMenu script attached to the object
                MainMenu mainMenuScript = mainMenuObject.GetComponent<MainMenu>();

                if (mainMenuScript != null){
                    Debug.Log("Linking signal to playScene");
                    // Subscribe to the signal
                    mainMenuScript.sinalJogar.AddListener(() => startGame());
                    mainMenuScript.sinalSair.AddListener(() => quit());
                }
                else{
                    Debug.LogWarning("MainMenu script not found on the object.");
                }
            }
            else{
                Debug.LogWarning("MainMenu object not found.");
            }
        }
    }

    
}
