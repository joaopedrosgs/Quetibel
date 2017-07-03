using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    public enum State
    {
        Idle,
        Following,
        Attacking,
        Rage
    }

    //State
    public State _state;

    // Components
    Animator _anim;
    NavMeshAgent _nav;


    //Timing and Numbers
    readonly float _attackTime = 2.8f;
    DateTime _lastAttack;
    private const float Delay = 2f;
    private const float Range = 4f;

    //Objects
    private GameObject _attackingTo;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        _state = State.Idle;
        _lastAttack = DateTime.Now;
        if (_anim == null || _nav == null)
            DestroyImmediate(gameObject);
    }

    public void FindNearest()
    {
        if (_lastAttack.AddSeconds(Delay) > DateTime.Now) return;
        var elements = FindObjectsOfType<Cat>();
        if (elements.Length < 1)
            return;
        elements = elements.OrderBy(cat => Vector3.Distance(transform.position, cat.transform.position)).ToArray();
        _attackingTo = elements[0].gameObject;
    }

    bool SetToNearest()
    {
        if (_attackingTo == null) return false;
        return _nav.SetDestination(_attackingTo.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Velocidade", _nav.velocity.magnitude);

        if ( DateTime.Now <= _lastAttack.AddSeconds(Delay)) return;
        switch (_state)
        {
            case State.Idle:
            {
                FindNearest();
                SetToNearest();
                _state = State.Following;
            }
                break;
            case State.Following:
            {
                if (!SetToNearest())
                {
                    _state = State.Idle;
                    return;
                }
                if(Vector3.Distance(transform.position, _attackingTo.transform.position) <= Range)
                    _state = State.Attacking;
            }
                break;
            case State.Attacking:
            {


                if (_attackingTo == null)
                {
                    _state = State.Idle;
                    return;
                }
                if (Vector3.Distance(transform.position, _attackingTo.transform.position) > Range+1)
                {
                    _state = State.Following;
                    return;
                }

                    _anim.SetTrigger("Attacking");
                    _lastAttack = DateTime.Now.AddSeconds(_attackTime);
                    transform.LookAt(_attackingTo.transform.position);


            }
                break;

            case State.Rage:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


    }


    private void Attack()
    {

        if (!_attackingTo || Vector3.Distance(_attackingTo.transform.position, gameObject.transform.position) >
           Range+2) return;

        GetComponent<AudioSource>().Play();
        _attackingTo.GetComponent<Cat>().Hp--;
    }
}