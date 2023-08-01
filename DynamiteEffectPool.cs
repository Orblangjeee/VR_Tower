using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 다이너마이트가 폭발할 때, 주위에 있는 Enemy를 폭파할 때 발생하는 효과 Object Pool로 관리
// Effect 공장, Effect Pool 개수, Effect를 담을 Array, 현재 사용하는 Particle
public class DynamiteEffectPool : MonoBehaviour
{
    public GameObject effectFactory; //Effect 공장
    public int poolSize = 10; //Effect Pool 개수
    public ParticleSystem[] effectPool; //Effect를 담을 Array
    public int currentParticle = 0; //현재 사용하는 Particle

    void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        //Object Pool 생성
        //1. PoolSize만큼 파티클 담아놓을 Pool(배열) 방 만들기
        effectPool = new ParticleSystem[poolSize];
        //2. 각 방에 Particle 생성해서 넣어두기
        for (int i = 0; i < poolSize; i++)
        {
            effectPool[i] = Instantiate(effectFactory,Vector3.zero, Quaternion.identity ).GetComponent<ParticleSystem>();
            
        }
    }

    //내가 Object Pool 로 사용한 Particle 사용하기
    public void UseDynamiteEffects(Vector3 pos)
    {
        /*
       //다음으로 사용할 파티클의 Index 번호가 마지막 번호를 넘어갔다면
       if (currentParticle +1 == poolSize)
        {
            // 처음 사용한 파티클로 돌아간다.
            currentParticle = 0;
        } 
       //다음으로 사용할 파티클의 Index 번호가 마지막 번호를 넘어가지 않았다면 
        else
        {
            //현재 사용한 파티클 번호를 다음 파티클 번호로 이동
            currentParticle ++;
        }

        /*        //0. Effect를 재활용할 수 있도록 순서대로
         *       if (currentParticle < poolSize)
                {
                    currentParticle = currentParticle + 1;
                } else
                {
                    currentParticle = 0;
                }
        */
        
        int lastIndex = poolSize - 1;
        //삼항 연산자를 사용해서 1줄로 줄인다.
        currentParticle = (currentParticle >= lastIndex) ? 0 : currentParticle + 1;
        //1. Effect를 특정 위치(Enemy)로 이동시킨다.
        effectPool[currentParticle].transform.position = pos;
        effectPool[currentParticle].Stop();
        effectPool[currentParticle].Play();
    }


}
