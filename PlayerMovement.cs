using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : Player�� �������� ���������� �����Ѵ�.
//  A. Player�� ������� �Է¿� ���� ��/��/��/��� ������ �ӷ����� �����δ�.
//    - ������� �Է� (a.��-�� / b.��-��)
//    - ������ �ӷ�
//  A-1. �߷� ������ �޽��ϴ�.
//    - CharacterController ������Ʈ
//    - �߷� ���ӵ�
//    - �߷� ������ (*�߷°��ӵ��� ���� ������ ������ �߷°�)
//  B. Player�� ������� �Է¿� ���� ��/��� ȸ����Ų��.
//    - ������� �Է� (���콺 ��-��)
//    - ȸ�� �ӷ� (�ΰ���)
//    - ������� �Է¿� ���� ȸ�� ������ ����
//  C. Player�� ������� �Է¿� ���� ������ �ӷ����� ��/�Ϸ� ȸ����Ų��.
//    - ������� �Է� (���콺 ��-��)
//    - ȸ�� �ӷ� (B���� ���� �� ����)
//    - ������� �Է¿� ���� ȸ�� ������ ����
//    - ���� ����Ÿ���.. ī�޶� ȸ��
//    - ��-�� ȸ�� ���� ����
//  D. ������� �Է¿� ���� Player�� "n��" �����ϰ� �մϴ�.
//    - ������� �Է�(Jump)
//    - Jump �� �Ŀ�
//    - �ִ� ���� ���� Ƚ��
//    - ���� ���� ���� Ƚ��

[RequireComponent(typeof (CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Movement & Gravity
    // �̵� �ӷ�
    public float moveSpeed = 5f;
    // CharacterController ������Ʈ
    private CharacterController cc;
    // �߷� ���ӵ� ( 9.81m/s )
    public float gravity = -9.81f;
    // �߷� ������ (*�߷°��ӵ��� ���� ������ ������ �߷°�)
    public float yVelocity;
    #endregion

    #region Jump
    // Jump �� �Ŀ�
    public float jumpPower = 4.0f;
    // ���� Jump�� �ߴ��� ����
    public bool isJump = false;
    // �ִ� ���� ���� Ƚ��
    public int maxJumpCount = 1;
    // ���� ���� ���� Ƚ��
    private int jumpCount = 0;
    #endregion

    #region Rotator
    // ȸ�� �ӷ� (�ΰ���)
    public float mouseSensitivity = 500f;
    // ������� �Է¿� ���� ��-�� ȸ�� ������ ����
    private float horizontalAngle;
    // ������� �Է¿� ���� ��-�� ȸ�� ������ ����
    private float verticalAngle;
    // ���� ����Ÿ���.. ī�޶� ȸ��
    public GameObject face;
    // ��-�� ȸ�� ���� ���� (�ʱⰪ=90��)  -90 ~ +90
    public float limitAngle = 90f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // ���콺 Ŀ���� ������ �ʰ��ϰ�,
        Cursor.visible = false;
        // ���콺 Ŀ���� ȭ�� ����� ������Ų��.
        Cursor.lockState = CursorLockMode.Locked;
        // ChacterController ������Ʈ �����´� (=�ʱ�ȭ)
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        RotateHorizontal();

        RotateVertical();
    }

    // Player�� �̵���Ű�� �Լ�
    // [����!] : Player�� ���� Z���� �ƴ�, Unity�� ���� Z������ �̵��Ѵ�
    // [�ذ�]  : Player�� Local �������� �̵��Ѵ�.
    private void MovePlayer()
    {
        // 1. Player�� ������� �Է¿� ����
        //    a.��-��
        float vertical = Input.GetAxis("Vertical");
        //    b.��-���
        float horizontal = Input.GetAxis("Horizontal");
        // 2. ����� �Է¿� ���� World�� ������ ���Ѵ�.
        // Vector3 direction = new Vector3(horizontal, 0, vertical);
        // 2-1. ����� �Է¿� ���� Local�� ������ ���Ѵ�.
        // - Local �յ� ���� * vetical / local �¿� ���� * horizontal
        Vector3 direction = transform.forward * vertical + transform.right * horizontal;
        // 3. Ư�� �������� ������ �ӷ����� �����δ�.
        //    P = Po + Vt
        // transform.position += direction * moveSpeed * Time.deltaTime;

        // [����] �߷��� ������ ��� ������ ���� ���� Ŀ������.
        // [�ذ�] ���� ���� ���� �پ����� ��, ������ ����Ǵ� �߷��� Reset
        // if (cc.collisionFlags == CollisionFlags.Below)
        if (cc.isGrounded == true)
        {
            // �߷°��� 0���� Reset
            yVelocity = 0f;
            // ���� ��������� Jump�� maxJumpCount ȸ����ŭ ������ ����
            jumpCount = maxJumpCount;
        }

        // ����, �������� Ƚ���� �����ִ� ���¿��� Player�� Jump ��ư�� ������, 
        if (jumpCount > 0 && Input.GetButtonDown("Jump"))
        {
            // JumpPower��ŭ �ڱ�ģ��.
            yVelocity = jumpPower;
            // ������ �ϸ�.. ���� ���� Ƚ���� -1 ����.
            jumpCount--;
        }




        // �߷�(F) ����
        // 1. F(�߷�) = m(����=1)*a(�߷°��ӵ�=gravity)
        // 2. V(�ӵ�) = Vo(�߷´�����) + a(�߷°��ӵ�) * t
        yVelocity += gravity * Time.deltaTime;
        // �߷��� �޵��� Player�� Y�� ������ �����Ѵ�.
        direction.y = yVelocity;


        // * CharacterController�� ���� �����δ�.
        cc.Move(direction * moveSpeed * Time.deltaTime);
    }

    //  B. Player�� ������� �Է¿� ���� ��/��� ȸ����Ų��.
    private void RotateHorizontal()
    {
        // 1. ������� �Է°� (���콺)
        //  a. ��-�� �̵���
        float horizontal = Input.GetAxis("Mouse X");
        // 2. ���콺 �ΰ����� ���� �Է� ���� ����
        float turnPlayer = horizontal * mouseSensitivity * Time.deltaTime;
        // 3. * ���콺 �̵��� ���� ��-�� ȸ���� ����
        horizontalAngle += turnPlayer;
        // 4. ������ ȸ�� ���� Player�� ȸ���� �ݿ�
        transform.localEulerAngles = new Vector3(0, horizontalAngle, 0);
    }

    // C. Player ������ ��-�Ϸ� ȸ����Ű�� �Լ�
    // [����] : ����ȸ���� �� �Ǵµ�.. 360�� ��� ȸ���ع�����.
    // [�ذ�] : ���� ȸ�� ���� ������ ���� �ذ��Ѵ�.
    private void RotateVertical()
    {
        // 1. ������� �Է°��� �����´�. (��-��) ( -1 ~ +1 )
        float vertical = Input.GetAxis("Mouse Y");
        // 2. ���콺 �ΰ����� ���� �Է� ���� ����
        float turnFace = vertical * mouseSensitivity * Time.deltaTime;
        // 3. * ���콺 �̵��� ���� ��-�� ȸ���� ����
        verticalAngle += turnFace;
        // * ���� ȸ�� ���� ������ ���� �ذ��Ѵ�.
        verticalAngle = Mathf.Clamp(verticalAngle, -limitAngle, limitAngle);
        // 4. ������ ȸ�� ���� Face(=Camera)�� ȸ���� �ݿ�
        face.transform.localEulerAngles = new Vector3(-verticalAngle, 0, 0);

    }
}
