using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public string nameGameScene = "cidades_juncao";
    private GameObject mainMenuObject;

    [SerializeField] GameObject mainMenuPrefab;

    public void playScene(string sceneName){
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    public void unloadScene(string sceneName){
        SceneManager.UnloadSceneAsync(sceneName);
    }
    public void quit(){
        Application.Quit();
    }

    public void startGame(){
        if (mainMenuObject != null){
            Destroy(mainMenuObject);
        }
        playScene(nameGameScene);
    }

    void Start(){
        mainMenuObject = Instantiate(mainMenuPrefab, transform.position, Quaternion.identity);
    
        MainMenu mainMenuScript = mainMenuObject.GetComponentInChildren<MainMenu>();
        
        if (mainMenuScript != null) {
            mainMenuScript.sinalJogar.AddListener(() => startGame());
            mainMenuScript.sinalSair.AddListener(() => quit());
        } else {
            Debug.LogWarning("MainMenu script not found on the object.");
        }
    }


    
}
