using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : Ư�� ��Ȳ������ UI ������ �� �ִ� Ray�� Ȱ��ȭ / ��Ȱ��ȭ
public class ActiveUICaster : MonoBehaviour
{
    public GameObject leftUIRay;
    public GameObject rightUIRay;

    private void Start()
    {
        ActiveUIRay(false);
    }
    public void ActiveUIRay (bool active)
    {
        leftUIRay.SetActive (active);
        rightUIRay.SetActive (active);  
    }
}
