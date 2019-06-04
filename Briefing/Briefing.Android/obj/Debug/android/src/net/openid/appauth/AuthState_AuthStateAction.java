package net.openid.appauth;


public class AuthState_AuthStateAction
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		net.openid.appauth.AuthState.AuthStateAction
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_execute:(Ljava/lang/String;Ljava/lang/String;Lnet/openid/appauth/AuthorizationException;)V:GetExecute_Ljava_lang_String_Ljava_lang_String_Lnet_openid_appauth_AuthorizationException_Handler:OpenId.AppAuth.AuthState/IAuthStateActionInvoker, OpenId.AppAuth.Android\n" +
			"";
		mono.android.Runtime.register ("OpenId.AppAuth.AuthState+AuthStateAction, OpenId.AppAuth.Android", AuthState_AuthStateAction.class, __md_methods);
	}


	public AuthState_AuthStateAction ()
	{
		super ();
		if (getClass () == AuthState_AuthStateAction.class)
			mono.android.TypeManager.Activate ("OpenId.AppAuth.AuthState+AuthStateAction, OpenId.AppAuth.Android", "", this, new java.lang.Object[] {  });
	}


	public void execute (java.lang.String p0, java.lang.String p1, net.openid.appauth.AuthorizationException p2)
	{
		n_execute (p0, p1, p2);
	}

	private native void n_execute (java.lang.String p0, java.lang.String p1, net.openid.appauth.AuthorizationException p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
