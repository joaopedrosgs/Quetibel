public class Node<T>
{
    public T Elemento { get; set; }
    public Node<T> Anterior { get; set; }
}

public class PilhaEncadeada<T>
{
    private Node<T> _topo;
    private int _tamanho;

    public int GetTamanho()
    {
        return _tamanho;
    }

    public PilhaEncadeada() // Construtor
    {
        _tamanho = -1; 
        _topo = null;
    }


    public void Inserir(ref T elemento) // Referencia
    {
        Node<T> node = new Node<T>
        {
            Elemento = elemento,
            Anterior = null
        };
        if (_topo == null)
        {
            _topo = node;
        }
        else
        {
            node.Anterior = _topo;
            _topo = node;
        }
        _tamanho++;
    }

    public bool Retirar(out T elemento) // Referencia que garante que não retornará nulo
    {
        if (Vazia())
        {
            elemento = default(T);
            return false;
        }

        elemento = _topo.Elemento;
        _topo = _topo.Anterior;
        _tamanho--;
        return elemento != null;
    }

    public bool Vazia()
    {
        return _tamanho == -1;
    }

    public bool Encontrar(int i, out T elemento)
    {
        elemento = default(T);

        if (Vazia() || i > _tamanho) return false;

        PilhaEncadeada<T> temp = new PilhaEncadeada<T>();
        T ele = default(T);
        while (_tamanho >= i)
        {
            if (!Retirar(out ele))
                return false;

            temp.Inserir(ref ele);
        }

        elemento = ele;

        while (temp.Retirar(out ele))
        {
            Inserir(ref ele);
        }
        return true;
    }
}