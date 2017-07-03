using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    public GameObject ModeloGato;
    public GameObject CentroMontanha;

    // Use this for initialization
    void LateUpdate()
    {
        if (GameController.Cats < 50)
        {
            Spawn();
        }
    }


    void Spawn()
    {
        Vector3 localDeSpawn;
        do
        {
            localDeSpawn = PosicaoForaDaCamera(0, 30);
        } while (Vector3.Distance(localDeSpawn, CentroMontanha.transform.position) < 100);

        Instantiate(ModeloGato, localDeSpawn, Quaternion.identity).transform.Rotate(0, Random.Range(0, 360), 0);
    }


    public static Vector3 PosicaoForaDaCamera(int min, int max)
    {
        Vector3 randPos =
            Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), 0.5f, Random.Range(min, max)));
        randPos.y = Terrain.activeTerrain.SampleHeight(randPos);
        return randPos;
    }
}