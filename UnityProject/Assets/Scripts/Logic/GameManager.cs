﻿using UnityEngine;
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

    public int SpawnExtraFrequency = 5;
    
    public static GameManager instance;
    public int score = 0;
   
	public static void heavenScores(){
		GameManager.instance.score -= 1;
	}
	
	public static void hellScores(){
		GameManager.instance.score += 1;
	}
	
	public static int getScore(){
		return instance.score;
		//return instance.BottomNPCsKilled - instance.TopNPCsKilled;
	}
	
	void Start()
	{
		instance = this;
	}
	
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
        if (Instance != this)
            return;

        InitThreadQueue();
    }

    void OnDisable()
    {
        if (Instance != this)
            return;

        DestroyThreadQueue();
    }

    public void KilledTopNPC(GameObject npc)
    {
        print("D T");
        TopNPCsKilled++;

        // TODO some more stuff

        StartCoroutine(DelayedSpawnNPC(TopNPCs, TopSpawnPoints));

        if (TopNPCsKilled % SpawnExtraFrequency == 0)
        {
            StartCoroutine(DelayedSpawnNPC(TopNPCs, TopSpawnPoints));
        }
    }
    public void KilledBottomNPC(GameObject npc)
    {
        print("D B");
        BottomNPCsKilled++;

        // TODO some more stuff

        StartCoroutine(DelayedSpawnNPC(BottomNPCs, BottomSpawnPoints));

        if (BottomNPCsKilled % SpawnExtraFrequency == 0)
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
