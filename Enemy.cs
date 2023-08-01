using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

//���� : Tower�� ���� �ٰ����� ������ ���� �����Ѵ�
//Tower(Target), NavMesh Agent, Animator, �̵��ӵ�, ȸ���ӵ�, HP(Get/Set Property), FSM : Idle / Move / Attack / Damaged / Die

public class Enemy : MonoBehaviour, IDamaged
{
    public enum State { Idle, Move, Attack, Damaged, Die } //enum�� 
    public State m_state = State.Idle;

    //My Transform (����ȭ- ĳ��)
    private Transform myTrans;

    public Transform target; //Tower(Target)
    public LayerMask targetLayer; //�浹 ���̾� �˻�
    public NavMeshAgent agent;
    public Animator anim;
    public float speed = 3.5f; //�̵� �ӵ�
    public float rotSpeed = 120.0f; //ȸ�� �ӵ�
    public float attackRange = 3.5f; //���� �����Ÿ�

    public int currentHp; //HP
    public int maxHp = 3;

    //public int HP { get => currentHp; set => currentHp = value;
    public int HP
    {
        get { return currentHp; }
        set
        {
            currentHp = value;


            //2-a. HP�� ���� �����ִٸ�, �ǰ� �ִϸ��̼� ���
            if (currentHp > 0)
            {
                ChangeState(State.Damaged);

                StartCoroutine(Stun()); // 1.5�� ���� ���� ��, Idle ���·� ��ȯ
            }
            //2-b.  HP�� 0�� �Ǹ� Enemy �� Die} //���� �ҷ��ͼ� �����ϱ�
            else
            {
                StopAllCoroutines(); // ���� ����ִ� Stun �ڷ�ƾ ��� ����
                ChangeState(State.Die);
                print("����");
            }
        }

    }
    private void OnEnable()
    {
        //OnEnable�� Start���� ���� ����ǹǷ� ���� ����ÿ� agent ���� ���� Error�� ���� ���� ���´�.
        if (agent != null && target != null) 
        { 
            agent.ResetPath(); //agent Target���� �� �� �ֵ��� ��θ� ����
            agent.SetDestination(target.position); //Agent�� ��ǥ(Tower)�� �̵��� �� �ֵ��� ����
        }
    }
       

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Nav Agent �ʱ�ȭ (�̵�/ȸ��)
        myTrans = GetComponent<Transform>(); //�� Transform Chaching

        currentHp = maxHp; //�ʱ�ȭ
        agent.speed = speed; //Agent�� �̵��ӵ� �ʱ�ȭ
        agent.angularSpeed = rotSpeed; //Agent�� ȸ���ӵ� �ʱ�ȭ
        agent.SetDestination(target.position); //Agent�� ��ǥ(Tower)�� �̵��� �� �ֵ��� ����
    }
    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case State.Idle: Idle(); break;
            case State.Move: Move(); break;
            case State.Attack: Attack(); break;
            case State.Damaged: Damaged(); break;
            case State.Die: Die(); break;
        }
    }

    private void Idle() { ChangeState(State.Move); }
    private void Move()
    {
        //Tower�� ������ ���� �ε����� (�����ϸ�)
        bool reached = IsHitTarget();
        //�̵��ϴ� Tower�� �ε����� ����
        if (reached) { ChangeState(State.Attack); }
    }
    private void Attack() { }
    private void Damaged()
    {

    }

    public void DamagedProcess(int damage)
    {
        //1. damage ��ŭ Enemy HP�� ���ҽ�Ų��
        HP -= damage;

    }
    private void Die() { }

    public void ChangeState(State nextState)
    {
        /* Error!
        if (nextState == State.Attack || nextState == State.Die || nextState == State.Damaged) { agent.isStopped = true; }
        else if (nextState == State.Move) { agent.isStopped = false; }

        anim.SetInteger("State", (int)nextState); //�ִϸ��̼� ����
        m_state = nextState; //���� Enemy ����(m_state)�� ���ϴ� ����(nextState)�� ����

        /*
         * if (nextState == State.Move) { anim.SetInteger("State", 1); }
        else if (nextState == State.Attack) { anim.SetInteger("State", 2); }
        */

        //B. SetTrigger ( Damaged / Die )
        if (nextState == State.Damaged || nextState == State.Die)
        {
            //Enemy ����
            agent.isStopped = true;
            //Animator ���� ��ȯ
            anim.SetTrigger(nextState.ToString());

            if (nextState == State.Die)
            {
               
                //���� �ð� �� �ı� (����Ʈ�� �߰�)
                //Destroy(gameObject, 3.0f);
                StartCoroutine(Death());
            }
        }

        //A. SetInteger (Idle / Move / Attack)
        else
        {
            //Enemy �ٽ� ������ (Move)
            if (nextState == State.Move) { agent.isStopped = false; }
            //Enemy ���� ( Idle, Attack)
            else { agent.isStopped = true; }
            //Animator ���� ��ȯ
            anim.SetInteger("State", (int)nextState);
        }

        m_state = nextState; //���� Enemy ����(m_state)�� ���ϴ� ����(nextState)�� ����
    }

    //Target(Tower)�� �ε������� üũ
    private bool IsHitTarget()
    {
        //Tower ���̾ �浹 �˻�
        //1. OverLapSphere Ȱ���ؼ� �� �����Ÿ� �� Target �浹 �˻�
        Collider[] hits = Physics.OverlapSphere(myTrans.position, attackRange, targetLayer);

        //2-1 .���� �浹�ߴٸ�? true
        if (hits != null && hits.Length > 0) { return true; }
        //2-2. �浹 ���ߴٸ�? false
        else { return false; }
    }

    //�ǰ� ���ϸ� �����ð� ���� ����
    private IEnumerator Stun()
    {
        // ���� �ð����� ���� ... ���Ŀ� Idle ���·� ��ȯ
        // 1. �ð��� ���
        yield return new WaitForSeconds(0.5f);
        // 2. ���� �ð� ���Ŀ� Idle ���·� 
        ChangeState(State.Idle);
        print("���� ��");
    }

    //Enemy�� ���� ���, ���ο� ���� ���� ����Ǵ� �ڷ�ƾ
    private IEnumerator Death()
    {
        // Gun / Dynamite ���⿡ ���ؼ� Ÿ���� ���� �ʵ��� CC ��Ȱ��ȭ
        GetComponent<CharacterController>().enabled = false;

        // 1. 3�� ���
        yield return new WaitForSeconds(3f);

        //[����] ��Ȱ��ȭ ���¿��� Agent���� Go/Stop ����� ���� �� Error
        //[�ذ�] Ȱ��ȭ ���¿��� �ʱ갪 �������� �� ��Ȱ��ȭ ���·� �����Ѵ�.
        //Enemy Deactive �߰�
        EnemyCreator.Instance.DeactiveEnemy(gameObject);
    }

    //Enemy State Reset ---

    public void ResetState()
    {
        currentHp = maxHp;  //HP ȸ��
        GetComponent<CharacterController>().enabled = true; //CC Ȱ��ȭ
        ChangeState(State.Idle);    //State -> Idle�� ����
    }
}
