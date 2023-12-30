using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //Levels
    public List<LevelClass> GameLevels;
    public int selectedGameIndex;

    //Keep track of
    public int gold;

    //Singleton stuff
    public static GameManager Intance { get; private set; }

    private void Awake()
    {
        if (Intance != null && Intance != this)
        {
            Destroy(this);
        }
        else
        {
            Intance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void StartGameplay(int index)
    {
        selectedGameIndex = index; //set the levelIndex so it can be accessed in gameplay scene 
        SceneManager.LoadScene("Gameplay"); //load the gameplay scene
    }

    public void QuitGameplay()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void UpdateHighScore(int levelIndex, float score)
    {
        if (score > GameLevels[levelIndex].highScore)
        {
            GameLevels[levelIndex].highScore = score;
        }
    }

    public void AddGold(int newGold)
    {
        gold += newGold;
    }
}
