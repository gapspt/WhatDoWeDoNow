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

    void OnEnable()
    {
        InitThreadQueue();
    }

    void OnDisable()
    {
        DestroyThreadQueue();
    }

    IEnumerator TempSpawnStuff()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(DelayedSpawnNPC(Instance.TopNPCs, Instance.TopSpawnPoints));
            yield return new WaitForSeconds(1);
            StartCoroutine(DelayedSpawnNPC(Instance.BottomNPCs, Instance.BottomSpawnPoints));
        }
    }

    public void KillTopNPC(GameObject npc)
    {
        // TODO some more stuff

        StartCoroutine(DelayedSpawnNPC(Instance.TopNPCs, Instance.TopSpawnPoints));
    }
    public void KillBottomNPC(GameObject npc)
    {
        // TODO some more stuff

        StartCoroutine(DelayedSpawnNPC(Instance.BottomNPCs, Instance.BottomSpawnPoints));
    }

    private IEnumerator DelayedSpawnNPC(GameObject[] npcs, Transform[] spawnPoints)
    {
        yield return new WaitForSeconds(1);
        Instantiate(npcs[UnityEngine.Random.Range(0, npcs.Length)],
            spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position,
            Quaternion.identity);
    }

    public static void StartNewGame()
    {
        Application.LoadLevel("GameScene");
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
