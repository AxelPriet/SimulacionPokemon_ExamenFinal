// EnemyAI.cs
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public PokemonStatus targetPlayer; // assign PlayerPokemon.PokemonStatus
    public BattleMessageBox messageBox;

    // Apply a random status (not None)
    public StatusCondition ApplyRandomStatus()
    {
        StatusCondition random = (StatusCondition)Random.Range(1, System.Enum.GetValues(typeof(StatusCondition)).Length);
        targetPlayer.ApplyStatus(random);

        messageBox.ShowMessage($"{targetPlayer.name} ha sido {random}!");
        return random;
    }
}

