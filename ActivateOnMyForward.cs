using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : ������Ʈ�� �پ��ִ� Ư�� ������Ʈ �տ��� Ȱ��ȭ
// Ư�� ������Ʈ(����)
// �Ÿ� : �������κ��� �󸶸�ŭ �������� ��ġ��ų ������

public class ActivateOnMyForward : MonoBehaviour
{
    public Transform target; //Ư�� ������Ʈ(����)
    public float adjustDistance = 1.0f; //�Ÿ� : �������κ��� �󸶸�ŭ �������� ��ġ��ų ������
    public bool useFixedHeight = true; //Y�� ������
    public bool deactiveOnAwake = true; //�ڵ� ��Ȱ��ȭ ����

    private void Awake()
    {
        if (deactiveOnAwake) gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Activate();
    }


    //Target ����, Target�� �ٶ󺸴� ���⿡�� distance��ŭ ������ �Ÿ����� Ȱ��ȭ
    private void Activate()
    {
        //[����] Target�� �ٶ󺸴� ���� ���� �����Ǳ� ������, ��ü�� �������ų� �̻��� ���� �����Ǳ⵵ ��.
        //[�ذ�] Y�� ������ ���ּ� Player�� �� ���̿� �����ǵ���
        //Vector3 direction = new Vector3(target.forward.x, 0f ,target.forward.z).normalized;
        
        Vector3 direction = target.forward;
        if (useFixedHeight) { direction.y = 0; } //���� ���� ��� �ÿ�, Target�� �����̿� ���缭 ������ �����ȴ�.

        //P = P0 + Vt;
        //�� ��ġ = Target�� Forward ���� * distance ��ŭ ������ ��ġ
        transform.position = target.position + direction * adjustDistance;
        //[����] Canvas ����� Target ������ �����
        transform.forward = target.forward;
    }
   
}
