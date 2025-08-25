using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Hierarchy;
using static UnityEditor.PlayerSettings;

public class Enemy_Spawner : MonoBehaviour
{
    #region Customized Class

    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnInterval;
        public float spawnTimer;
        public int MaxEnemiesPerWave;
        public int spawnedEnemyCount;

    }
    #endregion

    #region Declaring Variables

    public List<Wave> waves;
    private int waveNumber;
    public Transform minPos;
    public Transform maxPos;

    #endregion

    private void Update()
    {
        #region Managing WaveNumber & other Variables like spawn Interval and SpawnTimer

        if (Player_Controller.instance.gameObject.activeSelf)
        {
            waves[waveNumber].spawnTimer += Time.deltaTime;
            if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
            {
                waves[waveNumber].spawnTimer = 0;
                Instantiate(waves[waveNumber].enemyPrefab, RandomSpawnPoint(), transform.rotation);
                waves[waveNumber].spawnedEnemyCount++;
            }
            if (waves[waveNumber].spawnedEnemyCount >= waves[waveNumber].MaxEnemiesPerWave)
            {
                waves[waveNumber].spawnedEnemyCount = 0;
                if (waves[waveNumber].spawnInterval > 1f)
                {
                    waves[waveNumber].spawnInterval -= 0.10f;
                }
                Debug.Log("Increasing Wave Number");
                waveNumber++;
            }
            if (waveNumber >= waves.Count)
            {
                waveNumber = 0;
            }
        }

        #endregion
    }
    #region Random Spawn Point Function

    private Vector3 RandomSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        if (Random.Range(0f, 1f) > 0.5f)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            if (Random.Range(0f, 1f) > 0.5)
            {

                spawnPoint.z = minPos.position.z;
            }
            else
            {
                spawnPoint.z = maxPos.position.z;
            }
        }
        else
        {
            spawnPoint.z = Random.Range(minPos.position.z, maxPos.position.z);
            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.x = minPos.position.x;
            }
            else
            {
                spawnPoint.x = maxPos.position.x;
            }
        }
        return spawnPoint;
    }


    #endregion

}
