using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPresets; // Prefab �������ͧ��ҧ�
    public GameObject[] itemPrefabs; // Prefab �������ҧ�
    public bool haveItem;
    public int minItemsPerRoom = 3;
    public int maxItemsPerRoom = 7;

    
    public Transform playerSpawnPoint; // �ش�Դ���������ͧ
    private Bounds roomBounds; // �ͺࢵ�ͧ��ͧ
   

    void Start()
    {
        GenerateRoom();
        // if (haveItem)
        // {
        //     SpawnItems();
        // }
    }

    void GenerateRoom()
    {
        // �������͡�������ͧ
        int randomIndex = Random.Range(0, roomPresets.Length);
        GameObject selectedRoom = roomPresets[randomIndex];

        // ���ҧ��ͧ
        GameObject roomInstance = Instantiate(selectedRoom, transform.position, transform.rotation, transform);

        // �ӹǳ�ͺࢵ�ͧ��ͧ
        roomBounds = GetRoomBounds(roomInstance);
    }

    void SpawnItems()
    {
        // �����ӹǹ�����
        int itemCount = Random.Range(minItemsPerRoom, maxItemsPerRoom + 1);

        for (int i = 0; i < itemCount; i++)
        {
            // �������͡�����
            int itemIndex = Random.Range(0, itemPrefabs.Length);
            GameObject selectedItem = itemPrefabs[itemIndex];

            // �������˹���������ͧ
            Vector3 spawnPosition = GetRandomPointInRoomBounds();

            // ���ҧ�����
            Instantiate(selectedItem, spawnPosition, Quaternion.identity, transform);
        }
    }

    Vector3 GetRandomPointInRoomBounds()
    {
        // �������˹����㹢ͺࢵ�ͧ��ͧ
        Vector3 randomPoint = new Vector3(
            Random.Range(roomBounds.min.x, roomBounds.max.x),
            Random.Range(roomBounds.min.y, roomBounds.max.y),
            Random.Range(roomBounds.min.z, roomBounds.max.z)
        );

        return randomPoint;
    }

    Bounds GetRoomBounds(GameObject room)
    {
        // �ӹǳ�ͺࢵ�ͧ��ͧ�ҡ Renderer (��Ѻ����ç���ҧ��ͧ�ͧ�س)
        Renderer[] renderers = room.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }
}
