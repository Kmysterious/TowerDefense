using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManager : MonoBehaviour
{
    public AudioClip[] Boom;
    public GameObject[] targers;
    public GameObject[] enemyPrefabs;
    protected List<GameObject> enemys;
    public GameObject boomAudio;
    protected List<GameObject> boomAudios;
    protected int boomCount = 0;
    public float spawnTimer = .5f;//出怪時間
    protected int enemyCount;
    protected int deadCount;
    public GameObject nextLevelButton;
    public bool nowSpawn { get; protected set; }
    public LevelInspectorSetting[] enemyAmount = new LevelInspectorSetting[50];
    protected LevelEnemys[] levelSetting;
    protected GameObject gameManager;
    protected int now_Level = 0;
    // Use this for initialization
    protected void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        enemys = new List<GameObject>();
        boomAudios = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            GameObject objAudio = Instantiate(boomAudio);
            objAudio.transform.SetParent(transform);
            objAudio.SetActive(false);
            boomAudios.Add(objAudio);
        }
        LevelSetting();
        enemyCount = 0;
        nowSpawn = false;
    }

    public void EnemyDeadCount()
    {//計算敵人死亡數，來決定能否開始下一關
        deadCount++;
        if (deadCount >= enemys.Count)
        {
            nextLevelButton.GetComponent<Button>().enabled = true;
            deadCount = 0;
            if (now_Level == gameManager.GetComponent<GameManager>().maxLevel)
                gameManager.GetComponent<GameManager>().GameEnd();
        }
    }

    public virtual void StartLevel(int nowLevel)
    {
        now_Level = nowLevel;
        enemys.Clear();
        for (int i = 0; i <enemyPrefabs.Length; i++)
        {//各個關卡的敵人
           for(int j =0;j< levelSetting[nowLevel-1].enemy[i].amount;j++)
            {//敵人的數量
                enemys.Add(enemyPrefabs[levelSetting[nowLevel-1].enemy[i].index]);
            }
        }

         //測試用-----------
        /*   enemys.Clear();
           enemys.Add(enemyPrefabs[0]);*/
         //----------------------
         enemyCount = 0;
        nowSpawn = true;
        StartCoroutine(SpawnEnemy());
        nextLevelButton.GetComponent<Button>().enabled = false;
    }

    public void PlayBoomSound(Vector3 where)
    {//播放爆炸音效及特效
        StartCoroutine(BoomBoomBoom(where));
        EnemyDeadCount();
    }
    int temp = 0;
    protected IEnumerator BoomBoomBoom(Vector3 where)
    {
        int a = Random.Range(0, Boom.Length);

        if (!boomAudios[boomCount].activeInHierarchy)
        {
            boomAudios[boomCount].SetActive(true);
            boomAudios[boomCount].GetComponent<PlayBoom>().SetBoomAudio(Boom[a], where);
            boomCount++;
            if (boomCount >= boomAudios.Count - 1)
                boomCount = 0;
            yield return 0;
        }

    }
    protected virtual IEnumerator SpawnEnemy()
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

        //避免路徑還沒設定完就呼叫Enemy的Movement，所以要先設為false
        enemys[enemyCount].SetActive(false);
        GameObject newEnemy =  Instantiate(enemys[enemyCount], transform.position, transform.rotation);
        newEnemy.GetComponent<Enemy>().SetPath(targers);
        newEnemy.SetActive(true);
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
    protected int[] enemy_增加倍率 = new int[14] {25,25,20,20,15,15,10,10,6,6,4,2,1,1 };
    protected void LevelSetting()
    {
        levelSetting = new LevelEnemys[gameManager.GetComponent<GameManager>().maxLevel];
        for (int i = 0; i < levelSetting.Length; i++)//設定敵人總共有幾種
            levelSetting[i].enemy = new EnemyAmount[enemyPrefabs.Length];
        int autoCountLevel = 0;
        for (int i = 0; i < 27; i++)
        {
            autoCountLevel++;
            for (int j = 0; j < levelSetting[i].enemy.Length; j++)
            {//將敵人編號及數量加進陣列中，方便之後轉入List
                levelSetting[i].enemy[j].index = j;
                levelSetting[i].enemy[j].amount = enemyAmount[i].levelAmount[j];
            }    
        }

        for (int i = 1; i < levelSetting.Length -autoCountLevel; i++)
        {
            for (int j = 0; j < levelSetting[i].enemy.Length; j++)
            {
                levelSetting[i + autoCountLevel].enemy[j].index = j;
                if (j == levelSetting[i + autoCountLevel].enemy[13].index)
                    levelSetting[i + autoCountLevel].enemy[j].amount = levelSetting[autoCountLevel].enemy[j].amount + i * enemy_增加倍率[j]/3;
                else
                    levelSetting[i + autoCountLevel].enemy[j].amount = levelSetting[autoCountLevel].enemy[j].amount + i * enemy_增加倍率[j];


            }
        }
        
        #region 檢查各個關卡的敵人數量
       /*   string ohya = "";
          int total = 0;
          for (int i = 0; i < levelSetting.Length; i++)
          {
              ohya += "Level" + (i +1) + "：  ";
              for (int j = 0; j < levelSetting[i].enemy.Length; j++)
              {
                  total += levelSetting[i].enemy[j].amount;
                  ohya += j+":" + levelSetting[i].enemy[j].amount + "  ";
              }
              Debug.Log(ohya + "   total:" + total);
              ohya = "";
              total = 0;
          }*/
        #endregion
        
    }

    [System.Serializable]
    public struct LevelInspectorSetting
    {
        public int[] levelAmount;
    }
    [System.Serializable]
    protected struct LevelEnemys
    {
        public EnemyAmount[] enemy;

    }
    [System.Serializable]
    protected struct EnemyAmount
    {
      public int index;
      public int amount;
    }
    
}
