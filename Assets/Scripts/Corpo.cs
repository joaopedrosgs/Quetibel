using UnityEngine;
using UnityEngine.AI;

public class Corpo : MonoBehaviour
{
    public int TamanhoMaximo;
    public PilhaEncadeada<Transform> Segmentos;
    Animator _anim;

    public bool Any()
    {
        return Segmentos.GetTamanho() > 0;
    }



    private void Awake()
    {
        Segmentos = new PilhaEncadeada<Transform>();
        var ts = transform;
        Crescer(ref ts);
    }

    void OnTriggerEnter(Collider gatinho)
    {
        if (Segmentos.GetTamanho() >= TamanhoMaximo)
            return;
        if (!gatinho.gameObject.CompareTag("CanBeSaved")) return;
        Transform gatinhoTransform = gatinho.gameObject.transform;
        gatinho.gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gatinho.tag = "Saved";
        Crescer(ref gatinhoTransform);
    }

    private void Crescer(ref Transform gatinhoTransform)
    {
        Segmentos.Inserir(ref gatinhoTransform);
    }



    private void Update()
    {
        for (int a = 1; a <= Segmentos.GetTamanho(); a++)
        {
            Transform elemento;
            Segmentos.Encontrar(a, out elemento);
            Transform elementoDaFrente;
            Segmentos.Encontrar(a - 1, out elementoDaFrente);

            if (!elementoDaFrente || !elemento)
                BreakStack(a);

            elemento.GetComponent<NavMeshAgent>().destination = elementoDaFrente.transform.position;
        }
    }

    public bool Retirar(out Transform a)
    {
        a = null;
        if (Segmentos.GetTamanho() == 0) return false;
        Segmentos.Retirar(out a);
        a.tag = "CanBeSaved";
        return true;
    }


    void BreakStack(int ele)
    {
        Debug.Log("Quebrou");
        for (int a = 0; a < ele; a++)
        {
            Transform removed;
            Retirar(out removed);
        }
    }
}