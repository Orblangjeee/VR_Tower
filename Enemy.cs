using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

//역할 : Tower를 향해 다가가며 가까이 가면 공격한다
//Tower(Target), NavMesh Agent, Animator, 이동속도, 회전속도, HP(Get/Set Property), FSM : Idle / Move / Attack / Damaged / Die

public class Enemy : MonoBehaviour, IDamaged
{
    public enum State { Idle, Move, Attack, Damaged, Die } //enum은 
    public State m_state = State.Idle;

    //My Transform (최적화- 캐싱)
    private Transform myTrans;

    public Transform target; //Tower(Target)
    public LayerMask targetLayer; //충돌 레이어 검사
    public NavMeshAgent agent;
    public Animator anim;
    public float speed = 3.5f; //이동 속도
    public float rotSpeed = 120.0f; //회전 속도
    public float attackRange = 3.5f; //공격 사정거리

    public int currentHp; //HP
    public int maxHp = 3;

    //public int HP { get => currentHp; set => currentHp = value;
    public int HP
    {
        get { return currentHp; }
        set
        {
            currentHp = value;


            //2-a. HP가 아직 남아있다면, 피격 애니메이션 재생
            if (currentHp > 0)
            {
                ChangeState(State.Damaged);

                StartCoroutine(Stun()); // 1.5초 동안 기절 후, Idle 상태로 전환
            }
            //2-b.  HP가 0이 되면 Enemy 는 Die} //값을 불러와서 갱신하기
            else
            {
                StopAllCoroutines(); // 현재 살아있는 Stun 코루틴 모두 종료
                ChangeState(State.Die);
                print("죽음");
            }
        }

    }
    private void OnEnable()
    {
        //OnEnable은 Start보다 먼저 실행되므로 최초 실행시에 agent 값이 없어 Error가 나는 것을 막는다.
        if (agent != null && target != null) 
        { 
            agent.ResetPath(); //agent Target으로 갈 수 있도록 경로를 재계산
            agent.SetDestination(target.position); //Agent가 목표(Tower)로 이동할 수 있도록 지정
        }
    }
       

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Nav Agent 초기화 (이동/회전)
        myTrans = GetComponent<Transform>(); //내 Transform Chaching

        currentHp = maxHp; //초기화
        agent.speed = speed; //Agent의 이동속도 초기화
        agent.angularSpeed = rotSpeed; //Agent의 회전속도 초기화
        agent.SetDestination(target.position); //Agent가 목표(Tower)로 이동할 수 있도록 지정
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
        //Tower랑 가까이 가서 부딪히면 (도착하면)
        bool reached = IsHitTarget();
        //이동하다 Tower랑 부딪히면 공격
        if (reached) { ChangeState(State.Attack); }
    }
    private void Attack() { }
    private void Damaged()
    {

    }

    public void DamagedProcess(int damage)
    {
        //1. damage 만큼 Enemy HP를 감소시킨다
        HP -= damage;

    }
    private void Die() { }

    public void ChangeState(State nextState)
    {
        /* Error!
        if (nextState == State.Attack || nextState == State.Die || nextState == State.Damaged) { agent.isStopped = true; }
        else if (nextState == State.Move) { agent.isStopped = false; }

        anim.SetInteger("State", (int)nextState); //애니메이션 변경
        m_state = nextState; //현재 Enemy 상태(m_state)를 원하는 상태(nextState)로 변경

        /*
         * if (nextState == State.Move) { anim.SetInteger("State", 1); }
        else if (nextState == State.Attack) { anim.SetInteger("State", 2); }
        */

        //B. SetTrigger ( Damaged / Die )
        if (nextState == State.Damaged || nextState == State.Die)
        {
            //Enemy 멈춤
            agent.isStopped = true;
            //Animator 상태 전환
            anim.SetTrigger(nextState.ToString());

            if (nextState == State.Die)
            {
               
                //일정 시간 후 파괴 (리스트에 추가)
                //Destroy(gameObject, 3.0f);
                StartCoroutine(Death());
            }
        }

        //A. SetInteger (Idle / Move / Attack)
        else
        {
            //Enemy 다시 움직임 (Move)
            if (nextState == State.Move) { agent.isStopped = false; }
            //Enemy 멈춤 ( Idle, Attack)
            else { agent.isStopped = true; }
            //Animator 상태 전환
            anim.SetInteger("State", (int)nextState);
        }

        m_state = nextState; //현재 Enemy 상태(m_state)를 원하는 상태(nextState)로 변경
    }

    //Target(Tower)과 부딪혔는지 체크
    private bool IsHitTarget()
    {
        //Tower 레이어만 충돌 검사
        //1. OverLapSphere 활용해서 내 사정거리 안 Target 충돌 검사
        Collider[] hits = Physics.OverlapSphere(myTrans.position, attackRange, targetLayer);

        //2-1 .만일 충돌했다면? true
        if (hits != null && hits.Length > 0) { return true; }
        //2-2. 충돌 안했다면? false
        else { return false; }
    }

    //피격 당하면 일정시간 동안 스턴
    private IEnumerator Stun()
    {
        // 일정 시간동안 스턴 ... 이후에 Idle 상태로 전환
        // 1. 시간이 경과
        yield return new WaitForSeconds(0.5f);
        // 2. 스턴 시간 이후에 Idle 상태로 
        ChangeState(State.Idle);
        print("스턴 끝");
    }

    //Enemy가 죽은 경우, 새로운 삶을 위해 실행되는 코루틴
    private IEnumerator Death()
    {
        // Gun / Dynamite 무기에 의해서 타격을 받지 않도록 CC 비활성화
        GetComponent<CharacterController>().enabled = false;

        // 1. 3초 대기
        yield return new WaitForSeconds(3f);

        //[문제] 비활성화 상태에서 Agent에게 Go/Stop 명령을 내릴 시 Error
        //[해결] 활성화 상태에서 초깃값 세팅해준 뒤 비활성화 상태로 변경한다.
        //Enemy Deactive 추가
        EnemyCreator.Instance.DeactiveEnemy(gameObject);
    }

    //Enemy State Reset ---

    public void ResetState()
    {
        currentHp = maxHp;  //HP 회복
        GetComponent<CharacterController>().enabled = true; //CC 활성화
        ChangeState(State.Idle);    //State -> Idle로 변경
    }
}
