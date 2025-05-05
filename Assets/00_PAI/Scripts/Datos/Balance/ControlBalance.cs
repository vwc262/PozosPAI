using UnityEngine;
using UnityEngine.UI;

public class ControlBalance : MonoBehaviour
{
    public float Balance;

    public Text BalanceLabel;
    public Text BalanceValue;

    public void SetLabel(string _text)
    {
        if (BalanceLabel != null)
            BalanceLabel.text = _text;
    }

    public void SetValue(string _text)
    {
        if (BalanceValue != null)
            BalanceValue.text = _text;
    }
}
