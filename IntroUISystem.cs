using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//역할 : Intro Ui 버튼 관리
// Game Start, Game Exit
//역할 : Loading Manager를 통해서 Scene 이동
public class IntroUISystem : MonoBehaviour
{
    public Image fadeImg;
    public float fadeDuration = 2.0f; //Fade Duration 페이드 시간


    public void OnClickStart()
    {
        print("게임 시작");
        StartCoroutine(LoadAfterFade());
        //게임 시작되면 -> 페이드아웃
    }

    //게임 시작되면 -> 페이드아웃
    public void OnClickExit()
    {
        print("게임 종료");
        Application.Quit();
    }
    
    IEnumerator LoadAfterFade()
    {
        //1. 페이드아웃 효과가 끝나면
        //yield return new WaitForSecondsRealtime(fadeDuration);
        yield return StartCoroutine(FadeOut());
        //2. LoadingManager를 통해서 MainScene으로 이동
        LoadingManager.LoadScene("Main");
    }


    IEnumerator FadeOut()
    {
        //1. 경과 시간
        float currentTime = 0f;
        while (true)
        {
            //1. Coroutine 반환 함수 사용해서 Interval 준다.
            yield return null;
            //2. 시간이 경과
            currentTime += Time.unscaledDeltaTime;
            //3. 경과하는 시간의 흐름에 따라 image가 점점 불투명해짐
            float alpha = currentTime / fadeDuration;
            SetImageAlpha(alpha);
            //4. 완전히 불투명해지면 코루틴 종료
            if (alpha >= 1.0f)
            {
                yield break; //코루틴 종료
            }
        }
    }

    //Image 컴포넌트의 Alpha(투명도) 변경
    private void SetImageAlpha(float alpha)
    {
        //1. 현재 image 컴포넌트의 color 값을 받아온다.
        Color c = fadeImg.color;
        //2. 변동되는 alpha값을 받아온 color의 alpha 값에 적용
        c.a = alpha;
        //3. alpha 값이 적용된 color 값을 image 컴포넌트에 재적용
        fadeImg.color = c;
    }
}
