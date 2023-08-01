using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//���� : ���� ���ϴ� Scene �̵�
//�ٸ� ���� �ε��ϴ� ������ LoadingBar(������), 100% ����Ǹ� �̵�!
// ��𼭵� �ٸ� Scene���� LoadingScene�� ȣ���� �� �ֵ���!
// �̵��� SceneName
// ��𼭵� ȣ���� �� �ִ� ���
// �ε����� (������) LoadingBar(Image)

//[����] : Loading �Ұ� ��� �ε�ȭ���� �ʹ� ���� ������
//[�ذ�] : �ּ� �ε��ð��� �ξ ��� �� �ʴ� �ε�ȭ���� �����ش�.
public class LoadingManager : MonoBehaviour
{
    private static string nextSceneName; //�̵��� SceneName
    
    public float minLoadingTime = 3f; //�ּ� �ε� �ð�

    public CanvasGroup canvasGroup; //�ε����� (Alpha �� ����)
    void Start()

    {
        
        // �ڷ�ƾ�� ���ؼ� ���� ������ �̵�
       StartCoroutine(LoadSceneProcess());
    }

    
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
        
    }

    //LoadingBar�� ���� �ε��ϴ� ������ �����ָ鼭 �� ä������ ���������� �̵�
    private IEnumerator LoadSceneProcess()
    {

        //Scene�� ���� ������ �ҷ��´�
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName); 
        //���� Scene���� �̵����� �ʵ��� ���
        op.allowSceneActivation = false;
        //�ּ� �ε� �ð��� üũ�� ��� �ð�
        float timer = 0f;

        //Scene�� ������ ��� Loading �� ������ �ݺ��ؼ� ���ݾ� ������ Load...
        while (op.isDone == false)
        {  
            //Coroutine�� �Լ� ������ �ݺ���(while ����)�� ���̴� ��쿣 �ʼ������� ��� (�� ���� ���� ó���ؼ� ���ߴ� ��찡 ����)
            yield return null; //�� �� �� , �ð� ������ �Ҽ�����
            
            //4-a. �ε� 100%�� ���� ���� ���
            if (op.progress < 0.9f)
            {
                //4-a-1. �ε� ���� ������¸� LoadingBar�� ���� �����ش�.
                //canvasGroup.alpha = 1- op.progress; -> �ʹ� ���� ����
            }
            else //4-b. �ε��� 100%�� �� ���
            {
                //5. Timer�� �ξ �ּ� �ε��ð��� ������ 100%�� �ǵ���
                //5-1. �ð��� ����Ѵ�
                timer += Time.unscaledDeltaTime; //TimeScale�� ������ ���� ����(unscaled)
                canvasGroup.alpha = Mathf.Lerp(1.0f, 0f, timer / minLoadingTime);
                //5-2. LoadingBar�� �ð��� ����ϴ� �Ϳ� ���� 100%�� �ǵ��� ä���ش�.

                //loadingBar.fillAmount = 1.0f; //4-b-1. LoadingBar�� 100% ä���ش�.
                if (canvasGroup.alpha <= 0f) //4-b-2. LoadingBar�� 100% �� ä������
                {
                    op.allowSceneActivation = true; //4-b-3. ���� Scene���� �̵�
                    yield break;//4-b-4. �ڷ�ƾ ����
                }
            }
        }
        



        
        
       
    }




}
