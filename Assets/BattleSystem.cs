using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

	private System.Random r = new System.Random();
	private System.Random rv = new System.Random();
	private double p1;
	private double p2;

	// Start is called before the first frame update
	void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		dialogueText.text = "The time to face " + enemyUnit.unitName + " for the last time has come...";
		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
		p1 = r.NextDouble();
		if (p1 > 0.15)
		{
			bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
			enemyHUD.SetHP(enemyUnit.currentHP);
			dialogueText.text = "The spell is on target!";
			yield return new WaitForSeconds(1f);

			if (isDead)
			{
				state = BattleState.WON;
				yield return new WaitForSeconds(1f);
				dialogueText.text = "You won the battle!";
				yield return new WaitForSeconds(3f);
				EndBattle();
			}
			else
			{
				state = BattleState.ENEMYTURN;
				yield return new WaitForSeconds(1f);
				StartCoroutine(EnemyTurn());
			}
		}
		else
		{
			dialogueText.text = "Your Attack missed!";
			yield return new WaitForSeconds(1f);
			state = BattleState.ENEMYTURN;
			yield return new WaitForSeconds(1f);
			StartCoroutine(EnemyTurn());
		}

	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.unitName + " used Avada Kedavra";
		yield return new WaitForSeconds(1f);
		p2 = rv.NextDouble();
		if (p2 > 0.1)
		{
			bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
			playerHUD.SetHP(playerUnit.currentHP);
			if (isDead)
			{
				state = BattleState.LOST;
				yield return new WaitForSeconds(1f);
				dialogueText.text = "You were defeated.";
				yield return new WaitForSeconds(3f);
				EndBattle();
			}
			else
			{
				state = BattleState.PLAYERTURN;
				yield return new WaitForSeconds(1f);
				PlayerTurn();
			}
		}
		else
        {
			dialogueText.text = "You evaded Voldemort's Attack!";
			yield return new WaitForSeconds(1f);
			state = BattleState.PLAYERTURN;
			yield return new WaitForSeconds(1f);
			PlayerTurn();

		}
	}
	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		} else if (state == BattleState.LOST)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(18);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(1f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		else 
			StartCoroutine(PlayerAttack());
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		else
			StartCoroutine(PlayerHeal());
	}
	
}
