using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//역할 : Tower 를 관리
// A. HP를 가지고 Enemy에 공격당했을 때 Damage 입는다
// Tower의 현재 Hp, Tower의 최대 Hp
// B. Damage를 입었을 때, Player에 Damage를 입었다고 알려준다
// 화면을 번쩍거리게 만들 Image UI, 번쩍거리는 시간 (interval)
// C. HP 가 감소되어서 0이 되면 게임 종료

//[문제1] alpha 값 줄이는 것을 시작하면 알아서 투명하게
//[문제2] VR에선 빨갛게 이미지가 안됌
//[해결]
public class Tower : MonoBehaviour
{
    public static Tower Instance;

    private int currentHp = 0; //Tower의 최대 Hp
    private int maxHp = 30; //Tower의 최대 Hp
    public Image imgHit; //화면을 번적거리게 만들 Image UI
    public float hitTime = 1f; //번쩍거리는 시간(interval)
    [Range (0.2f, 0.8f)]
    public float originDamageAlpha = 0.6f; //데미지 입었을 때 imgHit의 투명도 농도


    //Get-Set Property (TowerHp)
    private void Awake()
    {
        //싱글톤
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
    //    //1. TowerHp를 -1 씩
    //    currentHp += value;
    //    //2. TowerHp가 -1 될 때마다 player 시야를 빨갛게
    //    //3. TowerHp가 0이되면 게임 종료
    //}

 
    //Damage 받았을 때 화면을 깜빡거린다
    IEnumerator Damage()
    {
        //imgHit의 alpha 값을 불투명 -> 투명하게, HitTime 시간에 따라 
       
        SetImageAlpha(originDamageAlpha);

        //currentTime
        float currentTime = 0f;
        
        //1. imgHit의 alpha값이 불투명한 상태라면? (0보다 큰 경우)
        while (imgHit.color.a > 0 )
        {
            //coroutine용 반환기
            yield return null;
            //시간이 경과한다
            currentTime += Time.unscaledDeltaTime;
            //경과하는 시간에 따라 점점 alpha 값은 0으로 투명해진다
            float targetAlpha = (1 - (currentTime / hitTime)) * originDamageAlpha ;
            //투명해진 색상을 image에 반영
            SetImageAlpha(targetAlpha);
            
            //2. hitTime의 속도로
            //2-1. 현재 imgHit의 색상을 가져와 복사한다
            //2-2. imgHit 사본의 alpha 값을 변경한다
            //2-3. 변경한 사본의 색상을 imgHit 오리지널에 반영
             //3. imgHit의 alpha 값이 0으로 수렴 (점점 투명)
        }
    }

    public int HP
    {
        get => currentHp;
        //get {return currentHp;}
        set
        {
            //나의 현재 Hp를 변경한다
            currentHp = value;
            //현재 Hp 변경될 때마다 Player 데미지 입었다고 알려준다.
            StopAllCoroutines ();
            StartCoroutine(Damage());
            //Hp가 0이면
            if (currentHp < 1 && !GameManager.Instance.IsGaneEnd)
            {
                print("게임종료");
                GameManager.Instance.GameOver();
            }
        }
    }


    private void SetImageAlpha(float targetAlpha)
    {
        //1. 원래 Image 컴포넌트가 가진 색상 값을 가져와 복사
        Color color = imgHit.color;
        //2. 복사한 값 중에서 alpha 값만 원하는 값(targetAlpha) 값으로 갱신
        color.a = targetAlpha;
        //3. alpha 값만 변경한 Color 색상으로 다시 원래 Image 컴포넌트에 할당
        imgHit.color = color;
    }
}
