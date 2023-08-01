using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� : Tower �� ����
// A. HP�� ������ Enemy�� ���ݴ����� �� Damage �Դ´�
// Tower�� ���� Hp, Tower�� �ִ� Hp
// B. Damage�� �Ծ��� ��, Player�� Damage�� �Ծ��ٰ� �˷��ش�
// ȭ���� ��½�Ÿ��� ���� Image UI, ��½�Ÿ��� �ð� (interval)
// C. HP �� ���ҵǾ 0�� �Ǹ� ���� ����

//[����1] alpha �� ���̴� ���� �����ϸ� �˾Ƽ� �����ϰ�
//[����2] VR���� ������ �̹����� �ȉ�
//[�ذ�]
public class Tower : MonoBehaviour
{
    public static Tower Instance;

    private int currentHp = 0; //Tower�� �ִ� Hp
    private int maxHp = 30; //Tower�� �ִ� Hp
    public Image imgHit; //ȭ���� �����Ÿ��� ���� Image UI
    public float hitTime = 1f; //��½�Ÿ��� �ð�(interval)
    [Range (0.2f, 0.8f)]
    public float originDamageAlpha = 0.6f; //������ �Ծ��� �� imgHit�� ���� ��


    //Get-Set Property (TowerHp)
    private void Awake()
    {
        //�̱���
        if (Instance == null) { Instance = this; }


    }
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        SetImageAlpha(0f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //int damage = 3;
            Tower.Instance.HP-- ;
        }
    }
    
    //public int GetHp()
    //{
    //    return currentHp;
    //}

    //public void SetHp(int value)
    //{
    //    //1. TowerHp�� -1 ��
    //    currentHp += value;
    //    //2. TowerHp�� -1 �� ������ player �þ߸� ������
    //    //3. TowerHp�� 0�̵Ǹ� ���� ����
    //}

 
    //Damage �޾��� �� ȭ���� �����Ÿ���
    IEnumerator Damage()
    {
        //imgHit�� alpha ���� ������ -> �����ϰ�, HitTime �ð��� ���� 
       
        SetImageAlpha(originDamageAlpha);

        //currentTime
        float currentTime = 0f;
        
        //1. imgHit�� alpha���� �������� ���¶��? (0���� ū ���)
        while (imgHit.color.a > 0 )
        {
            //coroutine�� ��ȯ��
            yield return null;
            //�ð��� ����Ѵ�
            currentTime += Time.unscaledDeltaTime;
            //����ϴ� �ð��� ���� ���� alpha ���� 0���� ����������
            float targetAlpha = (1 - (currentTime / hitTime)) * originDamageAlpha ;
            //�������� ������ image�� �ݿ�
            SetImageAlpha(targetAlpha);
            
            //2. hitTime�� �ӵ���
            //2-1. ���� imgHit�� ������ ������ �����Ѵ�
            //2-2. imgHit �纻�� alpha ���� �����Ѵ�
            //2-3. ������ �纻�� ������ imgHit �������ο� �ݿ�
             //3. imgHit�� alpha ���� 0���� ���� (���� ����)
        }
    }

    public int HP
    {
        get => currentHp;
        //get {return currentHp;}
        set
        {
            //���� ���� Hp�� �����Ѵ�
            currentHp = value;
            //���� Hp ����� ������ Player ������ �Ծ��ٰ� �˷��ش�.
            StopAllCoroutines ();
            StartCoroutine(Damage());
            //Hp�� 0�̸�
            if (currentHp < 1 && !GameManager.Instance.IsGaneEnd)
            {
                print("��������");
                GameManager.Instance.GameOver();
            }
        }
    }


    private void SetImageAlpha(float targetAlpha)
    {
        //1. ���� Image ������Ʈ�� ���� ���� ���� ������ ����
        Color color = imgHit.color;
        //2. ������ �� �߿��� alpha ���� ���ϴ� ��(targetAlpha) ������ ����
        color.a = targetAlpha;
        //3. alpha ���� ������ Color �������� �ٽ� ���� Image ������Ʈ�� �Ҵ�
        imgHit.color = color;
    }
}
