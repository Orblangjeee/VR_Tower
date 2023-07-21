using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//���� : �� �տ� ���� ��ü�� ��ų� ���´�
// ���� ��ġ : ����, ������
// A. ��� -1. ���� �� �ִ� ���� -2. ���� ��ü -3. �浹 üũ�� ���� Rigidbody
// B. ����
//[����] ���� ��ü�� ������������ ���� ��ġ�� ��߳�
//[�ذ�] ��ü�� ��� ���� ������ ���������� on/off
//���� : TestPlayer ��, ���� ��ü�� ������.
//��ü�� ������ ��

//��� : VR Player�� Controller�� �°� Ű �Է� ���ġ
// ���� ������

public class PlayerHand : MonoBehaviour
{
    public enum Hand { Left, Right }; //���� ��ġ (��,��)
    public Hand m_hand = Hand.Left;

    //OVRInput.Controller controllerTouch;//���� ������ �ٸ��� VR ��Ʈ��Ű �Է�

    public float catchRadius = 0.5f; //���� �� �ִ� ����
    public GameObject catchObj; //���� ��ü
    private Rigidbody rb; //�浹üũ�� ���� Rigidbody
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
        //A. ���� ���콺�� Ŭ���ϸ�! ���
        if (Input.GetButtonDown("Fire1") && catchObj == null) 
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controllerTouch) && catchObj == null) 
        {
           
                CatchObj();
           
                
        }
        //B. ���콺 Ŭ���ߴٰ� ����! ����
        if (Input.GetButtonUp("Fire1") && catchObj != null)
        //if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controllerTouch) && catchObj != null)
        {
            
                DropObj();
           
               
        }
    }

    //����� ��
    // Hand�� �������� CatchRadius��ŭ�� �������� ���� ���� ���� = Ž�� ����
    // ��ü�� �����ִ� ��� ���� ������ �ִ� ��ü�� ��´�
    void CatchObj()
    {
        Vector3 position = transform.position; //�˻� ���� ��ġ (=�� ���� ��ġ)
        float radius = catchRadius; //�������� ���� CatchRadius
        int layerMask = 1 << LayerMask.NameToLayer("Item"); //Ư�� Layer�� ���� �� �ְ�
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);//�ش� Ž������ �ȿ� ���� ��ü���� ��� �˻��Ѵ�

        //Ž�� ���� �ȿ� ��ü�� �ִ��� ������
        int selectedIndex = -1;

        // ������ ��ü�� �浹�� ���, ���� ������ �ִ� ��ü�� Ÿ������ �Ѵ�. -> ���� ��ü�� ���ȣ�� selectedIndex�� �˷��ش�
        //1. Ž�� ���� �ȿ� ��ü�� �ִ��� ����
        if (hits != null && hits.Length > 0)
        {
            //2. selectedIndex�� 0�� ���� ���� �� �ֵ��� ���Ҵ�
            selectedIndex = 0;
            //3. �ε��� �༮�� �� ���� ª�� �Ÿ��� �༮�� ������ �� �ֵ��� �ݺ� �˻�
            for (int i = 1; i <hits.Length; i++)
            { //4. �񱳸� ���� �� �Ÿ��� ����� �浹ü�� index�� ���Ѵ�
                float currentDis = Vector3.Distance(position, hits[selectedIndex].transform.position);  // ���� �� ������ �Ǵ� �浹ü - �� �Ÿ�
                float nextDis = Vector3.Distance(position, hits[i].transform.position); // ���� ���� �浹ü - �� �Ÿ�
             //5. ���� ���� �浹ü �Ÿ��� ���� ���� �浹ü �Ÿ��� ��
             if(currentDis > nextDis)
                {
                    //6. �� ª�� �Ÿ��� �ִ� �浹ü�� index�� selectedindex�� �־��ش� 
                    selectedIndex = i;
                }
            }
        }
        //A. ��ü�� �ε��� ��� 
        if (selectedIndex != -1)
        {

            //��ü�� ��´�. -> �� ���� ��ġ�� �̵���Ų��.
             catchObj = hits[selectedIndex].gameObject;
            //�������� off
            EnablePhysics(false);
            
            //��ü�� ���� �������� ���󰡵��� �ڽ����� ���´�.
            catchObj.transform.parent = transform;
            
            // �� �� ��ġ <> ���� ��ü ��ġ ��ġ
            catchObj.transform.position = transform.position;
            //catchObj.transform.localPosition = Vector3.zero;
            
            //�� �� ���� <> ���� ��ü ���� ��ġ
            catchObj.transform.rotation = transform.rotation;
            //catchObj.transform.localEulerAngles = Vector3.zero;
            //catchObj.transform.localRotation = Quaternion.Euler(0, 0, 0);

            
        }
        else //B. ��ü�� �ε����� ���� ���
        {

        }


        

    }

    //������ ��
    void DropObj()
    {
        
        //���� ���� On
        EnablePhysics(true);
        
        ThrowObj();
        
        //������ �ִ� ��ü�� �� �տ��� ���´�. (�θ�-�ڽ� ���� ����)
        catchObj.transform.parent = null;
        catchObj = null;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }

    //���� ���� On/Off
    void EnablePhysics(bool active)
    {
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        //1. ���� ��ü�� Rigidbody�� �پ��ִ��� Ȯ��
        if (rb != null)
        {
            
            rb.isKinematic = !active;
            rb.useGravity = active;
        }
        //2. ���� Rigidbody�� �ִ� ���
        //3. �������� ����/������
        //4. �߷� ����/������
    }

    private void ThrowObj()
    {
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
              //Rigidbody ���� z�� �������� ������ ����� ����
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse); 
        }
    }
}
