using System;
using Com.Telerik.Widget.List;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;

namespace MLearning.Droid
{
	public class ObjetoAdapter : ListViewAdapter
	{
		private int width;
		private int height;

		public ObjetoAdapter(List<ObjetoTelerik> items) {
			base.Add (items);
		}

		public void setDimens(int width, int height) {
			this.width = width;
			this.height = height;
		}

		public override ListViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
			LayoutInflater inflater = LayoutInflater.From(parent.Context);
			View itemView = inflater.Inflate (Resource.Layout.test_telerik_item, parent, false); 
			return new DestinationViewHolder(itemView);
		}


		public override void onBindViewHolder(ListViewHolder holder, int position) {
			ObjetoTelerik item = (ObjetoTelerik)this.GetItem (position);

			DestinationViewHolder typedVh = (DestinationViewHolder)(holder);

			typedVh.position = position;
			typedVh.destinationInfo.SetText(item.getName());
			typedVh.destinationTitle.SetText(item.getCountry());

		}
		public class DestinationViewHolder : ListViewHolder {
			// public ImageView destinationImage;
			public TextView destinationTitle;
			public TextView destinationInfo;
			public ViewGroup destinationEnquiryLayout;
			// public Button destinationEnquiry;
			public View separator;
			public ViewGroup layout;
			public int position;

			private Context context;

			public DestinationViewHolder(View itemView) {
				base.Add(itemView);

				this.context = itemView.Context;
				// this.destinationImage = (ImageView) itemView.findViewById(R.id.destinationImage);
				this.destinationTitle = (TextView) itemView.FindViewById(Android.Resource.Id.destinationTitle);
				this.destinationInfo = (TextView) itemView.FindViewById(Android.Resource.Id.destinationInfo);
				//  this.destinationEnquiry = (Button) itemView.findViewById(R.id.destinationEnquiry);
				// this.destinationEnquiry.setOnClickListener(new View.OnClickListener() {
				//   @Override
				//   public void onClick(View v) {
				//       sendEnquiry();
				//   }
				// });
				this.layout = (ViewGroup)itemView.FindViewById(Android.Resource.Id.scrollingLayout);
				//this.destinationEnquiryLayout = (ViewGroup) itemView.findViewById(R.id.destinationEnquiryLayout);
				this.separator = itemView.FindViewById(Android.Resource.Id.separator);
			}

		}

	}
}

