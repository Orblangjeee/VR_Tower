using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//���� : ���� ���� ��, Game ���� ȿ�� ���� (Light ��Ӱ�)
// Light , ��ο����� �ӵ�
//���� : GameOver ��ư ����
// Restart, Main, Quit

public class PanelGameOver : MonoBehaviour
{
    public Light light;
    public float fadeDuration = 1.5f; //��ο����� �ӵ�
    private float originIntensity;//���� Light�� ���

    [Space(20)]
    public UnityEvent onEventEffect;
  
    private void Start()
    {
        originIntensity = light.intensity;
    }

    //�ڷ�ƾ�� ���ؼ� ������ ���� ���� ��ο�����
    public IEnumerator FadeLight(bool isFade)
    {
        //true�� ��ο���, false�� �����
        //1. ��� �ð�
        float currentTime = 0f;

        while (true)
        {
            yield return null;
            //yield return new WaitForSecondsRealtime(1);
            //2. �ð��� ���
            currentTime += Time.unscaledDeltaTime;
            if (isFade) //��ο���
            {
                //3. �ð��� ����Կ� ���� �ڿ������� light ��⵵ �پ���
                light.intensity = (1 - currentTime / fadeDuration) * originIntensity;
                //4. Light�� ��Ⱑ 0�� �Ǹ� �ڷ�ƾ ����
                if (light.intensity <= 0)
                {
                    yield break;
                }

            }
            else //�����
            {
                //3. �ð��� ����Կ� ���� �ڿ������� light ��⵵ �پ���
                light.intensity = currentTime / fadeDuration * originIntensity;
                //4. Light�� ��Ⱑ 0�� �Ǹ� �ڷ�ƾ ����
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

    //���� ���� �� ����Ǵ� GameOver Effect
    public void GameOverEffect()
    {
        /*
        gameObject.SetActive(true); // 1. GameOver �г� ����
        StartCoroutine(FadeLight()); // 2. ������ ���� ��Ӱ�...
        FindObjectOfType<ActiveUICaster>().ActiveUIRay(true); //3. Player�� Ray Ȱ��ȭ
        */

        //OnEventEffect�� ����س��� �Լ����� �����Ѵ�.
        onEventEffect.Invoke();
    }

    //������ �ٽ� �ε��ؼ� �ٽ� �����ϱ�
    public void OnClickRestart()
    {
        print("Restart!");
        //1. Unity�� �ð��� �ٽ� �帣�� �Ѵ�.
        GameManager.Instance.TimeStop = false;
        //2. ������ ��ư�� ������ �� �ش� Scene���� �̵�
        LoadingManager.LoadScene("Main");
    }

    //���� ȭ�� or ��Ʈ�� ù ȭ������ �̵�
    public void OnClickMain()
    {
        print("Go Back Main");
        LoadingManager.LoadScene("Intro");
    }

    //���� ����
    public void OnClickQuit()
    {
        print("Game Exit");
        //A. UnityEditor ���� ���� -> Editor ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //B. UnityEditor �̿ܿ��� ���� -> ���α׷�(App) ����
    Application.Quit();
#endif

        }
}
