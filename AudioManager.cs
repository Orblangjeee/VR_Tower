using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

//���� : BGM ������ �� �ֵ��� �մϴ�.
//singletone, DDL, AudioSource
//ó�� ���� ��, �ε巴�� FadeIn �Ǹ鼭 BGM ����
//fadeDuration

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instace;
    public AudioSource audioSource;
    [Range (0.0f, 5.0f)]
    public float fadeDuration = 2f;

    private void OnEnable()
    {
        //���ο� Scene�� ���۵Ǹ� OnSceneLoaded �Լ� ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Awake()
    {
        //A. ���� Instance�� ����ִ� ���¶��? ( = ���� ó�� ������ Object)
        if (Instace == null) 
        { 
            Instace = this;
            //Scene�� �ٲ���� �ش� Object(=�ڽ�)�� �ı����� �ʴ´�.
            DontDestroyOnLoad(gameObject);
        } 
        //B. ���� Instance�� ������� ���� ���¶��? ( = ���Ŀ� ������ Object)
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

    //Fade ȿ���� ���� Audio ���
    private IEnumerator FadeIn()
    {
        //0. AudioPlayer ���
        audioSource.Play();
        //1. ����ð�
        float currentTime =0f;
        while (true)
        {
            //1. Coroutine �� Return ��ȯ �Լ� ����
            yield return null;
            //2. �ð��� ����Ѵ�   
            currentTime += Time.unscaledDeltaTime;
            //3. �ð��� ����Կ� ���� ����� volume�� ���� Ŀ����. (0 -> 1)
            float volume = currentTime / fadeDuration;
            audioSource.volume = volume;
            //4. volume�� max�� �Ǹ� �ڷ�ƾ ����
            if (audioSource.volume >= 1f)
            {
                yield break;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        //1. ����ð�
        float currentTime = 0f;
        while (true)
        {
            //1. Coroutine �� Return ��ȯ �Լ� ����
            yield return null;
            //2. �ð��� ����Ѵ�   
            currentTime += Time.unscaledDeltaTime;
            //3. �ð��� ����Կ� ���� ����� volume�� ���� Ŀ����. (0 -> 1)
            float volume = 1 - currentTime / fadeDuration;
            audioSource.volume = volume;
            //4. volume�� max�� �Ǹ� �ڷ�ƾ ����
            if (audioSource.volume <= 0f)
            {
                //5. AudioPlayer ����
                audioSource.Stop();
                yield break;
            }
        }
    }
    //���ο� Scene�� ���� ������ ����Ǵ� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("�������� Scene : " + scene.name);
        //1. Loading ���� ������ BGM ����
        if (scene.name.Equals("Loading"))
        {
            Stop();
        }
        //2. �� �� �������� BGM ����
        else
        {
            Play();
        }
    }
}
