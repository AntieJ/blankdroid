package md57af922a912a9c1d2e90b297c1016fe19;


public class RecordingListActivity
	extends android.support.v4.app.ListFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onActivityCreated:(Landroid/os/Bundle;)V:GetOnActivityCreated_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("BlankDroid.RecordingListActivity, BlankDroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RecordingListActivity.class, __md_methods);
	}


	public RecordingListActivity ()
	{
		super ();
		if (getClass () == RecordingListActivity.class)
			mono.android.TypeManager.Activate ("BlankDroid.RecordingListActivity, BlankDroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onActivityCreated (android.os.Bundle p0)
	{
		n_onActivityCreated (p0);
	}

	private native void n_onActivityCreated (android.os.Bundle p0);

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
