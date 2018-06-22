using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManager_DoublePath : EnemyManager
{

    public Paths[] allTargets = new Paths[2];
    public GameObject[] spawnPosition = new GameObject[2];
    // Use this for initialization


    public override void StartLevel(int nowLevel)
    {
        now_Level = nowLevel;
        enemys.Clear();
        for (int i = 0; i <enemyPrefabs.Length; i++)
        {//各個關卡的敵人
           for(int j =0;j< levelSetting[nowLevel-1].enemy[i].amount / 2/*2個路徑及出生點 所以 / 2 */;j++)
            {//敵人的數量
                enemys.Add(enemyPrefabs[levelSetting[nowLevel-1].enemy[i].index]);
            }
        }


        enemyCount = 0;
        nowSpawn = true;
        StartCoroutine(SpawnEnemy());
        nextLevelButton.GetComponent<Button>().enabled = false;
    }

    protected override IEnumerator SpawnEnemy()
    {
        if (!nowSpawn)
        {//停止SpawnEnemy，並脫離IEnumerator
            StopCoroutine(SpawnEnemy());
            yield break;
        }
        float newSpawnTimer = spawnTimer;
        if (enemys[enemyCount].GetComponent<Enemy>().health > 10000)
            newSpawnTimer *= 3;//延長強敵的產生間隔

        for (float i = 0; i <= newSpawnTimer; i += Time.deltaTime)
        {
            yield return 0;
        }
        enemys[enemyCount].SetActive(false);
        GameObject newEnemy1 = Instantiate(enemys[enemyCount], spawnPosition[0].transform.position, spawnPosition[0].transform.rotation);
        GameObject newEnemy2 = Instantiate(enemys[enemyCount], spawnPosition[1].transform.position, spawnPosition[1].transform.rotation);
        newEnemy1.GetComponent<Enemy>().SetPath(allTargets[0].targets);
        newEnemy2.GetComponent<Enemy>().SetPath(allTargets[1].targets);
        newEnemy1.SetActive(true);
        newEnemy2.SetActive(true);
        if (enemyCount >= enemys.Count - 1)
        {
            nowSpawn = false;
        }
        else
        {
            enemyCount++;
            StartCoroutine(SpawnEnemy());
        }
           

    }

    [System.Serializable]
    public struct Paths
    {
        public GameObject[] targets;
    }
}
