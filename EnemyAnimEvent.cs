using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : Enemy의 AnimationEvent 시 시작되는 공격 함수

public class EnemyAnimEvent : MonoBehaviour
{
    /// <summary>
    /// Enemy가 Target을 공격할 때 발생하는 애니메이션
    /// </summary>
    public void OnAnimEnventAttack()
    {
        //Animation Event 발생 시, Tower를 공격한다
        Tower.Instance.HP--;
    }
}
