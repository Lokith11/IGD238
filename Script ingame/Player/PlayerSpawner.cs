using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab ������
    public RoomManager[] rooms; // Array �ͧ RoomManager
    public CinemachineVirtualCamera cinemachineCamera; // Virtual Camera �ͧ Cinemachine

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

        // �������͡��ͧ
        int randomIndex = Random.Range(0, rooms.Length);
        RoomManager selectedRoom = rooms[randomIndex];

        // ���ҧ�����蹷��ش�Դ���ͧ
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
