using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 특정 상황에서만 UI 선택할 수 있는 Ray를 활성화 / 비활성화
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
