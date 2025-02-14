using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject enemy;
    [SerializeField] Transform enemySpawn;
    public GameObject[] enemies;
    public bool waveOver;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaveStart();

    }

    void WaveStart()
    {

        if (waveOver == true)
        {
            Instantiate(enemy, enemySpawn.position, enemySpawn.rotation);
        }
       
    }

    void WaveOver()
    {
       waveOver = true; 
    }
}
