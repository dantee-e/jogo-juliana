using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{

    public string nameGameScene = "cidades_juncao";

    public void playScene(string sceneName){
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void unloadScene(string sceneName){
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void jogar(){
        print("startando jogo");
        playScene(nameGameScene);
    }
    public void sair(){
        Application.Quit();
    }

}
