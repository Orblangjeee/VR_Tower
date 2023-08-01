using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : EnemyCreator �� ������ �ִ� ��ȯ ��ġ�� �������� Enemy���� ����
// Object Pool : Enemy Factory, Enemy Pool ũ��, Enemy ���� �迭 (Original), Enemy List (��¥ źâ)
// ��ȯ ��ġ, ���� �ð�(����� �ð�), ��� �ð�

public class EnemyCreator : MonoBehaviour
{
    //�̱���
    public static EnemyCreator Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }

       
    }

    [Header("Object Pooling")]
    public GameObject enemyFactory; 
    public int enemyPoolSize;
    private GameObject[] enemyPool;
    private List<GameObject> enemyList;

    [Header("Spawner")]
    public Transform target; // Enemy�� ������
    private Transform[] spawnPositions; //��ȯ ��ġ
    private List<int> spawnIndexList; //�����ϰ� ���� ��ȯ ��ġ Index
    public float createTime = 3f; //�����ð�
    [SerializeField]
    private float currentTime = 0f; //����ð�

    void Start()
    {
        InitPool();
        InitSpawnPositions();

        // 1. �����ϰ� ���� ��ȯ��ġ�� ��ȣ���� ������ ������� �����.
        spawnIndexList = new List<int>();
        FillSpawnIndexList();
    }

    void Update()
    {
        //��� �ڵ� : Enemy ����Ʈ�� ���� ���� Enemy�� ���ٸ� Update ����
        if (enemyList.Count < 1) return;
        // 1. �ð��� ����Ѵ�.
        currentTime += Time.deltaTime;
        // 2. ����� �ð��� �����ð����� Ŀ����
        if (currentTime > createTime)
        {
            // 3. Enemy�� �����Ѵ�.
            SpawnEnemy();
            // 4. ����ð� Reset
            currentTime = 0f;
        }
    }
    //Enemy ���� ��ġ �ʱ�ȭ
    void InitSpawnPositions()
    {
        //Creator �� �ڽ����� �پ��ִ� Object ���� Transform �����´�. 
        // => GetComponentsInChildren ���� �������� �ڱ� �ڽ�(Creator)�� �����´�.
        // [�ذ�] �ڽĵ��� Trnasform���� ������ �����´�.

        //0. �ڽĵ��� ��Ƶ� ���� �����. (OutofRange ���� ��)
        spawnPositions = new Transform[transform.childCount];
        //1. �� �ڽĵ��� ���� : childCount �� �ڽĵ��� ���� ��ȯ
        for (int i = 0; i < transform.childCount; i++)
        {
            //2. �� �ڽĵ鿡�� ��� ������ ���ΰ�?
            spawnPositions[i] = transform.GetChild(i);
            
        }
    }

    //Enemy Pool �ʱ�ȭ (Original �迭 -> List�� ���
    void InitPool()
    {
        // 1. Pool(�迭) ũ�⸸ŭ ���� (�ʱ�ȭ)
        enemyPool = new GameObject[enemyPoolSize];
        // 1-1. PoolList ���� (�ʱ�ȭ)
        enemyList = new List<GameObject>();
        //�� �濡 ������ Enemy ���� ����ֽ��ϴ�. (�Ҵ�/�ʱ�ȭ)
        for (int i = 0; i < enemyPool.Length; i++)
        {
            //1. Enemy ����
            GameObject enemy = Instantiate(enemyFactory);
            //2. ������ Enemy�� �� �濡 �־��ش�
            enemyPool[i] = enemy;
            //3. Enemy�� ��Ȱ��ȭ ���·� �濡 �ִ´�.
            enemyPool[i].SetActive(false);
            //3-1. Enemy�� Target�� Tower�� �����ؼ� �־��ش�.
            enemyPool[i].GetComponent<Enemy>().target = target;
            //4. �� �濡 �־��� enemy���� List���� ���ʷ� �߰�
            enemyList.Add(enemyPool[i]);
        }
    }

    // Enemy�� Ư��(����)��ġ�� ��ȯ�Ѵ�..
    private void SpawnEnemy()
    {
      
        // 2. ��Ͽ��� �����ϰ� 1���� ��ȯ��ġ�� �̴´�.
        int randomIndex = Random.Range(0,spawnIndexList.Count);
        // 3. �� ��ġ�� Enemy ����
        CreateEnemy(randomIndex);
        // 4. ���� ��ȯ ��ġ ��Ͽ��� ����
        spawnIndexList.RemoveAt(randomIndex);
        // 5. ��Ͽ��� ��ȣ�� ��� �̾Ҵٸ� �ٽ� ����� ä���ش�.
        if (spawnIndexList.Count < 1)
        {
            FillSpawnIndexList();
        }
    }
    // �����ϰ� ���� ��ȯ ��ġ�� Index ��ȣ�� �ζǱ�迡 ä���ִ´�.
    private void FillSpawnIndexList()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnIndexList.Add(i);
        }
    }


    //ObjectPool���� ��Ȱ��� Enemy ���� (Ȱ��ȭ) 
    void CreateEnemy(int spawnPosIndex)
    {
        //EnemyPool ����Ʈ�� �ִ� ù��° Enemy Ȱ��ȭ
        enemyList[0].SetActive(true);
        //Enemy�� spawnPosIndex ��° ��ȯ ��ġ�� �Űܳ��´�.
        enemyList[0].transform.position = spawnPositions[spawnPosIndex].position;
        //Ȱ��ȭ�� Enemy�� EnemyPoolList���� ����
        enemyList.RemoveAt(0);
    }
    //���� Enemy���� �ٽ� EnemyPool ����Ʈ ��Ͽ� �߰��Ѵ�.
    public void DeactiveEnemy(GameObject enemy)
    {
        // 0. ���� Enemy�� ���¸� �ٽ� ������ �� �ְ� Reset
        enemy.GetComponent<Enemy>().ResetState();
        // 1. ���� Enemy ������Ʈ ��Ȱ��ȭ
        enemy.SetActive(false);
        // 2. ���� Enemy ������Ʈ EnemyList�� �ٽ� �߰�
        enemyList.Add(enemy);
    }
}
