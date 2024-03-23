using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    
    public Transform posicaoFinal;
    public float velocidade = 3.0f;
    // para quando for reproduzir o som da porta
    // public AudioClip somPorta;    

    public void Destrancar()
    {
        StartCoroutine(AbrirPorta());
    }

    IEnumerator AbrirPorta()
    {
        while(Vector3.Distance(transform.position, posicaoFinal.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicaoFinal.position, velocidade * Time.deltaTime);
            yield return null;
        }
    }
}
