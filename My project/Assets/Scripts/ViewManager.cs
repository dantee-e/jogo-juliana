using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public string nameGameScene = "cidades_juncao";
    private GameObject mainMenuObject;
    private bool is_in_game = false;
    // todo implementar menu de pausa usando essa variave ^

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
            print("destroying menu");
            Destroy(mainMenuObject);
        }
        if(!is_in_game){
            playScene(nameGameScene);
            is_in_game = true;
        }
        Time.timeScale = 1;
            
    }

    void instantiateMenu(){
        if (mainMenuObject != null) {
            return;
        }

        Time.timeScale = 0;
        mainMenuObject = Instantiate(mainMenuPrefab, transform.position, Quaternion.identity);

        MainMenu mainMenuScript = mainMenuObject.GetComponentInChildren<MainMenu>();
        if (mainMenuScript != null) {
            mainMenuScript.sinalJogar.AddListener(() => startGame());
            mainMenuScript.sinalSair.AddListener(() => quit());
        } else {
            Debug.LogWarning("MainMenu script not found on the object.");
        }
    }

    void Start(){
        instantiateMenu();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
            instantiateMenu();
    }


    
}
