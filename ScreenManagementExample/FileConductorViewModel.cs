using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenManagementExample
{
    public class FileConductorViewModel : Conductor<Screen>.Collection.OneActive, IHandle<New>
    {
        
        private readonly IEventAggregator _eventAggregator;
        private readonly FileViewModel _fileViewModel;
        //private readonly NewFileViewModel _newFileViewModel;

        public FileConductorViewModel(IEventAggregator eventAggregator, FileViewModel fileViewModel)
        {
            _eventAggregator = eventAggregator;
            _fileViewModel = fileViewModel;
            Items.AddRange(new Screen[] { _fileViewModel });

        }

        public void Handle(New message)
        {            
            Items.Clear();
            ActivateItem(new FileViewModel(_eventAggregator));
        }

       

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
            //ActivateItem(_fileViewModel);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
            
        }
        public override void CanClose(Action<bool> callback)
        {
            
        }

        
    }
}
