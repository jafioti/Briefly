package net.openid.appauth;


public class AuthorizationService_RegistrationResponseCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		net.openid.appauth.AuthorizationService.RegistrationResponseCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRegistrationRequestCompleted:(Lnet/openid/appauth/RegistrationResponse;Lnet/openid/appauth/AuthorizationException;)V:GetOnRegistrationRequestCompleted_Lnet_openid_appauth_RegistrationResponse_Lnet_openid_appauth_AuthorizationException_Handler:OpenId.AppAuth.AuthorizationService/IRegistrationResponseCallbackInvoker, OpenId.AppAuth.Android\n" +
			"";
		mono.android.Runtime.register ("OpenId.AppAuth.AuthorizationService+RegistrationResponseCallback, OpenId.AppAuth.Android", AuthorizationService_RegistrationResponseCallback.class, __md_methods);
	}


	public AuthorizationService_RegistrationResponseCallback ()
	{
		super ();
		if (getClass () == AuthorizationService_RegistrationResponseCallback.class)
			mono.android.TypeManager.Activate ("OpenId.AppAuth.AuthorizationService+RegistrationResponseCallback, OpenId.AppAuth.Android", "", this, new java.lang.Object[] {  });
	}


	public void onRegistrationRequestCompleted (net.openid.appauth.RegistrationResponse p0, net.openid.appauth.AuthorizationException p1)
	{
		n_onRegistrationRequestCompleted (p0, p1);
	}

	private native void n_onRegistrationRequestCompleted (net.openid.appauth.RegistrationResponse p0, net.openid.appauth.AuthorizationException p1);

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
