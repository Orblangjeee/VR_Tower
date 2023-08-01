using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : XR Controller �Է¿� ���ؼ� �ѱ��� ���ϴ� �������� �Ѿ� �߻�
//�ѱ� (FirePos), Ray ��, �Ѿ� �ε��� �� Effect, �Ѿ˽� �� Sound, �Ѿ� �� �� Effect

//���� : ������ ������ �ϴ� Canvas�� Ray ������ ������ ��ġ��Ų��
//CrossHair Canvas(������), RayDistance(Ray�� ����)

//[����] : CrossHair�� �ڱ�ڴ�� ȸ����
//[�ذ�] : ������ ����� �� �� �ֵ��� Player�� ī�޶� �׻� �ٶ󺸵��� ����

//[����] : CrossHair�� �Ÿ��� ���� ũ�Ⱑ ���ϹǷ� �ָ� ������ �Ⱥ��δ�
//[�ذ�] : �Ÿ��� ������� ������ ũ��� ���̵��� �Ѵ�.
//CrossHair �� ���� ������, �Ÿ��� ���� �����ϰ� ���� �� �ֵ��� �ϴ� ������
public class VRGun : MonoBehaviour
{
    public Transform crossHair;//CrossHair Canvas (������)
    public Transform playercam; //Camera
    public float rayDistance = 200f; //VR Gun�� �����Ÿ�
    private Vector3 originSize; //CrossHair �� ���� ������
    public float adjustScale = 0.1f; //�Ÿ��� ���� �����ϰ� ���� �� �ֵ��� �ϴ� ������
    public int damage = 1;

    public Transform firePos; //�ѱ�(FirePos)
    public ParticleSystem impactEffect; //�Ѿ� �ε��� �� Effect
    public ParticleSystem muzzleFlashEffect; //�Ѿ� �� �� Effect
    public AudioSource sound; //�Ѿ� �� �� Sound


    private void Update()
    {
        //CrossHair(������) ��ġ
        SetCrossHairPosition();
    }

    private void Start()
    {
        //crossHair�� ���� ũ�⸦ ������ �����Ѵ�.
        originSize = crossHair.localScale;
    }

    private void SetCrossHairPosition()
    {
        RaycastHit hitInfo;
        //A. Ray�� ��򰡿� �ε��� ���
        if (IsHit(out hitInfo) == true)
        {
            // �ε��� �ڸ��� CrossHair
            crossHair.position = hitInfo.point;
        }
        //B. Ray �ƹ��͵� �Ⱥε��� ���
        else
        {
            // Ray�� ������ ������ CrossHair
            Vector3 endPos = firePos.position + firePos.forward * rayDistance;
            //Ray�� ������ + ���� * Ray�� �Ÿ� = Ray�� ������ ����
            crossHair.position = endPos;
        }

        //�׻� ��(PlayerCam)�� �ٶ󺸰� �Ѵ�.
        //1. CrossHair �� ī�޶� �ٶ󺸴� ������ ���Ѵ�
        Vector3 direction = playercam.position -  crossHair.position;
        //2. �ش� ������ �ٶ󺸵��� CrossHair ȸ��
        //crossHair.rotation = Quaternion.LookRotation(direction);
        crossHair.forward = direction;

        //[ũ��] �Ÿ��� ���� �����ϰ� ���̵��� �Ѵ�.
        //palyer - crosshair ������ �Ÿ�
        float distance = Vector3.Distance(playercam.position, crossHair.position);
        // ���� ũ�� = ���� ũ�� * P-C �Ÿ� * ������
        crossHair.localScale = originSize * distance * adjustScale;
    }

    //XR Controller �Է¿� ���ؼ� ȣ��Ǵ� �� �߻� �Լ�
    public void OnactivateFire()
    {
        //�ѱ��� ���ϴ� �������� �Ѿ� �߻�
        print("�� �߻�");
        //�� �߻�
        Fire();
        //�Ѿ� �߻� ���� ���
        PlayFireSound();
        //�Ѿ� �߻� Effect ���
        PlayMuzzleEffect();
    }

    private void Fire()
    {
        //�ε��� ���� ���� hit �׸�
        RaycastHit hitInfo;
        //Ray�� �߻��ؼ� �ε��� ���?
        if (IsHit(out hitInfo))
        {
            //�ǰ� ����� HP�� damage ��ŭ ��� �Ѵ�.
            //���̾�� üũ
            if (hitInfo.collider.gameObject.layer.Equals( LayerMask.NameToLayer("Enemy"))) 
            {
                //������ �ִ� ������Ʈ�� üũ
                IDamaged enemy = hitInfo.collider.GetComponent<IDamaged>();
                if (enemy != null)
                {
                    enemy.DamagedProcess(damage);
                }
                /* �±׷� üũ
                //Enemy�� HP�� damage(-1) ��� �Ѵ�
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.GetComponent<Enemy>().DamagedProcess(1);
                } 
                */
               
            }

            //�ε��� ��ġ���� �Ѿ� �ε��� ȿ�� ���
            PlayImpactEffect(hitInfo);
        }
        
    }
     
    private void PlayMuzzleEffect()
    {
        //1. �ε��� ������ ��ƼŬ �̵�
        muzzleFlashEffect.transform.position = firePos.position;
        
        //3. ��ƼŬ ���
        muzzleFlashEffect.Stop();
        muzzleFlashEffect.Play();
    }

    private void PlayImpactEffect(RaycastHit hitInfo)
    {
        //1. �ε��� ������ ��ƼŬ �̵�
        impactEffect.transform.position = hitInfo.point;
        //2. �ε��� ���� normal �������� ��ƼŬ Z�� ���߱�
        impactEffect.transform.forward = hitInfo.normal;
        //3. ��ƼŬ ���
        impactEffect.Stop();
        impactEffect.Play();
    }

    private void PlayFireSound()
    {
        //5. �Ѿ� ���� ���
        sound.Stop();
        sound.Play();
    }

    private bool IsHit(out RaycastHit result)
    {
       
        //1. ray ����ϴ�.
        Ray ray = new Ray(firePos.position, firePos.forward);
        //2. raycasthit �׸� �����
        RaycastHit hitInfo;
        //3. �浹 ������ LayerMask
        int layer = 1 << LayerMask.NameToLayer("Player");
        //4. Ray�� �߻��ؼ� �ε����ٸ�
        if (Physics.Raycast(ray, out hitInfo, rayDistance, ~layer))
        {
            //�浹�� ���
            //�浹�� ���� out �� ���� ��ȯ
            result = hitInfo;
            return true;
        }
        else 
        { //�浹���� ���� ���
            result = new RaycastHit();
            return false;
        }
        
    }
}
