using Android.App;
using Android.OS;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Core.Repositories;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using MLearning.Core.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using System.Collections.Generic;
using Android.Support.V4.View;
using DataSource;
using System.Collections.ObjectModel;
using Android.Content.PM;
using Android.Content;
using System.Threading;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;

namespace MLearning.Droid.Views
{
	[Activity(Label = "View for LOViewModel", ScreenOrientation = ScreenOrientation.Portrait)]
	public class LOView : MvxActivity, VerticalScrollViewPager.ScrollViewListenerPager
	{

		LOViewModel vm; 
		Bitmap bm_user;
		Bitmap bmLike;
		Drawable drBack;


		ProgressDialog _dialogDownload;
		//	LinearLayout layoutPanelScroll;
		RelativeLayout _mainLayout;
		RelativeLayout mainLayoutIndice;
		RelativeLayout mainLayoutPages;


		int widthInDp;
		int heightInDp;
		List<FrontContainerView> listFront = new List<FrontContainerView> ();


		List<FrontContainerViewPager> listFrontPager = new List<FrontContainerViewPager>();
		//	VerticalScrollView scrollVertical;
		bool ISLOADED= false;
		int IndiceSection=0;

		List<VerticalScrollViewPager> listaScroll = new List<VerticalScrollViewPager>();
		List<VerticalScrollViewPager> listaScrollIni = new List<VerticalScrollViewPager>();

		ViewPager viewPager;
		ViewPager viewPagerIni;

		public List<string> adsImagesPath = new List<string>();
		public LinearLayout selectLayout;
		public LinearLayout _publicidadLayout;
		public LinearLayout _adLayout;
		public bool adOpen = false;

		async protected  override  void OnCreate(Bundle bundle)
		{
			


			this.Window.AddFlags(WindowManagerFlags.Fullscreen);
			base.OnCreate(bundle);
			var metrics = Resources.DisplayMetrics;
			widthInDp = ((int)metrics.WidthPixels);
			heightInDp = ((int)metrics.HeightPixels);
			Configuration.setWidthPixel (widthInDp);
			Configuration.setHeigthPixel (heightInDp);
			vm = this.ViewModel as LOViewModel;

			int tam = Configuration.getWidth (80);

			drBack = new BitmapDrawable(Bitmap.CreateScaledBitmap (getBitmapFromAsset ("images/fondocondiagonalm.png"), 640, 1136, true));
			LinearLayout test = new LinearLayout (this);
			test.LayoutParameters = new LinearLayout.LayoutParams (-1, -1);
			test.SetBackgroundColor(Color.Black);
			SetContentView (test);


			_dialogDownload = new ProgressDialog (this);
			_dialogDownload.SetCancelable (false);
			_dialogDownload.SetMessage ("Cargando");
			_dialogDownload.Show ();

			adsImagesPath = AddResources.Instance.addList;

			await ini();
			//LoadPagesDataSource ();

			SetContentView (_mainLayout);

			_dialogDownload.Dismiss ();
		} 

		void showAd(int idAd)
		{
			adOpen = true;
			_adLayout = new LinearLayout (this);
			_adLayout.LayoutParameters = new LinearLayout.LayoutParams (-1, Configuration.getHeight (255));
			Drawable dr = new BitmapDrawable (getBitmapFromAsset (adsImagesPath[idAd]));
			_adLayout.SetBackgroundDrawable (dr);
			_adLayout.SetY (Configuration.getHeight(1136-85-255));
			_mainLayout.AddView (_adLayout);

			_adLayout.Click += delegate {
				this.StartActivity(Configuration.getOpenFacebookIntent(this,"fb://page/114091405281757","http://www.hi-tec.com/pe/"));
			};
		}

		void hideAd()
		{
			adOpen = false;
			_mainLayout.RemoveView (_adLayout);
		}


		async Task  ini(){

			_mainLayout = new RelativeLayout (this);

			_mainLayout.LayoutParameters = new RelativeLayout.LayoutParams (-1,-1);	
			_mainLayout.SetBackgroundColor(Color.ParseColor("#ffffff"));

			mainLayoutIndice = new RelativeLayout (this);
			mainLayoutIndice.LayoutParameters = new RelativeLayout.LayoutParams (-1,-1);	
			mainLayoutIndice.SetBackgroundColor(Color.ParseColor("#ffffff"));

			mainLayoutPages = new RelativeLayout (this);
			mainLayoutPages.LayoutParameters = new RelativeLayout.LayoutParams (-1,Configuration.getWidth(1136-85));	
			mainLayoutPages.SetBackgroundColor(Color.ParseColor("#ffffff"));
			viewPager = new ViewPager (this);
			viewPagerIni = new ViewPager (this);

			mainLayoutIndice.SetX (0); mainLayoutIndice.SetY (0);
			_mainLayout.AddView (mainLayoutIndice);


			await vm.InitLoad();

			LoadPagesDataSource();

			viewPagerIni.SetOnPageChangeListener (new MyPageChangeListener (this,listFront));
			viewPager.SetOnPageChangeListener (new MyPageChangeListenerPager (this, listFrontPager));
			vm.PropertyChanged += Vm_PropertyChanged;


		}

		void Vm_PropertyChanged (object sender, PropertyChangedEventArgs e)
		{

			//var vm = this.ViewModel as LOViewModel;
			if (e.PropertyName == "IsWaiting") {
			}

			if (e.PropertyName == "LOsInCircle")
			if (vm.LOsInCircle != null) {
			}
			//vm.LOsInCircle.CollectionChanged+= Vm_LOsInCircle_CollectionChanged;
		}

		void Vm_LOsInCircle_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{

			//loadLOsInCircle (e.NewStartingIndex);		
			if (e.NewItems != null) {
				int i = 0;
				foreach (LOViewModel.lo_by_circle_wrapper lobycircle in e.NewItems) {
					VerticalScrollViewPager scrollPager = new VerticalScrollViewPager (this);
					scrollPager.setOnScrollViewListener (this); 
					LinearLayout linearScroll = new LinearLayout (this);
					linearScroll.LayoutParameters = new LinearLayout.LayoutParams (-1, -2);
					linearScroll.Orientation = Orientation.Vertical;

					//	if(Configuration.IndiceActual==i){
					FrontContainerView front = new FrontContainerView (this);
					front.Tag = "indice";
					front.Author = lobycircle.lo.name + " " + vm.LOsInCircle [i].lo.lastname;
					front.Chapter = lobycircle.lo.description;
					front.NameLO = lobycircle.lo.title;
					front.Like = "10";
					front.ImageChapter = lobycircle.lo.url_background;

					listFront.Add (front);
					listFront [i].setBack (drBack,bmLike);

					lobycircle.PropertyChanged += (s1, e1) =>
					{
						if (e1.PropertyName == "background_bytes")
						{
							front.ImageChapter = lobycircle.lo.url_background;

						}
					};								

					linearScroll.AddView (front);
					if (lobycircle.stack.IsLoaded) {				
						var s_list = lobycircle.stack.StacksList;
						int indice = 0;
						for (int j = 0; j < s_list.Count; j++) {				


							for (int k = 0; k < s_list [j].PagesList.Count; k++) {

								ChapterContainerView section = new ChapterContainerView (this);								
								section.Author = lobycircle.lo.name + " " + lobycircle.lo.lastname;					
								section.Title = s_list [j].PagesList [k].page.title;
								section.Container = s_list [j].PagesList [k].page.description;							
								section.ColorText = Configuration.ListaColores [indice % 6];
								section.setDefaultProfileUserBitmap (bm_user);


								section.Image = s_list [j].PagesList [k].page.url_img;
								section.Indice = indice;								
								section.Click += delegate {
									IndiceSection = section.Indice; 										
									mainLayoutIndice.Visibility = ViewStates.Invisible;		
									if (ISLOADED == false) {		
										LoadPagesDataSource();
									} else {
										viewPager.CurrentItem= IndiceSection;
										mainLayoutPages.Visibility = ViewStates.Visible;
										mainLayoutIndice.Visibility = ViewStates.Invisible;
									}

								};
								linearScroll.AddView (section);
								indice++;
							}

						}
					}
					scrollPager.VerticalScrollBarEnabled = false;
					scrollPager.AddView (linearScroll);
					listaScrollIni.Add (scrollPager);




					i++;
				}

				mainLayoutIndice.RemoveAllViews ();
				//_progresD.Hide ();
				mainLayoutIndice.AddView (viewPagerIni);
				mainLayoutIndice.SetX (0);
				mainLayoutIndice.SetY (0);
				//mainLayout.AddView (mainLayoutIndice);
				LOViewAdapter adapter = new LOViewAdapter (this, listaScrollIni);
				viewPagerIni.Adapter = adapter;



			}




		}

		void loadLOsInCircle(int index){


			//var vm = this.ViewModel as LOViewModel;
			if (vm.LOsInCircle != null) {		


				for (int i = 0; i < vm.LOsInCircle.Count; i++) {
					VerticalScrollViewPager scrollPager = new VerticalScrollViewPager (this);
					scrollPager.setOnScrollViewListener (this); 
					LinearLayout linearScroll = new LinearLayout (this);
					linearScroll.LayoutParameters = new LinearLayout.LayoutParams (-1, -2);
					linearScroll.Orientation = Orientation.Vertical;

					//	if(Configuration.IndiceActual==i){
					FrontContainerView front = new FrontContainerView (this);
					front.Tag = "indice";
					front.Author = vm.LOsInCircle [i].lo.name + " " + vm.LOsInCircle [i].lo.lastname;
					front.Chapter = vm.LOsInCircle [i].lo.description;
					front.NameLO = vm.LOsInCircle [i].lo.title;
					front.Like = "10";
					front.ImageChapter = vm.LOsInCircle [i].lo.url_background;

					listFront.Add (front);
					listFront [i].setBack (drBack,bmLike);


					/*
					if (vm.LOsInCircle [i].background_bytes != null) {
						Bitmap bm = BitmapFactory.DecodeByteArray (vm.LOsInCircle [i].background_bytes, 0, vm.LOsInCircle [i].background_bytes.Length);

						front.ImageChapterBitmap = bm;
						bm = null;
					}
					*/

					vm.LOsInCircle[i].PropertyChanged += (s1, e1) =>
					{
						if (e1.PropertyName == "background_bytes")
						{
							/*
							Bitmap bm = BitmapFactory.DecodeByteArray (vm.LOsInCircle [i].background_bytes, 0, vm.LOsInCircle [i].background_bytes.Length);
							front.ImageChapterBitmap = bm;
							bm = null;
							*/
							front.ImageChapter = vm.LOsInCircle [i].lo.url_background;

						}
					};								

					linearScroll.AddView (front);

					if (vm.LOsInCircle [i].stack.IsLoaded) {				
						var s_list = vm.LOsInCircle [i].stack.StacksList;
						int indice = 0;
						for (int j = 0; j < s_list.Count; j++) {				


							for (int k = 0; k < s_list [j].PagesList.Count; k++) {

								ChapterContainerView section = new ChapterContainerView (this);								
								section.Author = vm.LOsInCircle [i].lo.name + " " + vm.LOsInCircle [i].lo.lastname;					
								section.Title = s_list [j].PagesList [k].page.title;
								section.Container = s_list [j].PagesList [k].page.description;							
								section.ColorText = Configuration.ListaColores [indice % 6];
								section.setDefaultProfileUserBitmap (bm_user);


								section.Image = s_list [j].PagesList [k].page.url_img;
								/*
								if (s_list [j].PagesList [k].cover_bytes != null) {
									Bitmap bm = BitmapFactory.DecodeByteArray (s_list [j].PagesList [k].cover_bytes, 0, s_list [j].PagesList[k].cover_bytes.Length);
									section.ImageBitmap = bm;
									bm = null;
								}
								*/

								section.Indice = indice;								
								section.Click += delegate {


									IndiceSection = section.Indice; 										



									mainLayoutIndice.Visibility = ViewStates.Invisible;		



									if (ISLOADED == false) {		
										LoadPagesDataSource();
									} else {

										viewPager.CurrentItem= IndiceSection;
										mainLayoutPages.Visibility = ViewStates.Visible;

										mainLayoutIndice.Visibility = ViewStates.Invisible;



									}

								};
								linearScroll.AddView (section);
								indice++;
							}

						}
					} else {
						vm.LOsInCircle [i].stack.PropertyChanged+= (s3, e3) => {
							var s_list = vm.LOsInCircle [i].stack.StacksList;
							int indice = 0;
							for (int j = 0; j < s_list.Count; j++) {
								for (int k = 0; k < s_list [j].PagesList.Count; k++) {
									ChapterContainerView section = new ChapterContainerView (this);								
									section.Author = vm.LOsInCircle [i].lo.name + " " + vm.LOsInCircle [i].lo.lastname;					
									section.Title = s_list [j].PagesList [k].page.title;
									section.Container = s_list [j].PagesList [k].page.description;							
									section.ColorText = Configuration.ListaColores [indice % 6];
									section.setDefaultProfileUserBitmap (bm_user);
									section.Image = s_list [j].PagesList [k].page.url_img;
									section.Indice = indice;								
									section.Click += delegate {
										IndiceSection = section.Indice; 										
										mainLayoutIndice.Visibility = ViewStates.Invisible;		
										if (ISLOADED == false) {	
											LoadPagesDataSource();
										} else {
											viewPager.CurrentItem= IndiceSection;
											mainLayoutPages.Visibility = ViewStates.Visible;
											mainLayoutIndice.Visibility = ViewStates.Invisible;
										}
									};
									linearScroll.AddView (section);
									indice++;
								}

							}
						};

					}

					scrollPager.VerticalScrollBarEnabled = false;
					scrollPager.AddView (linearScroll);

					listaScrollIni.Add (scrollPager);

				}
				mainLayoutIndice.RemoveAllViews ();
				//_progresD.Hide ();
				mainLayoutIndice.AddView (viewPagerIni);
				mainLayoutIndice.SetX (0);
				mainLayoutIndice.SetY (0);
				//mainLayout.AddView (mainLayoutIndice);
				LOViewAdapter adapter = new LOViewAdapter (this, listaScrollIni);
				viewPagerIni.Adapter = adapter;
				//viewPager.CurrentItem = IndiceSection;
			}

		}



		void LoadPagesDataSource()
		{



			bool is_main = true;
			int space = Configuration.getWidth (30);
		
			var s_listp = vm.LOsInCircle[vm._currentUnidad].stack.StacksList;
				int indice = 0;

				if (s_listp != null) {


				int j = vm._currentSection;
				//	for (int j = 0; j < s_listp.Count; j++) {						

						for (int k = 0; k < s_listp [j].PagesList.Count; k++) {

				//		if (j == vm._currentSection) {

							VerticalScrollViewPager scrollPager = new VerticalScrollViewPager (this);
							scrollPager.setOnScrollViewListener (this); 
							LinearLayout linearScroll = new LinearLayout (this);
							linearScroll.LayoutParameters = new LinearLayout.LayoutParams (-1, -2);
							linearScroll.Orientation = Orientation.Vertical;

							var content = s_listp [j].PagesList [k].content;
							FrontContainerViewPager front = new FrontContainerViewPager (this);
							front.Tag = "pager";


							front.ImageChapter = s_listp [j].PagesList [k].page.url_img;


							front.Title = s_listp [j].PagesList [k].page.title;
							front.Description = s_listp [j].PagesList [k].page.description;


							var slides = s_listp [j].PagesList [k].content.lopage.loslide;
							front.setBack (drBack);


							linearScroll.AddView (front);

							LinearLayout descriptionLayout = new LinearLayout (this);
							descriptionLayout.LayoutParameters = new LinearLayout.LayoutParams (-1, -2);
							descriptionLayout.SetPadding (space, 0, space, space);
							descriptionLayout.Orientation = Orientation.Vertical;

							TextView titulo_detalle = new TextView (this);
							titulo_detalle.Text = "Descripción";
							titulo_detalle.Typeface = Typeface.CreateFromAsset (this.Assets, "fonts/ArcherMediumPro.otf");
							titulo_detalle.SetTextSize (ComplexUnitType.Fraction, Configuration.getHeight (38));
							titulo_detalle.SetTextColor (Color.ParseColor (Configuration.ListaColores [indice % 6]));
							titulo_detalle.SetPadding (0, 0, 0, space);
							descriptionLayout.AddView (titulo_detalle);

							TextView detalle = new TextView (this);
							detalle.TextFormatted = Html.FromHtml (slides [0].loparagraph);
							detalle.Typeface = Typeface.CreateFromAsset (this.Assets, "fonts/ArcherMediumPro.otf");
							detalle.SetTextSize (ComplexUnitType.Fraction, Configuration.getHeight (32));
							descriptionLayout.AddView (detalle);

							ViewTreeObserver vto = detalle.ViewTreeObserver;
							int H = 0;
							vto.GlobalLayout += (sender, args) =>
							{     
								H = detalle.Height;
								Console.WriteLine ("TAM:::1:" + H );
								detalle.LayoutParameters.Height = H-Configuration.getHeight(60);

							};  



							LinearLayout separationLinear = new LinearLayout (this);
							separationLinear.LayoutParameters = new LinearLayout.LayoutParams (-1, 5);
							separationLinear.SetBackgroundColor (Color.ParseColor ("#D8D8D8"));
							separationLinear.Orientation = Orientation.Horizontal;
							//separationLinear.SetPadding (0,0,0,50);

							linearScroll.AddView (descriptionLayout);
							linearScroll.AddView (separationLinear);

							listFrontPager.Add (front);

							var currentpage = s_listp [j].PagesList [k];

				


							for (int m = 1; m < slides.Count; m++) {
								LOSlideSource slidesource = new LOSlideSource (this);

								var _id_ = vm.LOsInCircle [vm._currentUnidad].lo.color_id;
								is_main = !is_main;


								slidesource.ColorS = Configuration.ListaColores [indice % 6];

								slidesource.Type = slides [m].lotype;
								if (slides [m].lotitle != null)
									slidesource.Title = slides [m].lotitle;
								if (slides [m].loparagraph != null)
									slidesource.Paragraph = slides [m].loparagraph;
								if (slides [m].loimage != null)
									slidesource.ImageUrl = slides [m].loimage;
								if (slides [m].lotext != null)
									slidesource.Paragraph = slides [m].lotext;
								if (slides [m].loauthor != null)
									slidesource.Author = slides [m].loauthor;
								if (slides [m].lovideo != null)
									slidesource.VideoUrl = slides [m].lovideo;
					
								var c_slide = slides [m];


								if (c_slide.loitemize != null) {
									slidesource.Itemize = new ObservableCollection<LOItemSource> ();
									var items = c_slide.loitemize.loitem;

									for (int n = 0; n < items.Count; n++) { 
										LOItemSource item = new LOItemSource ();
										if (items [n].loimage != null)
											item.ImageUrl = items [n].loimage;
										if (items [n].lotext != null)
											item.Text = items [n].lotext;


										var c_item_ize = items [n];
										
										slidesource.Itemize.Add (item);
									}
								}


								
								slidesource.title_page = front.Title;
								linearScroll.AddView (slidesource.getViewSlide ());


							} 

							scrollPager.VerticalScrollBarEnabled = false;
							if (k == 0) {
								scrollPager.AddView (linearScroll);
								listaScroll.Add (scrollPager);
								indice++;
							}


					//	}
						}

					//}

				} else {
					Console.WriteLine ("ERROR");
				}


			mainLayoutPages.RemoveAllViews ();
			mainLayoutPages.AddView (viewPager);
			mainLayoutPages.SetX (0);
			mainLayoutPages.SetY (0);
			_mainLayout.AddView (mainLayoutPages);

			_publicidadLayout = new LinearLayout (this);
			_publicidadLayout.LayoutParameters = new LinearLayout.LayoutParams (-1, Configuration.getHeight (85));
			Drawable drp = new BitmapDrawable (getBitmapFromAsset ("images/footerad.jpg"));
			_publicidadLayout.SetBackgroundDrawable (drp);
			_publicidadLayout.SetY (Configuration.getHeight(1136-85));
			_mainLayout.AddView (_publicidadLayout);
			_publicidadLayout.Click += delegate {
				if (adOpen) {
					hideAd ();
				} else {
					Random rnd = new Random();
					showAd (rnd.Next(adsImagesPath.Count));
				}
			};


			LOViewAdapter adapter = new LOViewAdapter (this, listaScroll);
			viewPager.Adapter = adapter;
			//viewPager.CurrentItem = IndiceSection;
			//viewPager.SetCurrentItem (vm._currentSection, true);
		}
		/*
		public void  OnScrollChanged(VerticalScrollView scrollView, int l, int t, int oldl, int oldt) {

			listFront[0].Imagen.SetY (scrollView.ScrollY / 2);
		//	listFront[0].Imagen.ScaleX =scrollView.ScrollY / 10;
		//	listFront[0].Imagen.ScaleY =scrollView.ScrollY / 10;

		}*/

		public void OnScrollChangedPager(VerticalScrollViewPager scrollView, int l, int t, int oldl, int oldt) {
			var view=(LinearLayout)scrollView.GetChildAt (0);

			if (view.GetChildAt (0).Tag.Equals("indice")) {
				var pagerrr =  (FrontContainerView)view.GetChildAt (0);
				pagerrr.Imagen.SetY (scrollView.ScrollY / 2);	
			}if (view.GetChildAt (0).Tag.Equals("pager")) {
				var pagerrr =  (FrontContainerViewPager)view.GetChildAt (0);
				pagerrr.Imagen.SetY (scrollView.ScrollY / 2);	
			}


			//Console.WriteLine("SCROLLEOOO LOS PAGERRRRRRRRRRRRR "+ scrollView.ScrollY);

		}


		public Bitmap getBitmapFromAsset( String filePath) {
			System.IO.Stream s =this.Assets.Open (filePath);
			Bitmap bitmap = BitmapFactory.DecodeStream (s);

			return bitmap;

		}


		public class MyPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
		{
			Context _context;
			List<FrontContainerView> listFront;
			public MyPageChangeListener (Context context, List<FrontContainerView> listFront)
			{
				_context = context;	
				this.listFront = listFront;

			}

			#region IOnPageChangeListener implementation
			public void OnPageScrollStateChanged (int p0)
			{
				//Console.WriteLine (p0);
			}

			public void OnPageScrolled (int p0, float p1, int p2)
			{
				//Console.WriteLine ("p0 = " + p0 + " p1 = " + p1 + " p2 = " + p2);
				listFront [p0].Imagen.SetX (p2 / 2);		
			}

			public void OnPageSelected (int position)
			{
				//	Toast.MakeText (_context, "Changed to page " + position, ToastLength.Short).Show ();
			}
			#endregion
		}




		public class MyPageChangeListenerPager : Java.Lang.Object, ViewPager.IOnPageChangeListener
		{
			Context _context;
			List<FrontContainerViewPager> listFront;
			//ScrollViewHorizontal scroll;
			public MyPageChangeListenerPager (Context context, List<FrontContainerViewPager> listFront)
			{
				_context = context;	
				this.listFront = listFront;

			}

			#region IOnPageChangeListener implementation
			public void OnPageScrollStateChanged (int p0)
			{
				//Console.WriteLine (p0);
			}

			public void OnPageScrolled (int p0, float p1, int p2)
			{

				//Console.WriteLine ("p0 = " + p0 + " p1 = " + p1 + " p2 = " + p2);
				listFront [p0].Imagen.SetX (p2 / 2);		
				//if(p0+1<listFront.Count){
				//	listFront [p0 + 1].Imagen.SetX (p2/2);
				//}

			}

			public void OnPageSelected (int position)
			{
				//	Toast.MakeText (_context, "Changed to page " + position, ToastLength.Short).Show ();
			}
			#endregion
		}


		public override void OnBackPressed ()
		{
			if (mainLayoutIndice.Visibility == ViewStates.Visible) {
				base.OnBackPressed ();
			}
			ISLOADED = true;
			mainLayoutIndice.Visibility = ViewStates.Visible;
			mainLayoutPages.Visibility = ViewStates.Invisible;


		}

	}
}