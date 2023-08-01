using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//역할 : 내가 원하는 Scene 이동
//다른 씬을 로드하는 과정을 LoadingBar(진행율), 100% 진행되면 이동!
// 어디서든 다른 Scene에서 LoadingScene을 호출할 수 있도록!
// 이동할 SceneName
// 어디서든 호출할 수 있는 방법
// 로딩과정 (진행율) LoadingBar(Image)

//[문제] : Loading 할게 없어서 로딩화면이 너무 빨리 지나감
//[해결] : 최소 로딩시간을 두어서 적어도 몇 초는 로딩화면을 보여준다.
public class LoadingManager : MonoBehaviour
{
    private static string nextSceneName; //이동할 SceneName
    
    public float minLoadingTime = 3f; //최소 로딩 시간

    public CanvasGroup canvasGroup; //로딩과정 (Alpha 값 변경)
    void Start()

    {
        
        // 코루틴을 통해서 다음 씬으로 이동
       StartCoroutine(LoadSceneProcess());
    }

    
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
        
    }

    //LoadingBar를 통해 로딩하는 과정을 보여주면서 다 채워지면 다음씬으로 이동
    private IEnumerator LoadSceneProcess()
    {

        //Scene에 대한 정보를 불러온다
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName); 
        //다음 Scene으로 이동하지 않도록 대기
        op.allowSceneActivation = false;
        //최소 로딩 시간을 체크할 경과 시간
        float timer = 0f;

        //Scene의 정보가 모두 Loading 될 때까지 반복해서 조금씩 정보를 Load...
        while (op.isDone == false)
        {  
            //Coroutine용 함수 내에서 반복문(while 같은)이 쓰이는 경우엔 필수적으로 사용 (한 번에 연산 처리해서 멈추는 경우가 있음)
            yield return null; //빈 거 줌 , 시간 지연도 할수있음
            
            //4-a. 로딩 100%가 되지 않은 경우
            if (op.progress < 0.9f)
            {
                //4-a-1. 로딩 현재 진행상태를 LoadingBar를 통해 보여준다.
                //canvasGroup.alpha = 1- op.progress; -> 너무 빨라서 생략
            }
            else //4-b. 로딩이 100%가 된 경우
            {
                //5. Timer를 두어서 최소 로딩시간이 지나면 100%가 되도록
                //5-1. 시간이 경과한다
                timer += Time.unscaledDeltaTime; //TimeScale의 영향을 받지 않음(unscaled)
                canvasGroup.alpha = Mathf.Lerp(1.0f, 0f, timer / minLoadingTime);
                //5-2. LoadingBar를 시간이 경과하는 것에 따라 100%가 되도록 채워준다.

                //loadingBar.fillAmount = 1.0f; //4-b-1. LoadingBar를 100% 채워준다.
                if (canvasGroup.alpha <= 0f) //4-b-2. LoadingBar가 100% 다 채워지면
                {
                    op.allowSceneActivation = true; //4-b-3. 다음 Scene으로 이동
                    yield break;//4-b-4. 코루틴 종료
                }
            }
        }
        



        
        
       
    }




}
