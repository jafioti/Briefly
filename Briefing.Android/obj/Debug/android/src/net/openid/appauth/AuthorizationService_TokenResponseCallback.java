package net.openid.appauth;


public class AuthorizationService_TokenResponseCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		net.openid.appauth.AuthorizationService.TokenResponseCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRequestCompleted:(Lnet/openid/appauth/TokenResponse;Lnet/openid/appauth/AuthorizationException;)V:GetOnTokenRequestCompleted_Lnet_openid_appauth_TokenResponse_Lnet_openid_appauth_AuthorizationException_Handler:OpenId.AppAuth.AuthorizationService/ITokenResponseCallbackInvoker, OpenId.AppAuth.Android\n" +
			"";
		mono.android.Runtime.register ("OpenId.AppAuth.AuthorizationService+TokenResponseCallback, OpenId.AppAuth.Android", AuthorizationService_TokenResponseCallback.class, __md_methods);
	}


	public AuthorizationService_TokenResponseCallback ()
	{
		super ();
		if (getClass () == AuthorizationService_TokenResponseCallback.class)
			mono.android.TypeManager.Activate ("OpenId.AppAuth.AuthorizationService+TokenResponseCallback, OpenId.AppAuth.Android", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRequestCompleted (net.openid.appauth.TokenResponse p0, net.openid.appauth.AuthorizationException p1)
	{
		n_onTokenRequestCompleted (p0, p1);
	}

	private native void n_onTokenRequestCompleted (net.openid.appauth.TokenResponse p0, net.openid.appauth.AuthorizationException p1);

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
