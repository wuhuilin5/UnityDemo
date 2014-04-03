using UnityEngine;
using System.Collections;

public class LoginView : MonoBehaviour {
	
	public UILabel txtName;
	public UILabel txtPwd;
	public UIImageButton btnLogin;
	
	// Use this for initialization
	void Start () {
		registerEvent();
	}
	
	private void registerEvent()
	{
		UIEventListener.Get( btnLogin.gameObject ).onClick += onBtnLoginClick;
	}
	
	void onBtnLoginClick( GameObject go ){
		Debug.Log( "onBtnLoginClick " + go.name );
	}

	void OnClick(){
		Debug.Log( "onClick..." );
	}
}
