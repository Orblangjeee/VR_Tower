using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//���� : Intro Ui ��ư ����
// Game Start, Game Exit
//���� : Loading Manager�� ���ؼ� Scene �̵�
public class IntroUISystem : MonoBehaviour
{
    public Image fadeImg;
    public float fadeDuration = 2.0f; //Fade Duration ���̵� �ð�


    public void OnClickStart()
    {
        print("���� ����");
        StartCoroutine(LoadAfterFade());
        //���� ���۵Ǹ� -> ���̵�ƿ�
    }

    //���� ���۵Ǹ� -> ���̵�ƿ�
    public void OnClickExit()
    {
        print("���� ����");
        Application.Quit();
    }
    
    IEnumerator LoadAfterFade()
    {
        //1. ���̵�ƿ� ȿ���� ������
        //yield return new WaitForSecondsRealtime(fadeDuration);
        yield return StartCoroutine(FadeOut());
        //2. LoadingManager�� ���ؼ� MainScene���� �̵�
        LoadingManager.LoadScene("Main");
    }


    IEnumerator FadeOut()
    {
        //1. ��� �ð�
        float currentTime = 0f;
        while (true)
        {
            //1. Coroutine ��ȯ �Լ� ����ؼ� Interval �ش�.
            yield return null;
            //2. �ð��� ���
            currentTime += Time.unscaledDeltaTime;
            //3. ����ϴ� �ð��� �帧�� ���� image�� ���� ����������
            float alpha = currentTime / fadeDuration;
            SetImageAlpha(alpha);
            //4. ������ ������������ �ڷ�ƾ ����
            if (alpha >= 1.0f)
            {
                yield break; //�ڷ�ƾ ����
            }
        }
    }

    //Image ������Ʈ�� Alpha(����) ����
    private void SetImageAlpha(float alpha)
    {
        //1. ���� image ������Ʈ�� color ���� �޾ƿ´�.
        Color c = fadeImg.color;
        //2. �����Ǵ� alpha���� �޾ƿ� color�� alpha ���� ����
        c.a = alpha;
        //3. alpha ���� ����� color ���� image ������Ʈ�� ������
        fadeImg.color = c;
    }
}
