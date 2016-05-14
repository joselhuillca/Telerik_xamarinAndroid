using System;
using Android.Support.V4.App;
using Android.Views;
using System.Collections.Generic;
using Android.OS;
using Android.Net;
using Java.Lang;
using Android.Widget;

namespace MLearning.Droid
{
	public class ExampleFragmentBase: Fragment {

		private ExampleLoadedListener listener;

		public bool onBackPressed() {
			return false;
		}

		public ExampleFragmentBase() {
			// Required empty public constructor
		}

		public void unloadExample() {
		}

		public void onHidden() {

		}

		public void onVisualized() {

		}

		public void onExampleSuspended() {

		}

		public void onExampleResumed() {

		}


		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			if (savedInstanceState == null) {
				if (this.usesInternet()) {
					//ConnectivityManager connectivityManager = (ConnectivityManager) this.getActivity().getSystemService(Service.CONNECTIVITY_SERVICE);
					//NetworkInfo ni = connectivityManager.getActiveNetworkInfo();

					///if (ni == null || !ni.isConnectedOrConnecting()) {
						//Toast toast = Toast.makeText(this.getActivity(), R.string.internet_connectivity_prompt, Toast.LENGTH_SHORT);
						//toast.setGravity(Gravity.center, 0, 0);
						//toast.setDuration(Toast.LENGTH_LONG);
						//toast.show();
					//}
				}
			}
		}

		public ExampleSourceModel getSourceCodeModel() {
			return new ExampleSourceModel(this.getClassHierarchyNames());
		}

		public string getEQATECCategory() {
			return "";
		}

		public override void OnResume() {
			base.OnResume();
			if (this.listener != null) {
				//this.listener.onExampleLoaded(this.getView());
			}
		}

		public void setOnExampleLoadedListener(ExampleLoadedListener listener) {
			if (listener != null && this.listener != null) {
				throw new IllegalArgumentException("Listener already set!");
			}
			this.listener = listener;
		}

		protected bool usesInternet() {
			return false;
		}

		private List<string> getClassHierarchyNames() {
			List<string> classes = new List<string>();

			//for (Class c = this.getClass(); c != null; c = c.getSuperclass()) {
				//if (c.getSimpleName().equals(ExampleFragmentBase.class.getSimpleName())) {
				//	break;
				//}

				//classes.Add(c.getSimpleName());
			//}

			return classes;
		}

		public interface ExampleLoadedListener {
			void onExampleLoaded(View root);
		}
	}

}

