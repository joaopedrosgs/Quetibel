using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SaveCat : MonoBehaviour
{

    public GameObject Portal;
    public GameObject TeleportedAnimation;
    public Text Score;


    // Use this for initialization


    // Update is called once per frame
    void Start()
    {
        Score = GameObject.Find("Score").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            MoverTodos();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Teleported")) return;
        Destroy(Instantiate(TeleportedAnimation, other.gameObject.transform.position, Quaternion.identity), 5);
        Destroy(other.gameObject);
    }

    void MoverTodos()
    {
        var corpo = GameController.Player.GetComponent<Corpo>();
        if (!corpo.Any())
            return;

        Transform tr;
        float multiplier = 0;

        while (corpo.Retirar(out tr))
        {
            tr.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            tr.GetComponent<NavMeshAgent>().destination = Portal.transform.position;
            tr.tag = "Teleported";
            multiplier++;
            multiplier *= 1.02f;
            GameController.SavedCats++;
        }

        StartCoroutine(GameController.IncrementScore(multiplier * 50));
    }
}