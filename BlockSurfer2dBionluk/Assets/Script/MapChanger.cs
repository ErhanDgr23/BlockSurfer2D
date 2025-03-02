using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanger : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>(); // 3 obje buraya atanacak

    private Vector2[] positionRanges = new Vector2[]
    {
        new Vector2(-5, -3), // İlk obje için aralık (-5 ile -3 arası)
        new Vector2(-1,  1), // İkinci obje için aralık (-1 ile 1 arası)
        new Vector2( 3,  5)  // Üçüncü obje için aralık (3 ile 5 arası)
    };

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            objects.Add(transform.GetChild(i));
        }

        ShuffleArray(positionRanges); // Rastgele sıra oluştur
        PlaceObjectsRandomly();
    }

    private void PlaceObjectsRandomly()
    {
        float[] basePositions = { -4, 0, 4 }; // Ana konumlar
        for (int i = 0; i < objects.Count && i < basePositions.Length; i++)
        {
            float minRange = basePositions[i] - 1f; // -1 ekleyerek minimum aralık belirle
            float maxRange = basePositions[i] + 1f; // +1 ekleyerek maksimum aralık belirle

            float randomX = Random.Range(minRange, maxRange); // Belirlenen aralıkta rastgele değer seç
            Vector3 newPosition = new Vector3(randomX + objects[i].transform.parent.transform.position.x, objects[i].position.y, objects[i].position.z);
            objects[i].position = newPosition;
        }
    }

    private void ShuffleArray(Vector2[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
        }
    }
}
