using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EnemyWaves : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<Transform> enemySpawns; // List of spawn points
    
    public List<GameObject> enemies = new List<GameObject>();
    public bool waveOver = true;
    public int enemiesPerWave = 5;
    public Text waveCount;
    private int waveNumber = 0;

    void Update()
    {
        CheckWaveStatus();
    }

    void StartWave()
    {
        waveNumber++;
        waveOver = false;
        enemies.Clear();
        waveCount.text = "Wave: " + waveNumber.ToString(); 

        int spawnIndex = 0; // Track which spawn point to use

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform spawnPoint = enemySpawns[spawnIndex]; // Use spawn point in order
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemies.Add(newEnemy);

            spawnIndex = (spawnIndex + 1) % enemySpawns.Count; // Cycle through spawn points
        }
    }

    void CheckWaveStatus()
    {
        enemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);

        if (enemies.Count == 0 && !waveOver)
        {
            waveOver = true;
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(3f);
        StartWave();
    }
}
