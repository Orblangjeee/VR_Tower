using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : Enemy 등 Damaged를 피격 당할 수 있는 모든 GameObject에게 붙여서, 이 Interface를 가지고 있는 Object를 피격 가능 (데미지 입힐 수 있음)

public interface IDamaged
{
    //구현하고 싶은 기능들 (함수 형태, "IDamaged" 인터페이스 사용하려면 반드시 만들어야 함)
    void DamagedProcess(int damage)
    {

    }
}
