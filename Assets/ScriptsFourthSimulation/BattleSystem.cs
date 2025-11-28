using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public PokemonStatus playerPokemon;
    public PokemonStatus enemyPokemon;

    public BattleMenu battleMenu;
    public BagMenu bagMenu;
    public BattleMessageBox messageBox;
    public EnemyAI enemyAI;

    public enum State { EnemyTurn, ShowMessage, PlayerMenu, PlayerBag }
    public State state = State.EnemyTurn;

  
    void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnUIMove += BattleInputBroker.OnUIMove;
            InputManager.Instance.OnUIInteract += BattleInputBroker.OnUIInteract;
            InputManager.Instance.OnUICancel += BattleInputBroker.OnUICancel;
        }
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnUIMove -= BattleInputBroker.OnUIMove;
            InputManager.Instance.OnUIInteract -= BattleInputBroker.OnUIInteract;
            InputManager.Instance.OnUICancel -= BattleInputBroker.OnUICancel;
        }
    }

    void Start()
    {
        battleMenu.Close();
        bagMenu.Close();
        messageBox.messageText.gameObject.SetActive(false);
        StartCoroutine(EnemyFirstRoutine());

    }

    IEnumerator EnemyFirstRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        enemyAI.ApplyRandomStatus();
        yield return new WaitForSeconds(1.1f);
        EnterPlayerTurn();
    }

    void EnterPlayerTurn()
    {
        state = State.PlayerMenu;
        battleMenu.Open();
    }

    void Update()
    {
        BattleInputBroker.UpdateFallback(); // opcional teclado

        switch (state)
        {
            case State.PlayerMenu:
                battleMenu.Simulate();
                break;
            case State.PlayerBag:
                bagMenu.Simulate();
                break;
        }

        BattleInputBroker.ClearPulse();
    }

    public void OpenBag()
    {
        battleMenu.Close();
        bagMenu.Open();
        state = State.PlayerBag;
    }

    public void ReturnToBattleMenu()
    {
        bagMenu.Close();
        battleMenu.Open();
        state = State.PlayerMenu;
    }

    public void OnItemUsed(StatusCondition cures)
    {
        if (playerPokemon.currentStatus == cures)
        {
            playerPokemon.CureStatus();
            messageBox.ShowMessage($"¡{playerPokemon.name} está curado!");
        }
        else
        {
            messageBox.ShowMessage("No surte efecto...");
        }

        StartCoroutine(PostItemRoutine());
    }

    IEnumerator PostItemRoutine()
    {
        bagMenu.Close();
        battleMenu.Close();
        state = State.ShowMessage;

        yield return new WaitForSeconds(messageBox.displayTime + 0.05f);

        // ✅ Turno del enemigo
        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        enemyAI.ApplyRandomStatus();
        yield return new WaitForSeconds(1.0f);

        EnterPlayerTurn();
    }



}






