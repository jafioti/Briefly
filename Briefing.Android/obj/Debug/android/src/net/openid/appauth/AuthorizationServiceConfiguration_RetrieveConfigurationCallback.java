package net.openid.appauth;


public class AuthorizationServiceConfiguration_RetrieveConfigurationCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		net.openid.appauth.AuthorizationServiceConfiguration.RetrieveConfigurationCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFetchConfigurationCompleted:(Lnet/openid/appauth/AuthorizationServiceConfiguration;Lnet/openid/appauth/AuthorizationException;)V:GetOnFetchConfigurationCompleted_Lnet_openid_appauth_AuthorizationServiceConfiguration_Lnet_openid_appauth_AuthorizationException_Handler:OpenId.AppAuth.AuthorizationServiceConfiguration/IRetrieveConfigurationCallbackInvoker, OpenId.AppAuth.Android\n" +
			"";
		mono.android.Runtime.register ("OpenId.AppAuth.AuthorizationServiceConfiguration+RetrieveConfigurationCallback, OpenId.AppAuth.Android", AuthorizationServiceConfiguration_RetrieveConfigurationCallback.class, __md_methods);
	}


	public AuthorizationServiceConfiguration_RetrieveConfigurationCallback ()
	{
		super ();
		if (getClass () == AuthorizationServiceConfiguration_RetrieveConfigurationCallback.class)
			mono.android.TypeManager.Activate ("OpenId.AppAuth.AuthorizationServiceConfiguration+RetrieveConfigurationCallback, OpenId.AppAuth.Android", "", this, new java.lang.Object[] {  });
	}


	public void onFetchConfigurationCompleted (net.openid.appauth.AuthorizationServiceConfiguration p0, net.openid.appauth.AuthorizationException p1)
	{
		n_onFetchConfigurationCompleted (p0, p1);
	}

	private native void n_onFetchConfigurationCompleted (net.openid.appauth.AuthorizationServiceConfiguration p0, net.openid.appauth.AuthorizationException p1);

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
