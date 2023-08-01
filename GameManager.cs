using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : ��ü���� ���� ���࿡ ���� �κ��� �����Ѵ�.
//�̱��� , ���� ����Ǿ����� ����, ���� ����(Panel), Menu Panel

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        
    }

    public PanelGameOver pGameOver; //GameOver Panel(â)
    public GameObject pMenu; //Menu Panel(â)
    private bool hasGameEnded = false; //���� ����Ǿ����� ����
    

    public bool IsGaneEnd
    {
        get => hasGameEnded; //���ٽ�
        
    }
    //GameWorld�� �ð��� ���߰� �ٽ� ����Ѵ�.
    public bool TimeStop
    {
        set
        {
            //A. TimeStop == true�� �ð� ����
            if (value == true) { Time.timeScale = 0f; }
           //B. TimeStop == false�� �ð� �簳
            else { Time.timeScale = 1f; }
            
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    //Tower�� HP�� ��� ��Ƽ� ���� ����(�й�)
    public void GameOver()
    {
        // ���� ����
        hasGameEnded = true;
        // ���� ���� �г� Open
        pGameOver.GameOverEffect();
        //BGM ����
        AudioManager.Instace.Stop();
        // Unity World�� �ð��� �����.
        TimeStop = true;


    }

}
