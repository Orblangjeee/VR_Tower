using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//역할 : 내 손에 닿은 물체를 잡거나 놓는다
// 손의 위치 : 왼쪽, 오른족
// A. 잡다 -1. 잡을 수 있는 범위 -2. 잡은 물체 -3. 충돌 체크를 위한 Rigidbody
// B. 놓다
//[문제] 잡은 물체의 물리적용으로 인해 위치가 어긋남
//[해결] 물체를 잡고 놓을 때마다 물리적용을 on/off
//역할 : TestPlayer 시, 잡은 물체를 던진다.
//물체를 던지는 힘

//기능 : VR Player의 Controller에 맞게 키 입력 재배치
// 왼쪽 오른쪽

public class PlayerHand : MonoBehaviour
{
    public enum Hand { Left, Right }; //손의 위치 (왼,오)
    public Hand m_hand = Hand.Left;

    //OVRInput.Controller controllerTouch;//왼쪽 오른쪽 다르게 VR 컨트롤키 입력

    public float catchRadius = 0.5f; //잡을 수 있는 범위
    public GameObject catchObj; //잡은 물체
    private Rigidbody rb; //충돌체크를 위한 Rigidbody
    public float throwForce = 5f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        switch (m_hand)
        {
           // case Hand.Left: controllerTouch = OVRInput.Controller.LTouch; break;
           // case Hand.Right: controllerTouch = OVRInput.Controller.RTouch; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //A. 내가 마우스를 클릭하면! 잡기
        if (Input.GetButtonDown("Fire1") && catchObj == null) 
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controllerTouch) && catchObj == null) 
        {
           
                CatchObj();
           
                
        }
        //B. 마우스 클릭했다가 떼면! 놓기
        if (Input.GetButtonUp("Fire1") && catchObj != null)
        //if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controllerTouch) && catchObj != null)
        {
            
                DropObj();
           
               
        }
    }

    //잡았을 때
    // Hand를 기준으로 CatchRadius만큼의 반지름을 가진 구의 영역 = 탐색 영역
    // 물체가 들어와있는 경우 가장 가까이 있는 물체를 잡는다
    void CatchObj()
    {
        Vector3 position = transform.position; //검색 기준 위치 (=내 손의 위치)
        float radius = catchRadius; //반지름의 볌위 CatchRadius
        int layerMask = 1 << LayerMask.NameToLayer("Item"); //특정 Layer만 잡을 수 있게
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);//해당 탐색범위 안에 들어온 물체들을 모두 검색한다

        //탐색 범위 안에 물체가 있는지 없는지
        int selectedIndex = -1;

        // 여러개 물체가 충돌한 경우, 가장 가까이 있는 물체를 타깃으로 한다. -> 잡을 물체의 방번호를 selectedIndex에 알려준다
        //1. 탐색 영역 안에 물체가 있는지 여부
        if (hits != null && hits.Length > 0)
        {
            //2. selectedIndex를 0번 부터 비교할 수 있도록 재할당
            selectedIndex = 0;
            //3. 부딪힌 녀석들 중 가장 짧은 거리의 녀석을 선택할 수 있도록 반복 검사
            for (int i = 1; i <hits.Length; i++)
            { //4. 비교를 통해 더 거리가 가까운 충돌체의 index를 구한다
                float currentDis = Vector3.Distance(position, hits[selectedIndex].transform.position);  // 현재 비교 기준이 되는 충돌체 - 손 거리
                float nextDis = Vector3.Distance(position, hits[i].transform.position); // 다음 비교할 충돌체 - 손 거리
             //5. 현재 기준 충돌체 거리와 다음 비교할 충돌체 거리를 비교
             if(currentDis > nextDis)
                {
                    //6. 더 짧은 거리에 있는 충돌체의 index를 selectedindex에 넣어준다 
                    selectedIndex = i;
                }
            }
        }
        //A. 물체가 부딪힌 경우 
        if (selectedIndex != -1)
        {

            //물체를 잡는다. -> 내 손의 위치로 이동시킨다.
             catchObj = hits[selectedIndex].gameObject;
            //물리적용 off
            EnablePhysics(false);
            
            //물체가 손의 움직임을 따라가도록 자식으로 놓는다.
            catchObj.transform.parent = transform;
            
            // 내 손 위치 <> 잡은 물체 위치 일치
            catchObj.transform.position = transform.position;
            //catchObj.transform.localPosition = Vector3.zero;
            
            //내 손 방향 <> 잡은 물체 방향 일치
            catchObj.transform.rotation = transform.rotation;
            //catchObj.transform.localEulerAngles = Vector3.zero;
            //catchObj.transform.localRotation = Quaternion.Euler(0, 0, 0);

            
        }
        else //B. 물체가 부딪히지 않은 경우
        {

        }


        

    }

    //놓았을 때
    void DropObj()
    {
        
        //물리 적용 On
        EnablePhysics(true);
        
        ThrowObj();
        
        //가지고 있는 물체를 내 손에서 놓는다. (부모-자식 관계 해제)
        catchObj.transform.parent = null;
        catchObj = null;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }

    //물리 적용 On/Off
    void EnablePhysics(bool active)
    {
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        //1. 잡은 물체에 Rigidbody가 붙어있는지 확인
        if (rb != null)
        {
            
            rb.isKinematic = !active;
            rb.useGravity = active;
        }
        //2. 만일 Rigidbody가 있는 경우
        //3. 물리연산 적용/미적용
        //4. 중력 적용/미적용
    }

    private void ThrowObj()
    {
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
              //Rigidbody 손의 z축 방향으로 일정힘 세기로 날림
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse); 
        }
    }
}
