using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : Enemy �� Damaged�� �ǰ� ���� �� �ִ� ��� GameObject���� �ٿ���, �� Interface�� ������ �ִ� Object�� �ǰ� ���� (������ ���� �� ����)

public interface IDamaged
{
    //�����ϰ� ���� ��ɵ� (�Լ� ����, "IDamaged" �������̽� ����Ϸ��� �ݵ�� ������ ��)
    void DamagedProcess(int damage)
    {

    }
}
