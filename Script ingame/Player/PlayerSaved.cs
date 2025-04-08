using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerSaved : ScriptableObject
{
    public int health;
    public string items = "";

    public int roomClear;

    private bool isFirstGame;

    private void OnEnable()
    {
        if (isFirstGame || !Application.isPlaying)
        {
            ResetData(); // àÃÕÂ¡ãªé¿Ñ§¡ìªÑ¹ÃÕà«çµ
            isFirstGame = false;
        } 
    }

    public void ResetData()
    {
        health = 100;
        roomClear = 0;
    }
}
