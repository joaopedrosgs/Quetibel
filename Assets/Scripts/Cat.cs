using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    Animator _anim;
    private NavMeshAgent _nav;
    public Texture[] Textures;
    public GameObject DeathAnimation;
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    private bool _isDead;


    void Awake()
    {

        // Hp
        MaxHp = 3;
        Hp = MaxHp;

        //Tamanho Aleatorio
        float size = Random.Range(0.6f, 1);
        transform.localScale = new Vector3(size, size, size);

        //Componentes
        _anim = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _nav.stoppingDistance /= size;

        //Textura aleatoria
        var child = transform.Find("Cat_Lite").gameObject;
        child.GetComponent<Renderer>().material.mainTexture = Textures[Random.Range(0, Textures.Length)];

        GameController.Cats++;
    }

    // Use this for initialization
    void Start()
    {
        if (_anim == null || _nav == null)
            DestroyImmediate(gameObject);


        InvokeRepeating("RandomWalk", Random.Range(1, 5), Random.Range(1, 7));

    }
    void OnBecameInvisible() {
        Destroy(gameObject, 30);
    }
    void Update()
    {
        if (Hp <= 0)
            _isDead = true;
    }


    // Update is called once per frame

    void LateUpdate()
    {
        if (_isDead)
        {
            GameController.DeadCats++;
            enabled = false;
            _nav.isStopped = true;
            Destroy(Instantiate(DeathAnimation, transform.position, Quaternion.identity), 5);
            Destroy(gameObject, 1);
        }
        _anim.SetFloat("Velocidade", _nav.velocity.magnitude);
    }

    void OnDestroy()
    {
        GameController.Cats--;
    }


    void RandomWalk()
    {
        if (!CompareTag("CanBeSaved"))
            return;
        Vector3 randPos = transform.position;
        randPos.x += Random.Range(0, 6);
        randPos.z += Random.Range(0, 6);
        randPos.y = Terrain.activeTerrain.SampleHeight(randPos);
        if (!_nav.SetDestination(randPos))
            Destroy(gameObject);
    }

}