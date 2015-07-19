package etctimeapp.droid;


public class DbSyncService
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("ETCTimeApp.Droid.DbSyncService, ETCTimeApp.Android, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null", DbSyncService.class, __md_methods);
	}


	public DbSyncService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DbSyncService.class)
			mono.android.TypeManager.Activate ("ETCTimeApp.Droid.DbSyncService, ETCTimeApp.Android, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);

	java.util.ArrayList refList;
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
