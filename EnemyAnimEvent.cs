using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : Enemy�� AnimationEvent �� ���۵Ǵ� ���� �Լ�

public class EnemyAnimEvent : MonoBehaviour
{
    /// <summary>
    /// Enemy�� Target�� ������ �� �߻��ϴ� �ִϸ��̼�
    /// </summary>
    public void OnAnimEnventAttack()
    {
        //Animation Event �߻� ��, Tower�� �����Ѵ�
        Tower.Instance.HP--;
    }
}
