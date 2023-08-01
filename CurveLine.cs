using System.Collections;
using System.Collections.Generic;

using UnityEngine;


//역할 : hand 곡선 형태의 LineRender 를 그리고 Line이 부딪힌 곳에 대한 정보를 가져온다
// LineRenderer, PlayerHand, 곡선의 거리( Far ), 곡선의 높낮이 (down), 곡선의 점 개수, Bezier 커브를 그리기 위한 기준점(P0, P1,P2)
//역할 : Line 부딪힌 곳으로 Teleport 이동하고 싶다
// 충돌체크, 충돌한 지점에 대한 정보



[RequireComponent(typeof(LineRenderer))]
public class CurveLine : MonoBehaviour
{
    private LineRenderer lr;
    public Transform hand; //playerHand
    public float far = 4f; //곡선의 거리
    public float down = 2f; //곡선의 높낮이
    public int dotCount = 50; //곡선의 점 개수
    private Vector3 p0, p1, p2; //Beizer 커브를 그릴 기준점
    private int count = 0; //곡선이 충돌하기 전까지 1개씩 카운트할 내가 그린 점의 개수
    public Vector3 telePoint; // 충돌한 지점에 대한 정보
    public Transform player;
    private bool teleOn = false;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = dotCount;
    }

    // Update is called once per frame
    void Update()
    {
        //사용자 입력에 따라 곡선 Curve를 On/Off
        //1. 버튼을 누르면 LineRender 보여준다
        if (Input.GetButtonDown("Fire3"))
        {
            //LR 활성화
            lr.enabled = true;
        }
        //2. 버튼을 누르는 동안 곡선을 갱신한다.
        else if (Input.GetButton("Fire3"))
        {
            PositionWithHand(); //P0, p1, p2 playerHand 기준으로 위치
            DrawCurve(); //곡선그리기
        }
        //3. 버튼을 뗄 때 Line 감추고 필요한 동작 실행
        else if (Input.GetButtonUp("Fire3"))
        {
            
            if (teleOn)
            {
            Teleport(); //해당 공간으로 Teleport 한다
            lr.enabled = false; //LR 비활성화

            }
            else
            {
                lr.enabled = false; //LR 비활성화
               
            }
        }
    }

    private void Teleport()
    {
       
        //telePoint 지점을 순간이동
        //player 가 가진 cc 잠시 비활성화
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.position = telePoint + Vector3.up; //player 이동
        cc.enabled = true;
        //순간이동 하고 나면 telePoint 지점은 Reset
        telePoint = Vector3.zero; //리셋
    }

    //p0, p1, p2 를 구하는 공간

    private void PositionWithHand()
    {
        //1. p0 위치 구하기
        p0 = hand.position;
        //2. p1 위치 구하기 ( p0 위치 기준 Hand 방향으로 Far 만큼 떨어진 거리)
        p1 = p0 + hand.forward * far;
        //3. p3 위치 구하기 (p1 위치 기준 Hand 방향으로 Far 만큼 Down 방향으로 Down 만큼 떨어진 거리)
        p2 = p1 + hand.forward * far + Vector3.down * down;
    }

    //곡선을 그린다
    private void DrawCurve()
    {
        //1. 충돌한 위치와 방향을 구하기 위해 필요한 이전 점(tpos)의 위치
        Vector3 prePos = Vector3.zero;
        count = 0; //초기화
        //점의 개수만큼 반복해서 곡선이 될 T의 점을 찍어서 LR 그려달라고 한다.
        for (int i = 0; i < dotCount; i++)
        {
            float t = i / (float)dotCount; //1. t의 값을 구한다.
            Vector3 tPosition = Bezier(p0, p1, p2, t); //2. t의 위치를 구한다
            //lr.SetPosition(i, tPosition); //3. t의 위치를 LR 한테 알려준다.

            //3. 곡선의 진행방향에서 충돌이 일어났는지 여부를 확인
            // 3-A. 만일 부딪힌 경우
            if (i>0 && IsHit(prePos, tPosition))
            {
                // 부딪힌 곳까지의 점의 개수가 LR이 그리는 총 점의 개수
                lr.positionCount = count;
                //부딪힌 곳까지만 그리고 더이상 곡선을 그리지 않습니다
                return;
            }
            //3-B. 만일 부딪히지 않은 경우
            else
            {
                // 곡선을 계속해서 그립니다.
                // count + 1
                // 점을 어느 위치에 찍지? (tPosition)
                AddPointToLineRenderer(tPosition);
            }
            prePos = tPosition; //현재 위치 저장
        }
        //LR이 가지고 있는 전체 점의 개수를 count랑 동일
        count = 0;
    }

    //[이전 tPos 위치]에서 [현재 tPos 위치]로 Ray를 쏴서 충돌 여부 검사
    private bool IsHit(Vector3 prePos, Vector3 pos)
    {
        //1. 충돌 체크할 Ray를 쏠 방향을 구한다. (현재위치 - 이전위치)
        Vector3 direction = pos - prePos;
        direction.Normalize();
        //2. Ray를 쏠 거리(길이) (현재위치 <> 이전위치)
        float distance = Vector3.Distance(pos, prePos);
        //3. Ray 생성 (위치(이전위치), 방향(이전위치->현재위치 방향))
        Ray ray = new Ray(prePos, direction);
        RaycastHit hitInfo;//4. RaycastHit 충돌 정보 그릇
        //5-a. Ray 발사해서 맞으면 true
        if (Physics.Raycast(ray, out hitInfo,distance))
        {
            //충돌한 지점에 점 하나 그려준다.
            AddPointToLineRenderer(hitInfo.point);
            //만일 부딪힌 물체의 tag가 "telePoint" 이면
            if (hitInfo.collider.CompareTag("Telepoint"))
            {
                teleOn = true;
                //충돌한 곳에 정보를 telePoint 에 넣어준다.
                telePoint = hitInfo.transform.position;
            } else
            {
                teleOn = false;
            }

            return true;

        } else
        //5-b. 안맞으면 false
        {
            teleOn = false;
            return false;

        }
    }

    //곡선을 계속해서 그리기 위해 LR의 점 1개씩 추가
    private void AddPointToLineRenderer(Vector3 pos)
    {
        //0. 만일 충돌 했을 때 count에 있는 점의 개수가 positionCount 보다 많을 대
        if (count >= lr.positionCount)
        {
            //카운트 +1 만큼 여유분을 LR의 점 전체 개수로 넣어준다
            lr.positionCount = count + 1;
        }

        //1. 점을 LR에 추가한다.
        lr.SetPosition(count, pos);
        //2. 점 LR을 추가했으니까 내가 그린 점 count +1
        count++;
    }

    private Vector3 Bezier (Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //p0 - p1 점 사이의 선형보간 t의 위치 구하기
        Vector3 p0p1 = Vector3.Lerp(p0, p1, t);
        //p1 - p2 점 사이의 선형보간 t의 위치 구하기
        Vector3 p1p2 = Vector3.Lerp(p1, p2, t);
        //p0p1 - p1p2 점 사이의 선형보간 t의 위치 구하기
        Vector3 tPosition = Vector3.Lerp(p0p1, p1p2, t);
        //Bezier 커비를 사용한 t의 최종 위치
        return tPosition;
    }
}
