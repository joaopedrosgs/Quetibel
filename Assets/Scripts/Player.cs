using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent _nav;
    private Animator _anim;
    private RaycastHit _hit;


    // Use this for initialization
    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Velocidade", _nav.velocity.magnitude);
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, 100))
            {
                _nav.SetDestination(_hit.point);
            }
        }
    }

}