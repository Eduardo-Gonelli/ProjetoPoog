using System.Collections;
using UnityEngine;
/* Explicando a lógica
 * Quando o jogador colidir com a área de dano, a rotina de dano contínuo será iniciada.
 * Continuará aplicando dano ao jogador a cada intervalo de tempo definido em damageRateTime.
 * Somente quando sair da área de dano, a rotina será finalizada.
*/
public class DamageArea : MonoBehaviour
{
    public float damage = 10.0f;         // Dano a ser aplicado ao jogador
    public float damageRateTime = 1.0f;  // Tempo entre cada dano aplicado
    private bool canDamage = true;       // Flag para indicar se o jogador pode receber dano

    // Função para detectar a colisão com o jogador
    // Inicia a rotina de dano contínuo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            canDamage = true;
            StartCoroutine(ContinuousDamage(collision.GetComponent<PlayerBase>()));
        }
    }

    // Função para detectar a saída da colisão com o jogador
    // Finaliza a rotina de dano contínuo
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canDamage = false;
            StopCoroutine(ContinuousDamage(collision.GetComponent<PlayerBase>()));
        }
    }
    // Função para aplicar dano contínuo ao jogador
    IEnumerator ContinuousDamage(PlayerBase player)
    {
        while (canDamage)
        {
            player.TakeDamage(damage);            
            yield return new WaitForSeconds(damageRateTime);
        }
    }
}
