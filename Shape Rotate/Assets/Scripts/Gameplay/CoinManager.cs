using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinManager
{
    public static void SetCoinAmount(int _amount)
    {
        PlayerPrefs.SetInt("CoinAmount", _amount);
    }

    public static void AddCoins(int _amount)
    {
        PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + _amount);
    }

    public static bool SpendCoins(int _amount)
    {
        if (GetCoinAmount() < _amount)
            return false;

        AddCoins(-_amount);

        return true;
    }

    public static int GetCoinAmount()
    {
        return PlayerPrefs.GetInt("CoinAmount", 0);
    }
}
