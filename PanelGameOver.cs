using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//역할 : 게임 종료 시, Game 종료 효과 실행 (Light 어둡게)
// Light , 어두워지는 속도
//역할 : GameOver 버튼 관리
// Restart, Main, Quit

public class PanelGameOver : MonoBehaviour
{
    public Light light;
    public float fadeDuration = 1.5f; //어두워지는 속도
    private float originIntensity;//원래 Light의 밝기

    [Space(20)]
    public UnityEvent onEventEffect;
  
    private void Start()
    {
        originIntensity = light.intensity;
    }

    //코루틴을 통해서 세상의 빛이 점점 어두워진다
    public IEnumerator FadeLight(bool isFade)
    {
        //true면 어두워짐, false면 밝아짐
        //1. 경과 시간
        float currentTime = 0f;

        while (true)
        {
            yield return null;
            //yield return new WaitForSecondsRealtime(1);
            //2. 시간이 경과
            currentTime += Time.unscaledDeltaTime;
            if (isFade) //어두워짐
            {
                //3. 시간이 경과함에 따라 자연스럽게 light 밝기도 줄어든다
                light.intensity = (1 - currentTime / fadeDuration) * originIntensity;
                //4. Light의 밝기가 0이 되면 코루틴 종료
                if (light.intensity <= 0)
                {
                    yield break;
                }

            }
            else //밝아짐
            {
                //3. 시간이 경과함에 따라 자연스럽게 light 밝기도 줄어든다
                light.intensity = currentTime / fadeDuration * originIntensity;
                //4. Light의 밝기가 0이 되면 코루틴 종료
                if (light.intensity >= 1)
                {
                    yield break;
                }

            }

        }
    }
    public void Delight()
    {
        StartCoroutine(FadeLight(true));
    }

    public void Fade()
    {
        StartCoroutine(FadeLight(false));
    }

    //게임 종료 시 실행되는 GameOver Effect
    public void GameOverEffect()
    {
        /*
        gameObject.SetActive(true); // 1. GameOver 패널 열기
        StartCoroutine(FadeLight()); // 2. 세상의 빛을 어둡게...
        FindObjectOfType<ActiveUICaster>().ActiveUIRay(true); //3. Player의 Ray 활성화
        */

        //OnEventEffect에 등록해놓은 함수들을 실행한다.
        onEventEffect.Invoke();
    }

    //게임을 다시 로드해서 다시 진행하기
    public void OnClickRestart()
    {
        print("Restart!");
        //1. Unity의 시간을 다시 흐르게 한다.
        GameManager.Instance.TimeStop = false;
        //2. 각각의 버튼을 눌렀을 때 해당 Scene으로 이동
        LoadingManager.LoadScene("Main");
    }

    //메인 화면 or 인트로 첫 화면으로 이동
    public void OnClickMain()
    {
        print("Go Back Main");
        LoadingManager.LoadScene("Intro");
    }

    //게임 종료
    public void OnClickQuit()
    {
        print("Game Exit");
        //A. UnityEditor 에서 실행 -> Editor 정지
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //B. UnityEditor 이외에서 실행 -> 프로그램(App) 종료
    Application.Quit();
#endif

        }
}
