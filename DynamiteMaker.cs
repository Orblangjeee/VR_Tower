using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 일정 시간 주기로 Dynamite 생성
//ObjectPool, Dynamite 공장, 일정(생성)시간, 경과시간, 생성위치
//[문제] 아직 다이너마이트 가져가지 않았는데 생성되는 경우 자기들끼리 터짐
//[해결] 아직 다이너마이트를 가져가지 않았다면 새로 생성하지 않는다.
//충돌감지Layer 지정(Item Layer), 충돌 범위? (checkRange), 충돌감지 어디서 해?(createPos), 지금 만들어 안만들어?
public class DynamiteMaker : MonoBehaviour
{
    public GameObject[] dynamitePool;
    public int poolSize = 5;
    public GameObject dynamiteFactory;
    public float createTime = 3f; //일정(생성)시간
    private float currentTime; //경과시간
    public Transform createPos; //생성 위치
    private int createIndex =0;

    public LayerMask checkMask; //충돌감지 Layer
    public float chechRange = 0.1f; //충돌 범위
    public bool isCreate = false; // 지금 만들지 안만들지 여부

    // Start is called before the first frame update
    void Start()
    {
        ObjectPoolInit();
    }

    // Update is called once per frame
    void Update()
    {
        //CreatePos 에 Dynamite 생성되었는지 체크
        isCreate = Physics.CheckSphere(createPos.position, chechRange, checkMask);


        //만들어진 물체가 없는 경우에만 Dynamite 생성
        if (isCreate == false)
        {
            //1. 시간이 경과한다
            currentTime += Time.deltaTime;
            //2. 일정시간 주기로(경과한 시간이 일정시간보다 커지면)
            if (currentTime > createTime)
            {
                //3. Dynamite 생성
                CreateDynamite();
                //4. 경과한 시간 Reset
                currentTime = 0f;
            }

        }

    }
    private void CreateDynamite()
    {
        //createPosition 으로 이동
        dynamitePool[createIndex].transform.position = createPos.position;
        //createIndex 에 해당하는 다이너마이트 활성하고
        //createIndex +1 해서 다음 방으로 넘어감
        dynamitePool[createIndex++].SetActive(true);
        

        // +1 한 CreateIndex 번호가 내 방번호로 넘어갔다면 다시 처음으로

        createIndex = createIndex >= poolSize ? 0 : createIndex + 1;
        /*if (createIndex >= poolSize)
        {
            createIndex = 0;
        } else { createIndex += 1; //다음방으로
        */
        //createIndex %= poolSize;
    }


    private void ObjectPoolInit()
    {
        //1. Pool Size 만큼의 방을 만든다.
        dynamitePool = new GameObject[poolSize];
        for (int i = 0; i< poolSize; i++)
        {
            //2. 만든 각 방 안에 Factory로 만든 Instance들을 넣엊네 
            dynamitePool[i] =Instantiate(dynamiteFactory);
            dynamitePool[i].SetActive(false);
        }
    }


}
