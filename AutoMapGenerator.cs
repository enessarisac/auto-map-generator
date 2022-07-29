using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AutoMapGenerator : MonoBehaviour
{
    [SerializeField] bool isTherePlane;
    [SerializeField] GameObject plane;
    [SerializeField] List<GameObject> Environment = new List<GameObject>();
    [SerializeField] List<GameObject> Obstacles = new List<GameObject>();
    [SerializeField] List<GameObject> Collectables = new List<GameObject>();
    [SerializeField] Vector3 mapSpawnPosition;
    [SerializeField] float mapWidth, mapHeight;
    [SerializeField] List<Vector3> mapParts = new List<Vector3>();
    [SerializeField] int environmentPlacementRate;
    [SerializeField] int roadMinX, roadMaxX, RandomAccuracy;
    GameObject parentReference;

    [Button]
    public void CreateMap()
    {
        if (parentReference != null)
        {
            mapParts.Clear();
            Destroy(parentReference);
        }
        if (!isTherePlane)
        {
            var newPlane = Instantiate(plane, mapSpawnPosition, Quaternion.identity);
            mapWidth = newPlane.transform.lossyScale.x;
            mapHeight = newPlane.transform.lossyScale.z;
        }
        else
        {
            mapWidth = plane.transform.lossyScale.x;
            mapHeight = plane.transform.lossyScale.z;
        }
        var mapMinHeightDecimal = (mapHeight * -5);
        var mapMinWidthDecimal = (mapWidth * -5);
        var mapMaxHeightDecimal = (mapHeight * 5);
        var mapMaxWidthDecimal = (mapWidth * 5);
        Debug.Log("MapMinHeightDecimal: " + mapMinHeightDecimal + " MapMinWidthDecimal: " + mapMinWidthDecimal + " MapMaxHeightDecimal: " + mapMaxHeightDecimal + " MapMaxWidthDecimal: " + mapMaxWidthDecimal);
        for (int i = (int)mapMaxHeightDecimal; i > mapMinHeightDecimal; i--)
        {
            for (int j = (int)mapMaxWidthDecimal; j > mapMinWidthDecimal; j--)
            {
                mapParts.Add(new Vector3(j, 0, i));
            }
        }
        GenerateEnvironment();
    }
    [Button]
    public void DestroyEnvironment()
    {
        DestroyImmediate(parentReference);
        mapParts.Clear();
    }
    public void GenerateEnvironment()
    {
        GameObject parent = new GameObject();
        parent.name = "Environment";
        parent.transform.position = Vector3.zero;
        parentReference = parent;
        plane.transform.SetParent(parent.transform);
        GameObject lastInstantiatedObject = null;
        var randomInt = Random.Range(environmentPlacementRate - RandomAccuracy, environmentPlacementRate + RandomAccuracy);
        for (int i = 0; i < mapParts.Count; i += randomInt)
        {
            if (mapParts[i].x > roadMinX && mapParts[i].x < roadMaxX)
            {
                continue;
            }
            randomInt = Random.Range(environmentPlacementRate - RandomAccuracy, environmentPlacementRate + RandomAccuracy);
            var random = Random.Range(0, Environment.Count);
            var newObject = Instantiate(Environment[random], mapParts[i], Quaternion.identity);
            newObject.transform.SetParent(parent.transform);
            lastInstantiatedObject = newObject;
        }
    }
}
