package md5ff7b7a43bbfd3e9fd779275d7790ec7e;


public class RecordingListFragment
	extends android.support.v4.app.ListFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onActivityCreated:(Landroid/os/Bundle;)V:GetOnActivityCreated_Landroid_os_Bundle_Handler\n" +
			"n_onListItemClick:(Landroid/widget/ListView;Landroid/view/View;IJ)V:GetOnListItemClick_Landroid_widget_ListView_Landroid_view_View_IJHandler\n" +
			"";
		mono.android.Runtime.register ("BlankDroid.Fragments.RecordingListFragment, BlankDroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RecordingListFragment.class, __md_methods);
	}


	public RecordingListFragment ()
	{
		super ();
		if (getClass () == RecordingListFragment.class)
			mono.android.TypeManager.Activate ("BlankDroid.Fragments.RecordingListFragment, BlankDroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onActivityCreated (android.os.Bundle p0)
	{
		n_onActivityCreated (p0);
	}

	private native void n_onActivityCreated (android.os.Bundle p0);


	public void onListItemClick (android.widget.ListView p0, android.view.View p1, int p2, long p3)
	{
		n_onListItemClick (p0, p1, p2, p3);
	}

	private native void n_onListItemClick (android.widget.ListView p0, android.view.View p1, int p2, long p3);

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
