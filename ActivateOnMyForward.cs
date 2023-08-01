using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 컴포넌트가 붙어있는 특정 오브젝트 앞에서 활성화
// 특정 오브젝트(기준)
// 거리 : 기준으로부터 얼마만큼 떨어져서 위치시킬 것인지

public class ActivateOnMyForward : MonoBehaviour
{
    public Transform target; //특정 오브젝트(기준)
    public float adjustDistance = 1.0f; //거리 : 기준으로부터 얼마만큼 떨어져서 위치시킬 것인지
    public bool useFixedHeight = true; //Y축 방향을
    public bool deactiveOnAwake = true; //자동 비활성화 여부

    private void Awake()
    {
        if (deactiveOnAwake) gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Activate();
    }


    //Target 기준, Target이 바라보는 방향에서 distance만큼 떨어진 거리에서 활성화
    private void Activate()
    {
        //[문제] Target이 바라보는 방향 기준 생성되기 때문에, 물체에 가려지거나 이상한 곳에 생성되기도 함.
        //[해결] Y축 방향을 없애서 Player의 눈 높이에 생성되도록
        //Vector3 direction = new Vector3(target.forward.x, 0f ,target.forward.z).normalized;
        
        Vector3 direction = target.forward;
        if (useFixedHeight) { direction.y = 0; } //높이 고정 사용 시에, Target의 눈높이에 맞춰서 방향이 설정된다.

        //P = P0 + Vt;
        //내 위치 = Target의 Forward 방향 * distance 만큼 떨어진 위치
        transform.position = target.position + direction * adjustDistance;
        //[문제] Canvas 방향과 Target 방향을 맞춘다
        transform.forward = target.forward;
    }
   
}
