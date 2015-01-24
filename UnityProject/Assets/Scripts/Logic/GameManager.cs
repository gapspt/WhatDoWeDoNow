using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public Transform[] TopSpawnPoints;
    public Transform[] BottomSpawnPoints;

    public GameObject[] TopNPCs;
    public GameObject[] BottomNPCs;

    public int TopNPCsKilled = 0;
    public int BottomNPCsKilled = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (Application.loadedLevelName == "GameScene")
            {
                StartNewGame();
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        InitThreadQueue();
    }

    void OnDisable()
    {
        DestroyThreadQueue();
    }

    public void KillTopNPC(GameObject npc)
    {
        TopNPCsKilled++;

        // TODO some more stuff

        StartCoroutine(DelayedSpawnNPC(TopNPCs, TopSpawnPoints));

        if (TopNPCsKilled % 10 == 0)
        {
            StartCoroutine(DelayedSpawnNPC(TopNPCs, TopSpawnPoints));
        }
    }
    public void KillBottomNPC(GameObject npc)
    {
        BottomNPCsKilled++;

        // TODO some more stuff

        StartCoroutine(DelayedSpawnNPC(BottomNPCs, BottomSpawnPoints));

        if (BottomNPCsKilled % 10 == 0)
        {
            StartCoroutine(DelayedSpawnNPC(BottomNPCs, BottomSpawnPoints));
        }
    }

    private IEnumerator DelayedSpawnNPC(GameObject[] npcs, Transform[] spawnPoints)
    {
        yield return new WaitForSeconds(2);
        SpawnNPC(npcs, spawnPoints);
    }
    private void SpawnNPC(GameObject[] npcs, Transform[] spawnPoints)
    {
        Instantiate(npcs[UnityEngine.Random.Range(0, npcs.Length)],
            spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position,
            Quaternion.identity);
    }

    public void StartNewGame()
    {
        Application.LoadLevel("GameScene");
        StartCoroutine(DelayedSpawnNPC(TopNPCs, TopSpawnPoints));
        StartCoroutine(DelayedSpawnNPC(BottomNPCs, BottomSpawnPoints));
    }


    #region Action Queue

    private const int HandleQueueThreads = 4;
    private static Queue<Action> queue = null;
    private static object queueLock = new object();

    private static void InitThreadQueue()
    {
        lock (queueLock)
        {
            if (queue == null)
            {
                queue = new Queue<Action>();
                for (int i = 0; i < HandleQueueThreads; i++)
                {
                    new Thread(new ThreadStart(QueueConsumer)).Start();
                }
            }
        }
    }

    private static void DestroyThreadQueue()
    {
        lock (queueLock)
        {
            queue = null;
        }
    }

    private static void QueueConsumer()
    {
        while (queue != null)
        {
            Action a = null;
            lock (queueLock)
            {
                if (queue != null && queue.Count > 0)
                {
                    a = queue.Dequeue();
                }
            }
            if (a != null)
            {
                a();
            }
            Thread.Sleep(1);
        }
    }

    public static void Enqueue(Action a)
    {
        lock (queueLock)
        {
            if (queue != null)
            {
                queue.Enqueue(a);
            }
        }
    }

    #endregion
}
