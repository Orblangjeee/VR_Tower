using System.Collections;
using System.Collections.Generic;

using UnityEngine;


//���� : hand � ������ LineRender �� �׸��� Line�� �ε��� ���� ���� ������ �����´�
// LineRenderer, PlayerHand, ��� �Ÿ�( Far ), ��� ������ (down), ��� �� ����, Bezier Ŀ�긦 �׸��� ���� ������(P0, P1,P2)
//���� : Line �ε��� ������ Teleport �̵��ϰ� �ʹ�
// �浹üũ, �浹�� ������ ���� ����



[RequireComponent(typeof(LineRenderer))]
public class CurveLine : MonoBehaviour
{
    private LineRenderer lr;
    public Transform hand; //playerHand
    public float far = 4f; //��� �Ÿ�
    public float down = 2f; //��� ������
    public int dotCount = 50; //��� �� ����
    private Vector3 p0, p1, p2; //Beizer Ŀ�긦 �׸� ������
    private int count = 0; //��� �浹�ϱ� ������ 1���� ī��Ʈ�� ���� �׸� ���� ����
    public Vector3 telePoint; // �浹�� ������ ���� ����
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
        //����� �Է¿� ���� � Curve�� On/Off
        //1. ��ư�� ������ LineRender �����ش�
        if (Input.GetButtonDown("Fire3"))
        {
            //LR Ȱ��ȭ
            lr.enabled = true;
        }
        //2. ��ư�� ������ ���� ��� �����Ѵ�.
        else if (Input.GetButton("Fire3"))
        {
            PositionWithHand(); //P0, p1, p2 playerHand �������� ��ġ
            DrawCurve(); //��׸���
        }
        //3. ��ư�� �� �� Line ���߰� �ʿ��� ���� ����
        else if (Input.GetButtonUp("Fire3"))
        {
            
            if (teleOn)
            {
            Teleport(); //�ش� �������� Teleport �Ѵ�
            lr.enabled = false; //LR ��Ȱ��ȭ

            }
            else
            {
                lr.enabled = false; //LR ��Ȱ��ȭ
               
            }
        }
    }

    private void Teleport()
    {
       
        //telePoint ������ �����̵�
        //player �� ���� cc ��� ��Ȱ��ȭ
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.position = telePoint + Vector3.up; //player �̵�
        cc.enabled = true;
        //�����̵� �ϰ� ���� telePoint ������ Reset
        telePoint = Vector3.zero; //����
    }

    //p0, p1, p2 �� ���ϴ� ����

    private void PositionWithHand()
    {
        //1. p0 ��ġ ���ϱ�
        p0 = hand.position;
        //2. p1 ��ġ ���ϱ� ( p0 ��ġ ���� Hand �������� Far ��ŭ ������ �Ÿ�)
        p1 = p0 + hand.forward * far;
        //3. p3 ��ġ ���ϱ� (p1 ��ġ ���� Hand �������� Far ��ŭ Down �������� Down ��ŭ ������ �Ÿ�)
        p2 = p1 + hand.forward * far + Vector3.down * down;
    }

    //��� �׸���
    private void DrawCurve()
    {
        //1. �浹�� ��ġ�� ������ ���ϱ� ���� �ʿ��� ���� ��(tpos)�� ��ġ
        Vector3 prePos = Vector3.zero;
        count = 0; //�ʱ�ȭ
        //���� ������ŭ �ݺ��ؼ� ��� �� T�� ���� �� LR �׷��޶�� �Ѵ�.
        for (int i = 0; i < dotCount; i++)
        {
            float t = i / (float)dotCount; //1. t�� ���� ���Ѵ�.
            Vector3 tPosition = Bezier(p0, p1, p2, t); //2. t�� ��ġ�� ���Ѵ�
            //lr.SetPosition(i, tPosition); //3. t�� ��ġ�� LR ���� �˷��ش�.

            //3. ��� ������⿡�� �浹�� �Ͼ���� ���θ� Ȯ��
            // 3-A. ���� �ε��� ���
            if (i>0 && IsHit(prePos, tPosition))
            {
                // �ε��� �������� ���� ������ LR�� �׸��� �� ���� ����
                lr.positionCount = count;
                //�ε��� �������� �׸��� ���̻� ��� �׸��� �ʽ��ϴ�
                return;
            }
            //3-B. ���� �ε����� ���� ���
            else
            {
                // ��� ����ؼ� �׸��ϴ�.
                // count + 1
                // ���� ��� ��ġ�� ����? (tPosition)
                AddPointToLineRenderer(tPosition);
            }
            prePos = tPosition; //���� ��ġ ����
        }
        //LR�� ������ �ִ� ��ü ���� ������ count�� ����
        count = 0;
    }

    //[���� tPos ��ġ]���� [���� tPos ��ġ]�� Ray�� ���� �浹 ���� �˻�
    private bool IsHit(Vector3 prePos, Vector3 pos)
    {
        //1. �浹 üũ�� Ray�� �� ������ ���Ѵ�. (������ġ - ������ġ)
        Vector3 direction = pos - prePos;
        direction.Normalize();
        //2. Ray�� �� �Ÿ�(����) (������ġ <> ������ġ)
        float distance = Vector3.Distance(pos, prePos);
        //3. Ray ���� (��ġ(������ġ), ����(������ġ->������ġ ����))
        Ray ray = new Ray(prePos, direction);
        RaycastHit hitInfo;//4. RaycastHit �浹 ���� �׸�
        //5-a. Ray �߻��ؼ� ������ true
        if (Physics.Raycast(ray, out hitInfo,distance))
        {
            //�浹�� ������ �� �ϳ� �׷��ش�.
            AddPointToLineRenderer(hitInfo.point);
            //���� �ε��� ��ü�� tag�� "telePoint" �̸�
            if (hitInfo.collider.CompareTag("Telepoint"))
            {
                teleOn = true;
                //�浹�� ���� ������ telePoint �� �־��ش�.
                telePoint = hitInfo.transform.position;
            } else
            {
                teleOn = false;
            }

            return true;

        } else
        //5-b. �ȸ����� false
        {
            teleOn = false;
            return false;

        }
    }

    //��� ����ؼ� �׸��� ���� LR�� �� 1���� �߰�
    private void AddPointToLineRenderer(Vector3 pos)
    {
        //0. ���� �浹 ���� �� count�� �ִ� ���� ������ positionCount ���� ���� ��
        if (count >= lr.positionCount)
        {
            //ī��Ʈ +1 ��ŭ �������� LR�� �� ��ü ������ �־��ش�
            lr.positionCount = count + 1;
        }

        //1. ���� LR�� �߰��Ѵ�.
        lr.SetPosition(count, pos);
        //2. �� LR�� �߰������ϱ� ���� �׸� �� count +1
        count++;
    }

    private Vector3 Bezier (Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //p0 - p1 �� ������ �������� t�� ��ġ ���ϱ�
        Vector3 p0p1 = Vector3.Lerp(p0, p1, t);
        //p1 - p2 �� ������ �������� t�� ��ġ ���ϱ�
        Vector3 p1p2 = Vector3.Lerp(p1, p2, t);
        //p0p1 - p1p2 �� ������ �������� t�� ��ġ ���ϱ�
        Vector3 tPosition = Vector3.Lerp(p0p1, p1p2, t);
        //Bezier Ŀ�� ����� t�� ���� ��ġ
        return tPosition;
    }
}
