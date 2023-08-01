using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : EnemyCreator 가 가지고 있는 소환 위치에 랜덤으로 Enemy들을 생성
// Object Pool : Enemy Factory, Enemy Pool 크기, Enemy 담을 배열 (Original), Enemy List (진짜 탄창)
// 소환 위치, 일정 시간(만드는 시간), 경과 시간

public class EnemyCreator : MonoBehaviour
{
    //싱글톤
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
    public Transform target; // Enemy의 목적지
    private Transform[] spawnPositions; //소환 위치
    private List<int> spawnIndexList; //랜덤하게 뽑힐 소환 위치 Index
    public float createTime = 3f; //일정시간
    [SerializeField]
    private float currentTime = 0f; //경과시간

    void Start()
    {
        InitPool();
        InitSpawnPositions();

        // 1. 랜덤하게 뽑을 소환위치의 번호들을 가져와 목록으로 만든다.
        spawnIndexList = new List<int>();
        FillSpawnIndexList();
    }

    void Update()
    {
        //방어 코드 : Enemy 리스트에 현재 만들 Enemy가 없다면 Update 종료
        if (enemyList.Count < 1) return;
        // 1. 시간이 경과한다.
        currentTime += Time.deltaTime;
        // 2. 경과한 시간이 일정시간보다 커지면
        if (currentTime > createTime)
        {
            // 3. Enemy를 생성한다.
            SpawnEnemy();
            // 4. 경과시간 Reset
            currentTime = 0f;
        }
    }
    //Enemy 생성 위치 초기화
    void InitSpawnPositions()
    {
        //Creator 의 자식으로 붙어있는 Object 들의 Transform 가져온다. 
        // => GetComponentsInChildren 으로 가져오면 자기 자신(Creator)도 가져온다.
        // [해결] 자식들의 Trnasform에만 접근해 가져온다.

        //0. 자식들을 담아둘 방을 만든다. (OutofRange 오류 뜸)
        spawnPositions = new Transform[transform.childCount];
        //1. 내 자식들의 개수 : childCount 는 자식들의 개수 반환
        for (int i = 0; i < transform.childCount; i++)
        {
            //2. 내 자식들에게 어떻게 접근할 것인가?
            spawnPositions[i] = transform.GetChild(i);
            
        }
    }

    //Enemy Pool 초기화 (Original 배열 -> List에 담기
    void InitPool()
    {
        // 1. Pool(배열) 크기만큼 생성 (초기화)
        enemyPool = new GameObject[enemyPoolSize];
        // 1-1. PoolList 생성 (초기화)
        enemyList = new List<GameObject>();
        //각 방에 생성한 Enemy 들을 집어넣습니다. (할당/초기화)
        for (int i = 0; i < enemyPool.Length; i++)
        {
            //1. Enemy 생성
            GameObject enemy = Instantiate(enemyFactory);
            //2. 생성한 Enemy를 각 방에 넣어준다
            enemyPool[i] = enemy;
            //3. Enemy는 비활성화 상태로 방에 넣는다.
            enemyPool[i].SetActive(false);
            //3-1. Enemy의 Target을 Tower로 지정해서 넣어준다.
            enemyPool[i].GetComponent<Enemy>().target = target;
            //4. 각 방에 넣어준 enemy들을 List에도 차례로 추가
            enemyList.Add(enemyPool[i]);
        }
    }

    // Enemy를 특정(랜덤)위치에 소환한다..
    private void SpawnEnemy()
    {
      
        // 2. 목록에서 랜덤하게 1개의 소환위치를 뽑는다.
        int randomIndex = Random.Range(0,spawnIndexList.Count);
        // 3. 그 위치에 Enemy 생성
        CreateEnemy(randomIndex);
        // 4. 뽑은 소환 위치 목록에서 제거
        spawnIndexList.RemoveAt(randomIndex);
        // 5. 목록에서 번호를 모두 뽑았다면 다시 목록을 채워준다.
        if (spawnIndexList.Count < 1)
        {
            FillSpawnIndexList();
        }
    }
    // 랜덤하게 뽑을 소환 위치의 Index 번호를 로또기계에 채워넣는다.
    private void FillSpawnIndexList()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnIndexList.Add(i);
        }
    }


    //ObjectPool에서 재활용된 Enemy 생성 (활성화) 
    void CreateEnemy(int spawnPosIndex)
    {
        //EnemyPool 리스트에 있는 첫번째 Enemy 활성화
        enemyList[0].SetActive(true);
        //Enemy를 spawnPosIndex 번째 소환 위치에 옮겨놓는다.
        enemyList[0].transform.position = spawnPositions[spawnPosIndex].position;
        //활성화한 Enemy는 EnemyPoolList에서 제거
        enemyList.RemoveAt(0);
    }
    //죽은 Enemy들을 다시 EnemyPool 리스트 목록에 추가한다.
    public void DeactiveEnemy(GameObject enemy)
    {
        // 0. 죽은 Enemy의 상태를 다시 재사용할 수 있게 Reset
        enemy.GetComponent<Enemy>().ResetState();
        // 1. 죽은 Enemy 오브젝트 비활성화
        enemy.SetActive(false);
        // 2. 죽은 Enemy 오브젝트 EnemyList에 다시 추가
        enemyList.Add(enemy);
    }
}
