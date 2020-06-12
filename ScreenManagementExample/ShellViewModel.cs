using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenManagementExample
{
	public class ShellViewModel : Conductor<Screen>.Collection.OneActive, IHandle<IsDirtyMessage>, IHandle<RecentlySavedMessage>, IHandle<FileCreatedMessage>
	{
		public bool IsDirty { get; set; } = false;
		public bool IsFileCreated { get; set; } = false;
		public bool IsRecentlySaved { get; set; } = false;
		private readonly IEventAggregator _eventAggregator;
		private readonly FileConductorViewModel _fileConductorViewModel;

		public ShellViewModel(IEventAggregator eventAggregator, FileConductorViewModel fileConductorViewModel, HeaderViewModel headerViewModel)
		{
			_eventAggregator = eventAggregator;
			_fileConductorViewModel = fileConductorViewModel;
			HeaderViewModel = headerViewModel;
			Items.AddRange(new Screen[] { _fileConductorViewModel });
		}

		public HeaderViewModel HeaderViewModel { get; }		

		protected override void OnActivate()
		{
			base.OnActivate();
			_eventAggregator.Subscribe(this);
			ActivateItem(_fileConductorViewModel);			
		}

		protected override void OnDeactivate(bool close)
		{			
			base.OnDeactivate(close);
			_eventAggregator.Unsubscribe(this);
		}
		

		public override void CanClose(Action<bool> callback)
		{
			if (IsDirty && !IsRecentlySaved && IsFileCreated)
			{
				MessageBoxResult result = MessageBox.Show("Save File?", "My App", MessageBoxButton.YesNoCancel);
				switch (result)
				{
					case MessageBoxResult.Yes:
						_eventAggregator.PublishOnUIThread(new CloseMessage());
						if (IsRecentlySaved)
						{
							callback(true);
						}
						else callback(false);
						break;
					case MessageBoxResult.No:
						callback(true);
						break;
					case MessageBoxResult.Cancel:
						callback(false);
						break;
				}
			}
			else callback(true);
		}
		
		public void Handle(IsDirtyMessage message)
		{
			IsDirty = true;
		}

		public void Handle(RecentlySavedMessage message)
		{
			IsRecentlySaved = true;
		}

		public void Handle(FileCreatedMessage message)
		{
			IsFileCreated = message.Created;
		}		
	}
}
