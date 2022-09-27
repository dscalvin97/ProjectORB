using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public List<ObjectPool> pools = new List<ObjectPool>();
    public GameControls _GameControls;

    private void Awake()
    {
        _GameControls = new GameControls();
        _GameControls.Gameplay.Enable();
    }

    void Start()
    {

        for (int i = 0; i < pools.Count; i++)
            pools[i].InitializePool(transform);
    }
}
