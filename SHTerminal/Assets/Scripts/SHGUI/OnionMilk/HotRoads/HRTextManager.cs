using UnityEngine;
using System.Collections;

public class HRTextManager
{
	//manu
	public	string			pressText		= "PRESS ENTER TO START";
	public	string			unlockText		= "UNLOCK COST: 100";
	public	string			bestText		= "PERSONAL BEST: 0";
	public	string			moneyText		= "YOUR MONEY: 0";
	
	//end game
	public	string			restartText		= "PRESS ENTER TO RESTART";
	public	string			endText			= "PRESS ESCAPE TO QUIT";
	public	string			endPointsText	= "SCORE ";
	public	string			endBestText		= "NEW BEST SCORE";
	public	bool			mryga			= true;
	public	float			mrygaTimer		= 0.3f;

	//zapis
	public	bool[]			lockTab			= new bool[] {false, true, true, true , true}; //zablokowane / odblokowane postacie
	public	int				piniadze		= 100; //ilość pinieniędzy
	public	int				best			= 0; //najlepszy osobisty wynik
	public	int				score			= 0; //obecny wynik podczas rozgrywki
	public	int				oldBest			= 0; //nallepszy wynik jaki był w poprzedniej rundzie
	public	string			gameMoneyText	= "0$";
	public	string			gameScoreText	= "Score ";
	public	string			gameBestText	= "Best ";

	//kasa
	public	bool			onMoney			= true;
	public	float			timeShowMoney	= 0.3f;

	//to co dzieje się podczas update gdy gra jest odpalona, a ty przegrales
	public void UpdateInMenu(int yourLook)
	{
		bestText		= "PERSONAL BEST: " + best;
		moneyText		= "YOUR MONEY: " + piniadze + "$";
		
		if(lockTab[yourLook])
		{
			if(piniadze >= 100)
			{
				pressText		= "PRESS ENTER TO BUY";
			}
			else
			{
				pressText		= "";
			}
		}
		else
		{
			pressText		= "PRESS ENTER TO START";
		}
	}

	public void UpdateMoneyVisible()
	{
		timeShowMoney -= Time.unscaledDeltaTime; //mryganie napisu z kasą
		if(timeShowMoney <= 0f)
		{
			if(onMoney)
			{
				onMoney = false;
				timeShowMoney = 0.3f;
			}
			else
			{
				onMoney = true;
				timeShowMoney = 0.6f;
			}
		}
	}

	public void UpdateLoseInGame()
	{
		if(best > oldBest) oldBest = best;
		
		mrygaTimer -= Time.unscaledDeltaTime;
		if(mrygaTimer <= 0)
		{
			if(mryga)
			{
				mryga = false;
				mrygaTimer = 0.3f;
			}
			else
			{
				mryga = true;
				mrygaTimer = 0.5f;
			}
		}
	}

	public void UpdateBestScore(int posY)
	{
		if(score < (posY / 3)) score = posY / 3;
		if(best < score) best = score;
	}

}
