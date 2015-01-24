using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public Transform[] TopSpawnPoints;
    public Transform[] BottomSpawnPoints;

    public GameObject[] TopNPCs;
    public GameObject[] BottomNPCs;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(TempSpawnStuff());
    }

    IEnumerator TempSpawnStuff()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1);
            SpawnTopNPC();
            yield return new WaitForSeconds(1);
            SpawnBottomNPC();
        }
    }


    public static void SpawnTopNPC()
    {
        SpawnNPC(Instance.TopNPCs, Instance.TopSpawnPoints);
    }
    public static void SpawnBottomNPC()
    {
        SpawnNPC(Instance.BottomNPCs, Instance.BottomSpawnPoints);
    }
    private static void SpawnNPC(GameObject[] npcs, Transform[] spawnPoints)
    {
        Instantiate(npcs[Random.Range(0, npcs.Length)],
            spawnPoints[Random.Range(0, spawnPoints.Length)].position,
            Quaternion.identity);
    }

    public static void StartGame()
    {

    }

    

}
