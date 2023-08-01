using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : ���̳ʸ���Ʈ�� ������ ��, ������ �ִ� Enemy�� ������ �� �߻��ϴ� ȿ�� Object Pool�� ����
// Effect ����, Effect Pool ����, Effect�� ���� Array, ���� ����ϴ� Particle
public class DynamiteEffectPool : MonoBehaviour
{
    public GameObject effectFactory; //Effect ����
    public int poolSize = 10; //Effect Pool ����
    public ParticleSystem[] effectPool; //Effect�� ���� Array
    public int currentParticle = 0; //���� ����ϴ� Particle

    void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        //Object Pool ����
        //1. PoolSize��ŭ ��ƼŬ ��Ƴ��� Pool(�迭) �� �����
        effectPool = new ParticleSystem[poolSize];
        //2. �� �濡 Particle �����ؼ� �־�α�
        for (int i = 0; i < poolSize; i++)
        {
            effectPool[i] = Instantiate(effectFactory,Vector3.zero, Quaternion.identity ).GetComponent<ParticleSystem>();
            
        }
    }

    //���� Object Pool �� ����� Particle ����ϱ�
    public void UseDynamiteEffects(Vector3 pos)
    {
        /*
       //�������� ����� ��ƼŬ�� Index ��ȣ�� ������ ��ȣ�� �Ѿ�ٸ�
       if (currentParticle +1 == poolSize)
        {
            // ó�� ����� ��ƼŬ�� ���ư���.
            currentParticle = 0;
        } 
       //�������� ����� ��ƼŬ�� Index ��ȣ�� ������ ��ȣ�� �Ѿ�� �ʾҴٸ� 
        else
        {
            //���� ����� ��ƼŬ ��ȣ�� ���� ��ƼŬ ��ȣ�� �̵�
            currentParticle ++;
        }

        /*        //0. Effect�� ��Ȱ���� �� �ֵ��� �������
         *       if (currentParticle < poolSize)
                {
                    currentParticle = currentParticle + 1;
                } else
                {
                    currentParticle = 0;
                }
        */
        
        int lastIndex = poolSize - 1;
        //���� �����ڸ� ����ؼ� 1�ٷ� ���δ�.
        currentParticle = (currentParticle >= lastIndex) ? 0 : currentParticle + 1;
        //1. Effect�� Ư�� ��ġ(Enemy)�� �̵���Ų��.
        effectPool[currentParticle].transform.position = pos;
        effectPool[currentParticle].Stop();
        effectPool[currentParticle].Play();
    }


}
