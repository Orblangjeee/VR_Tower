using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 전체적인 게임 진행에 대한 부분을 관리한다.
//싱글톤 , 게임 종료되었는지 여부, 게임 종료(Panel), Menu Panel

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        
    }

    public PanelGameOver pGameOver; //GameOver Panel(창)
    public GameObject pMenu; //Menu Panel(창)
    private bool hasGameEnded = false; //게임 종료되었는지 여부
    

    public bool IsGaneEnd
    {
        get => hasGameEnded; //람다식
        
    }
    //GameWorld의 시간을 멈추고 다시 재생한다.
    public bool TimeStop
    {
        set
        {
            //A. TimeStop == true면 시간 멈춤
            if (value == true) { Time.timeScale = 0f; }
           //B. TimeStop == false면 시간 재개
            else { Time.timeScale = 1f; }
            
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    //Tower의 HP가 모두 닳아서 게임 종료(패배)
    public void GameOver()
    {
        // 게임 종료
        hasGameEnded = true;
        // 게임 종료 패널 Open
        pGameOver.GameOverEffect();
        //BGM 정지
        AudioManager.Instace.Stop();
        // Unity World의 시간을 멈춘다.
        TimeStop = true;


    }

}
