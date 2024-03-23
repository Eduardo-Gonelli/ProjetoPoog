using System.Collections;
using UnityEngine;
/* Explicando a l�gica
 * Quando o jogador colidir com a �rea de dano, a rotina de dano cont�nuo ser� iniciada.
 * Continuar� aplicando dano ao jogador a cada intervalo de tempo definido em damageRateTime.
 * Somente quando sair da �rea de dano, a rotina ser� finalizada.
*/
public class DamageArea : MonoBehaviour
{
    public float damage = 10.0f;         // Dano a ser aplicado ao jogador
    public float damageRateTime = 1.0f;  // Tempo entre cada dano aplicado
    private bool canDamage = true;       // Flag para indicar se o jogador pode receber dano

    // Fun��o para detectar a colis�o com o jogador
    // Inicia a rotina de dano cont�nuo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            canDamage = true;
            StartCoroutine(ContinuousDamage(collision.GetComponent<PlayerBase>()));
        }
    }

    // Fun��o para detectar a sa�da da colis�o com o jogador
    // Finaliza a rotina de dano cont�nuo
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canDamage = false;
            StopCoroutine(ContinuousDamage(collision.GetComponent<PlayerBase>()));
        }
    }
    // Fun��o para aplicar dano cont�nuo ao jogador
    IEnumerator ContinuousDamage(PlayerBase player)
    {
        while (canDamage)
        {
            player.TakeDamage(damage);            
            yield return new WaitForSeconds(damageRateTime);
        }
    }
}
