using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//���� : Ư�� ��ư ������ �޴�â Ȱ��ȭ
//���� �ð� ����, ��ư(�簳, ���θ޴� ���ư���)


public class PanelMenu : MonoBehaviour
{
    public GameObject panelMenu;
    [Space(20)] public UnityEvent onActivateEvent;//â�� Ȱ��ȭ �� ��
    [Space()] public UnityEvent onDeactivateEventEffect;//â�� ��Ȱ��ȭ �� ��
    public InputActionProperty menuButton; //Ư�� ��ư (����� �Է�)
    
    void Update()
    {
        //1. ������� �Է��� �ִٸ� (��ư�� �����ٸ�)
        if(menuButton.action.WasPressedThisFrame()) 
        {
            //�޴�â�� Ȱ��ȭ/��Ȱ��ȭ ����
            bool active = panelMenu.activeSelf;
            //��ư ������ ���� �޴�â ����/�ݱ�
            panelMenu.SetActive(!active);
            //��ư ������ ���� ���ӽð� ����/�簳
            GameManager.Instance.TimeStop = !active;
            //�޴�â�� ���� ������ ���� Event ���� (���� ��, ���� ��)
            if (!active)
            {
                onActivateEvent.Invoke();
                //BGM ����/�簳 (DontDestroy�� �ı��Ǳ� ������ UnityEvent ���� �ڵ�� �ҷ��;��Ѵ�.)
                AudioManager.Instace.Stop();
                //Light ��Ӱ�
                

            } else
            {
                onDeactivateEventEffect.Invoke();
                AudioManager.Instace.Play();
                //Light ���

            }
           
            /*
            //2. Pause â Ȱ��ȭ
            panelMenu.SetActive(true);
            //3. ���� �ð� ��� ����
            GameManager.Instance.TimeStop = true;
            //4. ��ġ --> (ActivateOnMyForward.cs)
            //5. ����� �� �����Ű�� ���� ���� �ִٸ� EventAction ����
            onActivateEvent.Invoke();

            //B. �޴�â �ݱ�
            //2. Pause â ��Ȱ��ȭ
            panelMenu.SetActive(false);
            //3. ���� �ð� �ٽ� ���
            GameManager.Instance.TimeStop = false;
            //4. â�� ���� �� �����Ű�� ������ �ִٸ� ����
            onDeactivateEventEffect.Invoke();
            */

        }
    }
    //���� �Ͻ����� Ǯ��
    public void OnClickClose()
    {
        //1. ���� �ð��� ������� �ǵ�����.
        GameManager.Instance.TimeStop = false;
        //2. OnDeactivate �̺�Ʈ ����
        onDeactivateEventEffect.Invoke(); //-> (Invoke) ���ÿ� �Ѳ����� �����ϴ� �Լ�
        //â �ݱ�
        gameObject.SetActive(false);
    }
    //���� �޴��� ���ư���
    public void OnClickMain()
    {
        //1. ���� �ð��� ������� �ǵ�����.
        GameManager.Instance.TimeStop = false;
        //â �ݱ�
        gameObject.SetActive(false);
        LoadingManager.LoadScene("Main");
    }
}
