using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // Use this for initialization
    public GameObject Monster;

    public GameObject EvilAnimation;

    void Start()
    {
        InvokeRepeating("RandomSummon", 0, 12);
    }

    // Update is called once per frame

    void RandomSummon()
    {
        var randPos = GameController.Player.transform.position;
        randPos.x += Random.Range(3, 10);
        randPos.z += Random.Range(3, 10);
        randPos.y = Terrain.activeTerrain.SampleHeight(randPos);
        Destroy(Instantiate(EvilAnimation, randPos, Quaternion.identity), 5);
        Instantiate(Monster, randPos, Quaternion.identity);
    }
}