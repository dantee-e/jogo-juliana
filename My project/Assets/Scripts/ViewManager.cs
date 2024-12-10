using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public string nameGameScene = "cidades_juncao", nameLCrashScene = "Crash Animation", nameLTimeScene = "Time Animation";
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

        playerScriptCollisions.noPoints.AddListener(() => noPoints());
        playerScriptMovement.noTime.AddListener(() => noTime());

        
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

    IEnumerator loadLTimeScene(){
        unloadScene(nameGameScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameLTimeScene, LoadSceneMode.Additive);
        
        // aguarda a cena carregar
        while (!asyncLoad.isDone)
            yield return null;

        Scene animacao = SceneManager.GetSceneByName(nameLTimeScene);
        GameObject timeAnimationObject = findObjectInSceneByName(animacao, "Time Animation");
        TimeAnimation timeAnimationScript = timeAnimationObject.GetComponentInChildren<TimeAnimation>();
        timeAnimationScript.sinalSair.AddListener(() => {
            unloadScene(nameLTimeScene);
            instantiateMenu();
        });
    }

    IEnumerator loadLCrashScene(){
        unloadScene(nameGameScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameLCrashScene, LoadSceneMode.Additive);
        
        // aguarda a cena carregar
        while (!asyncLoad.isDone)
            yield return null;

        Scene animacao = SceneManager.GetSceneByName(nameLCrashScene);
        GameObject CrashAnimationObject = findObjectInSceneByName(animacao, "Crash Animation");
        CrashAnimation CrashAnimationScript = CrashAnimationObject.GetComponentInChildren<CrashAnimation>();
        CrashAnimationScript.sinalSair.AddListener(() => {
            unloadScene(nameLCrashScene);
            instantiateMenu();
        });
    }

    private void noPoints(){
        print("Crashed");
        is_in_game = false;
        StartCoroutine(loadLCrashScene());
    }
    private void noTime(){
        print("No Time");
        is_in_game = false;
        StartCoroutine(loadLTimeScene());
    }

    private void startGame(){
        print("Iniciando jogo");

        if (mainMenuObject != null){
            print("destroying menu");
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
            print("adicionando listeners");
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
