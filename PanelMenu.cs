using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//역할 : 특정 버튼 누르면 메뉴창 활성화
//게임 시간 정지, 버튼(재개, 메인메뉴 돌아가기)


public class PanelMenu : MonoBehaviour
{
    public GameObject panelMenu;
    [Space(20)] public UnityEvent onActivateEvent;//창이 활성화 될 때
    [Space()] public UnityEvent onDeactivateEventEffect;//창이 비활성화 될 때
    public InputActionProperty menuButton; //특정 버튼 (사용자 입력)
    
    void Update()
    {
        //1. 사용자의 입력이 있다면 (버튼을 눌렀다면)
        if(menuButton.action.WasPressedThisFrame()) 
        {
            //메뉴창의 활성화/비활성화 여부
            bool active = panelMenu.activeSelf;
            //버튼 누름에 따라 메뉴창 열기/닫기
            panelMenu.SetActive(!active);
            //버튼 누름에 따라 게임시간 정지/재개
            GameManager.Instance.TimeStop = !active;
            //메뉴창이 열고 닫힘에 따라 Event 실행 (열릴 때, 닫힐 때)
            if (!active)
            {
                onActivateEvent.Invoke();
                //BGM 정지/재개 (DontDestroy로 파괴되기 때문에 UnityEvent 말고 코드로 불러와야한다.)
                AudioManager.Instace.Stop();
                //Light 어둡게
                

            } else
            {
                onDeactivateEventEffect.Invoke();
                AudioManager.Instace.Play();
                //Light 밝게

            }
           
            /*
            //2. Pause 창 활성화
            panelMenu.SetActive(true);
            //3. 게임 시간 잠시 정지
            GameManager.Instance.TimeStop = true;
            //4. 위치 --> (ActivateOnMyForward.cs)
            //5. 잡았을 때 실행시키고 싶은 것이 있다면 EventAction 실행
            onActivateEvent.Invoke();

            //B. 메뉴창 닫기
            //2. Pause 창 비활성화
            panelMenu.SetActive(false);
            //3. 게임 시간 다시 재생
            GameManager.Instance.TimeStop = false;
            //4. 창을 닫을 때 실행시키고 싶은게 있다면 실행
            onDeactivateEventEffect.Invoke();
            */

        }
    }
    //게임 일시정지 풀기
    public void OnClickClose()
    {
        //1. 게임 시간을 원래대로 되돌린다.
        GameManager.Instance.TimeStop = false;
        //2. OnDeactivate 이벤트 실행
        onDeactivateEventEffect.Invoke(); //-> (Invoke) 동시에 한꺼번에 실행하는 함수
        //창 닫기
        gameObject.SetActive(false);
    }
    //메인 메뉴로 돌아가기
    public void OnClickMain()
    {
        //1. 게임 시간을 원래대로 되돌린다.
        GameManager.Instance.TimeStop = false;
        //창 닫기
        gameObject.SetActive(false);
        LoadingManager.LoadScene("Main");
    }
}
