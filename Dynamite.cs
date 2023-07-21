using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� : ��򰡿� �ε����� �� ���߽�Ű�� ���� ��ü���� ����������
//1. ���� ���� 
//2. ���� ���� (�о�� ��, ���� ���������� ��)


public class Dynamite : MonoBehaviour
{
    private Rigidbody rb;
    public float expForce = 20.0f; //���� ����
    public float expRange = 15.0f; // ���� ����
    public float liftForce = 10.0f; //�̴� ��
    public GameObject expEffectFactory; //���� ����Ʈ

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
        //���� Effect
        ExplosionEffect();
        //���� :���� ��ü�� ��������
        Explosion();
        //�����
        gameObject.SetActive(false);
        //Destroy(gameObject);


        ResetDynamite();

    }

    public void ResetDynamite()
    {// Rigidbody �ݵ� Reset (Force, Torque)
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //ȸ�� �ʱ�ȭ
        transform.rotation = Quaternion.identity;
        //gravity ��Ȱ��ȭ
        rb.useGravity = false;
        //isKinematic Ȱ��ȭ
        rb.isKinematic = true;
    }

    private void Explosion()
    {
        //1. ���� ���� �ȿ� �ִ� �������� ��ü���� �˻�
        Collider[] hits = Physics.OverlapSphere(transform.position, expRange);
        //2. ���� ���� �ȿ� ��ü���� �ִٸ�
        if (hits != null && hits.Length > 0) 
        {
            for (int i = 0; i < hits.Length; i++) //3. ���� �� �� ��ü�鿡�� ���� ���� ���� (�ݺ�)
            {   //3-1. ��ü���� Rigidbody ������ �ִ��� Ȯ��
                Rigidbody rb = hits[i].GetComponent<Rigidbody>();
                if(rb != null)  //3-2. Rigidbody �� ������ �ִٸ�?
                {
                    rb.AddExplosionForce(expForce, transform.position, expRange, liftForce, ForceMode.Impulse); //(���߼���, ������ġ, ���߹���, ���ζ߰��ϴ� ��, ForceMode)
                    //4. ����! :���� ��ġ(���� ��ġ) �������� ����/����(exp/lift)
                }
            }
        }
        
       
       
        
    }

    void ExplosionEffect()
    {
        //1. ���� Effect ����
        GameObject effect = Instantiate(expEffectFactory);
        //2. ������ Effect �� ���� ��ġ�� �����ٵд�
        effect.transform.position = transform.position;
        //3. ���� effect�� ����Ѵ�.
        effect.GetComponent<ParticleSystem>().Stop();
        effect.GetComponent<ParticleSystem>().Play();

        /*
        expEffectFactory.transform.position = transform.position;
        expEffectFactory.SetActive(true);
    */
        }
}
