using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : ���� �ð� �ֱ�� Dynamite ����
//ObjectPool, Dynamite ����, ����(����)�ð�, ����ð�, ������ġ
//[����] ���� ���̳ʸ���Ʈ �������� �ʾҴµ� �����Ǵ� ��� �ڱ�鳢�� ����
//[�ذ�] ���� ���̳ʸ���Ʈ�� �������� �ʾҴٸ� ���� �������� �ʴ´�.
//�浹����Layer ����(Item Layer), �浹 ����? (checkRange), �浹���� ��� ��?(createPos), ���� ����� �ȸ����?
public class DynamiteMaker : MonoBehaviour
{
    public GameObject[] dynamitePool;
    public int poolSize = 5;
    public GameObject dynamiteFactory;
    public float createTime = 3f; //����(����)�ð�
    private float currentTime; //����ð�
    public Transform createPos; //���� ��ġ
    private int createIndex =0;

    public LayerMask checkMask; //�浹���� Layer
    public float chechRange = 0.1f; //�浹 ����
    public bool isCreate = false; // ���� ������ �ȸ����� ����

    // Start is called before the first frame update
    void Start()
    {
        ObjectPoolInit();
    }

    // Update is called once per frame
    void Update()
    {
        //CreatePos �� Dynamite �����Ǿ����� üũ
        isCreate = Physics.CheckSphere(createPos.position, chechRange, checkMask);


        //������� ��ü�� ���� ��쿡�� Dynamite ����
        if (isCreate == false)
        {
            //1. �ð��� ����Ѵ�
            currentTime += Time.deltaTime;
            //2. �����ð� �ֱ��(����� �ð��� �����ð����� Ŀ����)
            if (currentTime > createTime)
            {
                //3. Dynamite ����
                CreateDynamite();
                //4. ����� �ð� Reset
                currentTime = 0f;
            }

        }

    }
    private void CreateDynamite()
    {
        //createPosition ���� �̵�
        dynamitePool[createIndex].transform.position = createPos.position;
        //createIndex �� �ش��ϴ� ���̳ʸ���Ʈ Ȱ���ϰ�
        //createIndex +1 �ؼ� ���� ������ �Ѿ
        dynamitePool[createIndex++].SetActive(true);
        

        // +1 �� CreateIndex ��ȣ�� �� ���ȣ�� �Ѿ�ٸ� �ٽ� ó������

        createIndex = createIndex >= poolSize ? 0 : createIndex + 1;
        /*if (createIndex >= poolSize)
        {
            createIndex = 0;
        } else { createIndex += 1; //����������
        */
        //createIndex %= poolSize;
    }


    private void ObjectPoolInit()
    {
        //1. Pool Size ��ŭ�� ���� �����.
        dynamitePool = new GameObject[poolSize];
        for (int i = 0; i< poolSize; i++)
        {
            //2. ���� �� �� �ȿ� Factory�� ���� Instance���� �־��� 
            dynamitePool[i] =Instantiate(dynamiteFactory);
            dynamitePool[i].SetActive(false);
        }
    }


}
