﻿using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using System.IO;

namespace MLearning.Core
{
	public class CameraViewModel: MvxViewModel
	{
		private readonly IMvxPictureChooserTask _pictureChooserTask;

		public CameraViewModel(IMvxPictureChooserTask pictureChooserTask)
		{
			_pictureChooserTask = pictureChooserTask;
		}

		private Cirrious.MvvmCross.ViewModels.MvxCommand _takePictureCommand;
		public System.Windows.Input.ICommand TakePictureCommand
		{
			get
			{
				_takePictureCommand = _takePictureCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand(DoTakePicture);
				return _takePictureCommand;
			}
		}

		private void DoTakePicture()
		{
			_pictureChooserTask.TakePicture(400, 95, OnPicture, () => { });
		}

		private Cirrious.MvvmCross.ViewModels.MvxCommand _choosePictureCommand;
		public System.Windows.Input.ICommand ChoosePictureCommand
		{
			get
			{
				_choosePictureCommand = _choosePictureCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand(DoChoosePicture);
				return _choosePictureCommand;
			}
		}

		private void DoChoosePicture()
		{
			_pictureChooserTask.ChoosePictureFromLibrary(400, 95, OnPicture, () => {});
		}

		private byte[] _bytes;
		public byte[] Bytes
		{
			get { return _bytes; }
			set { _bytes = value; RaisePropertyChanged(() => Bytes); }
		}


		private void OnPicture(Stream pictureStream)
		{
			var memoryStream = new MemoryStream();
			pictureStream.CopyTo(memoryStream);
			Bytes = memoryStream.ToArray();

		
		}

		MvxCommand _registerOmmand;
		public System.Windows.Input.ICommand RegisterCommand
		{
			get
			{
				_registerOmmand = _registerOmmand ?? new MvxCommand(DoRegisterCommand);
				return _registerOmmand;
			}
		}

		void DoRegisterCommand()
		{
			ShowViewModel<RegisterViewModel>();
		}


	}
}

