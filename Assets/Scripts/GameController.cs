using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameController : MonoSingleton<GameController>
{
    public CursorLockMode currentCursorLockMode;
    public PlayerStats playerStats;
    public Agents defaultPlayerType = Agents.Windweaver;
    private Dictionary<Agents, AsyncOperationHandle<GameObject>> playerPrefabHandles;

    private IEnumerator Start()
    {
        yield return Addressables.InitializeAsync();
        playerPrefabHandles = new Dictionary<Agents, AsyncOperationHandle<GameObject>>();
        // Load all player prefabs asynchronously
        foreach (Agents type in Enum.GetValues(typeof(Agents)))
        {
            LoadPlayerPrefabAsync(type);
        }

    }

    public override void OnDestroy()
    {
        // Release loaded player prefabs when the GameController is destroyed
        foreach (var handle in playerPrefabHandles.Values)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }

    private void LoadPlayerPrefabAsync(Agents type)
    {
        // Load player prefab asynchronously
        var handle = Addressables.LoadAssetAsync<GameObject>($"{type}");

        // Register a callback for when the prefab is loaded
        handle.Completed += (operation) => OnPlayerPrefabLoaded(type, operation);
    }

    private void OnPlayerPrefabLoaded(Agents type, AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            playerPrefabHandles[type] = handle;
        }
        else
        {
            Debug.LogError($"Failed to load {type} prefab");
        }
    }

    public void InstantiatePlayer(Agents type)
    {
        if (playerPrefabHandles.ContainsKey(type) && playerPrefabHandles[type].IsValid())
        {
            GameObject playerPrefab = Instantiate(playerPrefabHandles[type].Result);
        }
        else
        {
            Debug.LogError($"Player prefab for {type.ToString()} is not loaded");
        }
    }

    public void HideCursor()
    {
        currentCursorLockMode = CursorLockMode.Locked;
        Cursor.lockState = currentCursorLockMode;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        currentCursorLockMode = CursorLockMode.None;
        Cursor.lockState = currentCursorLockMode;
        Cursor.visible = true;
    }

    public void ToggleCursor()
    {
        if (currentCursorLockMode == CursorLockMode.None)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }
    }
}
