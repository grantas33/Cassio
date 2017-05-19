package md55d72418a81de8ab5a4fe88ef1a8a12c9;


public class DayListActivity
	extends android.app.ListActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("android2.DayListActivity, CassioApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DayListActivity.class, __md_methods);
	}


	public DayListActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DayListActivity.class)
			mono.android.TypeManager.Activate ("android2.DayListActivity, CassioApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
