using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]

public class LevelClass : ScriptableObject
{
    public GameObject startingSection;
    public string levelTitle;
    public List<GameObject> sectionPrefabs;
    public AudioClip levelMusic;
    public float highScore;
    public Sprite levelThumbnail;
}
