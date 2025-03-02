using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabsDown; // Rastgele seçilecek objelerin listesi (Aşağı)
    public GameObject[] objectPrefabsUp; // Rastgele seçilecek objelerin listesi (Yukarı)
    public int maxObjects = 3; // Maksimum aktif obje sayısı
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float lastX = 4f; // İlk objenin x değeri 4 olacak
    private bool spawnFromUp = false; // Başlangıçta aşağıdan spawn et

    void Start()
    {
        for (int i = 0; i < maxObjects; i++)
        {
            SpawnObject();
        }
    }

    public void TriggerSpawn()
    {
        SpawnObject();
        if (spawnedObjects.Count > maxObjects)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.RemoveAt(0);
        }
    }

    private void SpawnObject()
    {
        GameObject[] selectedArray = spawnFromUp ? objectPrefabsUp : objectPrefabsDown;
        if (selectedArray.Length == 0) return;

        // Rastgele obje seç
        GameObject prefabToSpawn = selectedArray[Random.Range(0, selectedArray.Length)];

        // X konumunu belirle (önceki objenin x konumu + rastgele 2 veya 6)
        float newX = lastX + (Random.Range(12f, 16f));
        lastX = newX;

        // Obje oluştur
        GameObject newObject = Instantiate(prefabToSpawn, new Vector3(newX, 0, 0), Quaternion.identity);
        spawnedObjects.Add(newObject);

        // UpStartMap veya UpEndMap kontrolü
        if (newObject.name.Contains("UpStartMap"))
        {
            spawnFromUp = true;
        }
        else if (newObject.name.Contains("UpEndMap"))
        {
            spawnFromUp = false;
        }
    }
}
