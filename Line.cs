using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 내 손에서 직선으로 뻗어나가는 선을 하나 그려준다
//Raycast 어딘가에 부딪히면 거기까지 그려주는 친구
//LineRenderer, 뻗어나가는 기준점(Player hand), Line의 최대 길이

//[문제]: 어딘가 부딪힘 체크를 해서 뚫고 지나간다.
//[해결] : Raycast 활용해서 어딘가에 부딪히면 부딪힌 곳까지만 그려주고 부딪히지 않았다면 원래 거리만큼 그려준다.

public class Line : MonoBehaviour
{
    private LineRenderer lr;
    public Transform hand; //뻗어나가는 기준점 (Player hand)
    public float maxLength = 2f; //Line의 최대 길이

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

 
    //직선으로 뻗어나가는 선을 하나 그려준다
    void Update()
    {
        // 1. Ray : 시작 위치, 방향
        Ray ray = new Ray(hand.position, hand.forward);
        // 2. RaycastHit 충돌정보 담을 그릇
        RaycastHit hitInfo;
        // 3. Raycast 발사 (Player 레이어 제외)
        int layer = 1 << LayerMask.NameToLayer("Player");
        // A. 만일 Line이 어딘가에 부딪혔다면?
        if (Physics.Raycast(ray, out hitInfo, maxLength))
        {
            //1. 시작위치 : hand 의 위치
            lr.SetPosition(0, hand.position);
            //2. 끝나는위치 : Ray가 부딪힌 위치
            lr.SetPosition(1, hitInfo.point);
        } else
        // B. 만일 Line이 어딘가에 부딪히지 않았다면?
        {
            // 1. Line이 시작하는 위치를 하나 알려준다. (hand )
            lr.SetPosition(0, hand.position);
            // 2. Line이 끝나는 위치를 알려준다. 
            //위치 : 시작점 + 특정방향 * 특정거리
            lr.SetPosition(1, hand.position + hand.forward * maxLength);
        }
        
       
    }
}
