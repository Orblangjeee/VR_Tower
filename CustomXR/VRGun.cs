using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : XR Controller 입력에 의해서 총구가 향하는 방향으로 총알 발사
//총구 (FirePos), Ray 건, 총알 부딪힐 때 Effect, 총알쏠 때 Sound, 총알 쏠 때 Effect

//역할 : 조준점 역할을 하는 Canvas를 Ray 끝나는 지점에 위치시킨다
//CrossHair Canvas(조준점), RayDistance(Ray의 길이)

//[문제] : CrossHair가 자기멋대로 회전함
//[해결] : 조준을 제대로 할 수 있도록 Player의 카메라를 항상 바라보도록 수정

//[문제] : CrossHair가 거리에 따라 크기가 변하므로 멀리 있으면 안보인다
//[해결] : 거리에 상관없이 일정한 크기로 보이도록 한다.
//CrossHair 의 원래 사이즈, 거리에 따라 일정하게 보일 수 있도록 하는 보정값
public class VRGun : MonoBehaviour
{
    public Transform crossHair;//CrossHair Canvas (조준점)
    public Transform playercam; //Camera
    public float rayDistance = 200f; //VR Gun의 사정거리
    private Vector3 originSize; //CrossHair 의 원래 사이즈
    public float adjustScale = 0.1f; //거리에 따라 일정하게 보일 수 있도록 하는 보정값
    public int damage = 1;

    public Transform firePos; //총구(FirePos)
    public ParticleSystem impactEffect; //총알 부딪힐 때 Effect
    public ParticleSystem muzzleFlashEffect; //총알 쏠 때 Effect
    public AudioSource sound; //총알 쏠 때 Sound


    private void Update()
    {
        //CrossHair(조준점) 위치
        SetCrossHairPosition();
    }

    private void Start()
    {
        //crossHair의 원래 크기를 가져와 저장한다.
        originSize = crossHair.localScale;
    }

    private void SetCrossHairPosition()
    {
        RaycastHit hitInfo;
        //A. Ray가 어딘가에 부딪힌 경우
        if (IsHit(out hitInfo) == true)
        {
            // 부딪힌 자리에 CrossHair
            crossHair.position = hitInfo.point;
        }
        //B. Ray 아무것도 안부딪힌 경우
        else
        {
            // Ray의 끝나는 지점에 CrossHair
            Vector3 endPos = firePos.position + firePos.forward * rayDistance;
            //Ray의 시작점 + 방향 * Ray의 거리 = Ray가 끝나는 지점
            crossHair.position = endPos;
        }

        //항상 나(PlayerCam)를 바라보게 한다.
        //1. CrossHair 가 카메라를 바라보는 방향을 구한다
        Vector3 direction = playercam.position -  crossHair.position;
        //2. 해당 방향을 바라보도록 CrossHair 회전
        //crossHair.rotation = Quaternion.LookRotation(direction);
        crossHair.forward = direction;

        //[크기] 거리에 따라 일정하게 보이도록 한다.
        //palyer - crosshair 사이의 거리
        float distance = Vector3.Distance(playercam.position, crossHair.position);
        // 최종 크기 = 원래 크기 * P-C 거리 * 보정값
        crossHair.localScale = originSize * distance * adjustScale;
    }

    //XR Controller 입력에 의해서 호출되는 총 발사 함수
    public void OnactivateFire()
    {
        //총구가 향하는 방향으로 총알 발사
        print("총 발사");
        //총 발사
        Fire();
        //총알 발사 사운드 재생
        PlayFireSound();
        //총알 발사 Effect 재생
        PlayMuzzleEffect();
    }

    private void Fire()
    {
        //부딪힐 정보 담을 hit 그릇
        RaycastHit hitInfo;
        //Ray를 발사해서 부딪힌 경우?
        if (IsHit(out hitInfo))
        {
            //피격 대상의 HP를 damage 만큼 닳게 한다.
            //레이어로 체크
            if (hitInfo.collider.gameObject.layer.Equals( LayerMask.NameToLayer("Enemy"))) 
            {
                //가지고 있는 컴포넌트로 체크
                IDamaged enemy = hitInfo.collider.GetComponent<IDamaged>();
                if (enemy != null)
                {
                    enemy.DamagedProcess(damage);
                }
                /* 태그로 체크
                //Enemy의 HP를 damage(-1) 닳게 한다
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.GetComponent<Enemy>().DamagedProcess(1);
                } 
                */
               
            }

            //부딪힌 위치에서 총알 부딪힌 효과 재생
            PlayImpactEffect(hitInfo);
        }
        
    }
     
    private void PlayMuzzleEffect()
    {
        //1. 부딪힌 곳으로 파티클 이동
        muzzleFlashEffect.transform.position = firePos.position;
        
        //3. 파티클 재생
        muzzleFlashEffect.Stop();
        muzzleFlashEffect.Play();
    }

    private void PlayImpactEffect(RaycastHit hitInfo)
    {
        //1. 부딪힌 곳으로 파티클 이동
        impactEffect.transform.position = hitInfo.point;
        //2. 부딪힌 곳의 normal 방향으로 파티클 Z축 맞추기
        impactEffect.transform.forward = hitInfo.normal;
        //3. 파티클 재생
        impactEffect.Stop();
        impactEffect.Play();
    }

    private void PlayFireSound()
    {
        //5. 총알 사운드 재생
        sound.Stop();
        sound.Play();
    }

    private bool IsHit(out RaycastHit result)
    {
       
        //1. ray 만듭니다.
        Ray ray = new Ray(firePos.position, firePos.forward);
        //2. raycasthit 그릇 만들기
        RaycastHit hitInfo;
        //3. 충돌 제외할 LayerMask
        int layer = 1 << LayerMask.NameToLayer("Player");
        //4. Ray를 발사해서 부딪혔다면
        if (Physics.Raycast(ray, out hitInfo, rayDistance, ~layer))
        {
            //충돌한 경우
            //충돌한 정보 out 을 통해 반환
            result = hitInfo;
            return true;
        }
        else 
        { //충돌하지 않은 경우
            result = new RaycastHit();
            return false;
        }
        
    }
}
