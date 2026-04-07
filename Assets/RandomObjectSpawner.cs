using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RandomObjectSpawner : MonoBehaviour
{
    public List<GameObject> objects;     // Your 6 scene objects
    public List<Transform> podiums;      // Your 6 podium transforms
    public float heightOffset = 0.5f;    // How high above the podium objects sit
    public List<Vector3> lastPositions = new List<Vector3>();

        
    void RandomizeObjects()
{
    if (objects.Count != podiums.Count)
    {
        Debug.LogError("Objects count must match podiums count.");
        return;
    }

    List<Transform> shuffledPodiums = new List<Transform>(podiums);

    for (int i = 0; i < shuffledPodiums.Count; i++)
    {
        int randomIndex = Random.Range(i, shuffledPodiums.Count);
        Transform temp = shuffledPodiums[i];
        shuffledPodiums[i] = shuffledPodiums[randomIndex];
        shuffledPodiums[randomIndex] = temp;
    }

    // ✅ UNIQUE filename
    string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
    string path = Application.dataPath + "/object_positions_" + timestamp + ".txt";

    using (StreamWriter writer = new StreamWriter(path))
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Vector3 spawnPosition = shuffledPodiums[i].position + Vector3.up * heightOffset;

            objects[i].transform.position = spawnPosition;
            objects[i].transform.rotation = shuffledPodiums[i].rotation;

            writer.WriteLine(objects[i].name + " -> " + spawnPosition);
        }
    }

    Debug.Log("Saved to: " + path);
}

    void Start()
    {
        RandomizeObjects();
    }
}