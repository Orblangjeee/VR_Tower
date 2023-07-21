using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//���� : XR Controller Ű�� ������ �ش��ϴ� Hand Animation ���

// Animation (2) : ���� (�ش� Ű PlayerAction)
public class AnimateHand : MonoBehaviour
{
    
    public InputActionProperty gripAction; // Animation (1) : ��� (�ش� Ű PlayerAction[select])
    public InputActionProperty pinchAction; // Animation (2) : ���� (�ش� Ű PlayerAction)
    public Animator anim;
   
    void Start()
    {
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        //��� Ű�� �󸶳� ���������� ���� ���� �����´�.
        float gripValue = gripAction.action.ReadValue<float>(); ;
        anim.SetFloat("Grip", gripValue);

        float pinchValue = pinchAction.action.ReadValue<float>();
        anim.SetFloat("Trigger", pinchValue);
    }
}
