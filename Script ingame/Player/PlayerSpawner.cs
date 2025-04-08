using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab ผู้เล่น
    public RoomManager[] rooms; // Array ของ RoomManager
    public CinemachineVirtualCamera cinemachineCamera; // Virtual Camera ของ Cinemachine

    private CinemachineConfiner confiner;

    void Start()
    {
        SpawnPlayer();
        
    }
    void SpawnPlayer()
    {
        if (playerPrefab == null || rooms.Length == 0 || cinemachineCamera == null)
        {
            Debug.LogError("Player prefab or rooms are not assigned!");
            return;
        }

        // สุ่มเลือกห้อง
        int randomIndex = Random.Range(0, rooms.Length);
        RoomManager selectedRoom = rooms[randomIndex];

        // สร้างผู้เล่นที่จุดเกิดในห้อง
        GameObject playerInstance = Instantiate(playerPrefab, selectedRoom.playerSpawnPoint.position, selectedRoom.playerSpawnPoint.rotation);
        if (cinemachineCamera.Follow == null)
        {
            cinemachineCamera.Follow = playerInstance.transform;
            cinemachineCamera.LookAt = playerInstance.transform;

            confiner = cinemachineCamera.GetComponent<CinemachineConfiner>();
            if (confiner == null)
            {
                confiner = cinemachineCamera.gameObject.AddComponent<CinemachineConfiner>();
            }

            confiner.m_BoundingVolume = selectedRoom.GetComponentInChildren<BoxCollider>();
            if (confiner.m_BoundingVolume == null)
            {

            }
        }
        else
        {
            cinemachineCamera.Follow = playerInstance.transform;
            cinemachineCamera.LookAt = playerInstance.transform;
        }
    }
}
