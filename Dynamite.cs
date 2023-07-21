using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//역할 : 어딘가에 부딪혔을 때 폭발시키며 주위 물체들을 날려버린다
//1. 폭발 범위 
//2. 폭발 세기 (밀어내는 힘, 위로 날려보내는 힘)


public class Dynamite : MonoBehaviour
{
    private Rigidbody rb;
    public float expForce = 20.0f; //폭발 세기
    public float expRange = 15.0f; // 폭발 범위
    public float liftForce = 10.0f; //미는 힘
    public GameObject expEffectFactory; //폭발 이펙트

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //폭발 Effect
        ExplosionEffect();
        //폭발 :주위 물체를 날려버림
        Explosion();
        //사라짐
        gameObject.SetActive(false);
        //Destroy(gameObject);


        ResetDynamite();

    }

    public void ResetDynamite()
    {// Rigidbody 반동 Reset (Force, Torque)
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //회전 초기화
        transform.rotation = Quaternion.identity;
        //gravity 비활성화
        rb.useGravity = false;
        //isKinematic 활성화
        rb.isKinematic = true;
    }

    private void Explosion()
    {
        //1. 폭발 범위 안에 있는 날려버릴 물체들을 검사
        Collider[] hits = Physics.OverlapSphere(transform.position, expRange);
        //2. 폭발 범위 안에 물체들이 있다면
        if (hits != null && hits.Length > 0) 
        {
            for (int i = 0; i < hits.Length; i++) //3. 범위 안 각 물체들에게 폭발 힘을 전달 (반복)
            {   //3-1. 물체들이 Rigidbody 가지고 있는지 확인
                Rigidbody rb = hits[i].GetComponent<Rigidbody>();
                if(rb != null)  //3-2. Rigidbody 를 가지고 있다면?
                {
                    rb.AddExplosionForce(expForce, transform.position, expRange, liftForce, ForceMode.Impulse); //(폭발세기, 폭발위치, 폭발범위, 위로뜨게하는 힘, ForceMode)
                    //4. 폭발! :폭발 위치(나의 위치) 기준으로 범위/세기(exp/lift)
                }
            }
        }
        
       
       
        
    }

    void ExplosionEffect()
    {
        //1. 폭발 Effect 생성
        GameObject effect = Instantiate(expEffectFactory);
        //2. 생성한 Effect 를 폭발 위치에 가져다둔다
        effect.transform.position = transform.position;
        //3. 폭발 effect를 재생한다.
        effect.GetComponent<ParticleSystem>().Stop();
        effect.GetComponent<ParticleSystem>().Play();

        /*
        expEffectFactory.transform.position = transform.position;
        expEffectFactory.SetActive(true);
    */
        }
}
