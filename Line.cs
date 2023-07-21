using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : �� �տ��� �������� ������� ���� �ϳ� �׷��ش�
//Raycast ��򰡿� �ε����� �ű���� �׷��ִ� ģ��
//LineRenderer, ������� ������(Player hand), Line�� �ִ� ����

//[����]: ��� �ε��� üũ�� �ؼ� �հ� ��������.
//[�ذ�] : Raycast Ȱ���ؼ� ��򰡿� �ε����� �ε��� �������� �׷��ְ� �ε����� �ʾҴٸ� ���� �Ÿ���ŭ �׷��ش�.

public class Line : MonoBehaviour
{
    private LineRenderer lr;
    public Transform hand; //������� ������ (Player hand)
    public float maxLength = 2f; //Line�� �ִ� ����

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

 
    //�������� ������� ���� �ϳ� �׷��ش�
    void Update()
    {
        // 1. Ray : ���� ��ġ, ����
        Ray ray = new Ray(hand.position, hand.forward);
        // 2. RaycastHit �浹���� ���� �׸�
        RaycastHit hitInfo;
        // 3. Raycast �߻� (Player ���̾� ����)
        int layer = 1 << LayerMask.NameToLayer("Player");
        // A. ���� Line�� ��򰡿� �ε����ٸ�?
        if (Physics.Raycast(ray, out hitInfo, maxLength))
        {
            //1. ������ġ : hand �� ��ġ
            lr.SetPosition(0, hand.position);
            //2. ��������ġ : Ray�� �ε��� ��ġ
            lr.SetPosition(1, hitInfo.point);
        } else
        // B. ���� Line�� ��򰡿� �ε����� �ʾҴٸ�?
        {
            // 1. Line�� �����ϴ� ��ġ�� �ϳ� �˷��ش�. (hand )
            lr.SetPosition(0, hand.position);
            // 2. Line�� ������ ��ġ�� �˷��ش�. 
            //��ġ : ������ + Ư������ * Ư���Ÿ�
            lr.SetPosition(1, hand.position + hand.forward * maxLength);
        }
        
       
    }
}
