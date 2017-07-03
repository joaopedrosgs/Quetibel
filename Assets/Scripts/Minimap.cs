using UnityEngine;

public class Minimap : MonoBehaviour
{

    // Use this for initialization

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 posicao = GameController.Player.transform.position;
        posicao.y = 40;
        transform.position = posicao;
    }
}