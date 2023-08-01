using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

//역할 : BGM 관리할 수 있도록 합니다.
//singletone, DDL, AudioSource
//처음 시작 시, 부드럽게 FadeIn 되면서 BGM 시작
//fadeDuration

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instace;
    public AudioSource audioSource;
    [Range (0.0f, 5.0f)]
    public float fadeDuration = 2f;

    private void OnEnable()
    {
        //새로운 Scene이 시작되면 OnSceneLoaded 함수 실행
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Awake()
    {
        //A. 만일 Instance가 비어있는 상태라면? ( = 가장 처음 생성된 Object)
        if (Instace == null) 
        { 
            Instace = this;
            //Scene이 바뀌더라도 해당 Object(=자신)은 파괴되지 않는다.
            DontDestroyOnLoad(gameObject);
        } 
        //B. 만일 Instance가 비어있지 않은 상태라면? ( = 이후에 생성된 Object)
        else
        {
            Destroy(gameObject);
        }
       
    }

    
    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine (FadeOut());
    }

    //Fade 효과를 통해 Audio 재생
    private IEnumerator FadeIn()
    {
        //0. AudioPlayer 재생
        audioSource.Play();
        //1. 경과시간
        float currentTime =0f;
        while (true)
        {
            //1. Coroutine 용 Return 반환 함수 실행
            yield return null;
            //2. 시간이 경과한다   
            currentTime += Time.unscaledDeltaTime;
            //3. 시간이 경과함에 따라서 오디오 volume이 점점 커진다. (0 -> 1)
            float volume = currentTime / fadeDuration;
            audioSource.volume = volume;
            //4. volume이 max가 되면 코루틴 종료
            if (audioSource.volume >= 1f)
            {
                yield break;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        //1. 경과시간
        float currentTime = 0f;
        while (true)
        {
            //1. Coroutine 용 Return 반환 함수 실행
            yield return null;
            //2. 시간이 경과한다   
            currentTime += Time.unscaledDeltaTime;
            //3. 시간이 경과함에 따라서 오디오 volume이 점점 커진다. (0 -> 1)
            float volume = 1 - currentTime / fadeDuration;
            audioSource.volume = volume;
            //4. volume이 max가 되면 코루틴 종료
            if (audioSource.volume <= 0f)
            {
                //5. AudioPlayer 정지
                audioSource.Stop();
                yield break;
            }
        }
    }
    //새로운 Scene이 열릴 때마다 실행되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("현재실행된 Scene : " + scene.name);
        //1. Loading 씬이 열리면 BGM 정지
        if (scene.name.Equals("Loading"))
        {
            Stop();
        }
        //2. 그 외 씬에서는 BGM 실행
        else
        {
            Play();
        }
    }
}
