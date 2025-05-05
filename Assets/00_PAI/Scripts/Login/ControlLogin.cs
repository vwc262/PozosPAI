using UnityEngine;

public class ControlLogin : Singleton<ControlLogin>
{
    public GameObject panelLogin;
    public BoyLoginPassword login;
    
    public void ActivateLoginPanel()
    {
        if (panelLogin != null)
            panelLogin.SetActive(true);
    }

    public void SetEnableKeyboardInput(bool _enable)
    {
        if (FlyCamera._singletonExists)
            FlyCamera.singleton.enableInputKeyboard = _enable;
    }

    public void CloseLoginPanel()
    {
        if (login != null)
            login.SendEventFSM("close");
    }
}
