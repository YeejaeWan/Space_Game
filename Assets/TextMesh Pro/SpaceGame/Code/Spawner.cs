using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;



    int level;
    float timer;
    

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;


        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),spawnData.Length - 1);

        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
            

        }

       
    }

    void Spawn()
    {
        /*
         GameObject enemy = GameManager.instance.pool.Get(0);
         enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
         enemy.GetComponent<Enemy>().Init(spawnData[level]);
        */
        //마지막 적 같은 경우, 한개의 스폰 포인트에서만 나옴
        GameObject enemy = GameManager.instance.pool.Get(0);
        
        if (level == spawnData.Length - 1) // 마지막 인덱스의 적인 경우
        {
           
            enemy.transform.position = spawnPoint[1].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
            spawnData[level].spawnTime = 5000;
            

           
        }
        else if(level == spawnData.Length - 2)
        {
            enemy.transform.position = spawnPoint[Random.Range(1, 4)].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }

        else
        {
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);

        }

        

    }
}


[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriterType;
    public int health;
    public float speed;



}
