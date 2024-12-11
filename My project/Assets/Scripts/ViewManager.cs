using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public string nameGameScene = "cidades_juncao", nameLCrashScene = "Crash Animation", nameLTimeScene = "Time Animation", nameWScene = "YouWin";
    private GameObject mainMenuObject;
    private bool is_in_game = false;
    // todo implementar menu de pausa usando essa variave ^

    private Scene gameScene;
    private GameObject player;

    [SerializeField] GameObject mainMenuPrefab;

    private GameObject findObjectInSceneByName(Scene scene, string name){
        if (scene != null){
            foreach (GameObject obj in scene.GetRootGameObjects()){
                if (obj.name == name){
                    return obj;
                }
            }
        }
        return null;
    }

    IEnumerator playScene(string nameScene){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Additive);

        // aguarda a cena carregar
        while (!asyncLoad.isDone)
            yield return null;
        
        gameScene = SceneManager.GetSceneByName(nameScene);
        player = findObjectInSceneByName(gameScene, "Player");
        if (player == null){    
            Debug.Log("No player!!!");
        }
        else
            Debug.Log("Player!!!");
        CollisionDetect playerScriptCollisions = player.GetComponent<CollisionDetect>();
        PlayerMovement playerScriptMovement = player.GetComponent<PlayerMovement>();
        CarPositionReset playerScriptRotation = player.GetComponent<CarPositionReset>();

        playerScriptCollisions.noPoints.AddListener(() => crash());
        playerScriptMovement.noTime.AddListener(() => noTime());
        playerScriptMovement.ganhou.AddListener(() => ganhou());
        playerScriptRotation.capotouOCorsa.AddListener(() => crash());

        
    }
    
    private void unloadScene(string sceneName){
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid() && scene.isLoaded){
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
    private void quit(){
        Application.Quit();
    }

    IEnumerator loadEndScene(string sceneName){
        unloadScene(nameGameScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        // aguarda a cena carregar
        while (!asyncLoad.isDone)
            yield return null;

        Scene animacao = SceneManager.GetSceneByName(sceneName);
        GameObject winAnimationObject = findObjectInSceneByName(animacao, sceneName);
        ReturnToMenu AnimationScript = AnimationObject.GetComponentInChildren<ReturnToMenu>();
        AnimationScript.sinalSair.AddListener(() => {
            unloadScene(sceneName);
            instantiateMenu();
        });
    }


    private void crash(){
        is_in_game = false;
        StartCoroutine(loadEndScene(nameLCrashScene));
    }
    private void noTime(){
        is_in_game = false;
        StartCoroutine(loadEndScene(nameLTimeScene));
    }
    private void ganhou(){
        is_in_game = false;
        StartCoroutine(loadEndScene(nameWScene));
    }

    private void startGame(){
        if (mainMenuObject != null){
            Destroy(mainMenuObject);
        }
        if(is_in_game)
            return;

        StartCoroutine(playScene(nameGameScene));

        is_in_game = true;
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
