using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] GameObject player;
    [SerializeField] private LevelClass levelToSpawn;
    [SerializeField] private AudioSource musicPlayer;

    private float sectionDistance = 50;
    private bool timerIsRunning;

    public float levelScore;
    public int coinsCollectedInLevel;

    public UnityEvent OnCollision;
    public UnityEvent OnDeath;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelToSpawn = gameManager.GameLevels[gameManager.selectedGameIndex];

        musicPlayer = GetComponentInChildren<AudioSource>();
        musicPlayer.clip = levelToSpawn.levelMusic;
        musicPlayer.Play();

        //Spawn
        Instantiate(levelToSpawn.startingSection, Vector3.zero, Quaternion.identity);
        Instantiate(GetRandomPrefabFromLevel(), new Vector3(0, 0, sectionDistance), Quaternion.identity);
        Instantiate(GetRandomPrefabFromLevel(), new Vector3(0, 0, sectionDistance * 2), Quaternion.identity);
        Instantiate(player);

        //set defaults
        levelScore = 0;
        timerIsRunning = true;
        coinsCollectedInLevel = 0;

        //Event subscriptions
        OnCollision.AddListener(StopTimer);
    }
    private void Update()
    {
        if (timerIsRunning)
        {
            levelScore = Time.time;
        }
    }

    public GameObject GetRandomPrefabFromLevel()
    {
        int randomIndex = Random.Range(0, levelToSpawn.sectionPrefabs.Count);
        return levelToSpawn.sectionPrefabs[randomIndex];
    }

    private void StopTimer()
    {
        timerIsRunning = false;
    }

    private void InvokeDeath()
    {
        OnDeath.Invoke();
    }

    public void InvokeCollision()
    {
        OnCollision.Invoke();
        OnDeath.Invoke();
    }

    public void AddCoinsCollectedInLevel(int addedCoins)
    {
        coinsCollectedInLevel += addedCoins;
    }


}