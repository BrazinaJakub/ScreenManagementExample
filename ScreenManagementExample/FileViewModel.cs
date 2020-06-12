using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenManagementExample
{
    public class FileViewModel : Screen, IHandle<CloseMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        //public FileViewModel()
        //{

        //}
        public bool IsEmpty { get; set; } = true;
        private double par1;

        public double PAR1
        {
            get { return par1; }
            set
            {
                par1 = value;
                NotifyOfPropertyChange(() => PAR1);
                IsEmpty = false;
                _eventAggregator.PublishOnUIThread(new IsDirtyMessage());
            }
        }

        private double par2;

        public double PAR2
        {
            get { return par2; }
            set
            {
                par2 = value;
                NotifyOfPropertyChange(() => PAR2);
                IsEmpty = false;
                _eventAggregator.PublishOnUIThread(new IsDirtyMessage());
            }
        }



        public FileViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;            
        }
        protected override void OnActivate()
        {
            _eventAggregator.PublishOnUIThread(new FileCreatedMessage { Created = true });
            base.OnActivate();
            //MessageBox.Show("Activating");
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            //SaveFile();
            //MessageBox.Show("Deactivating");
            //SaveFile();
            _eventAggregator.PublishOnUIThread(new FileCreatedMessage { Created = false });
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);

        }

        public override void CanClose(Action<bool> callback)
        {
            MessageBoxResult result = MessageBox.Show("Save File?", "My App", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (SaveFile())
                    {
                        callback(true);
                    }
                    else callback(false);
                    break;
                case MessageBoxResult.No:
                    callback(true);
                    break;
            }
            //_eventAggregator.BeginPublishOnUIThread(new CloseMessage());
        }


        public void CLose()
        {
            TryClose();
        }

        public void Handle(CloseMessage message)
        {
            //MessageBox.Show("Pozadavek na zavreni aplikace");
            SaveFile();
            
        }


        public bool SaveFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "KSK text file (*.ksk)|*.ksk";
            saveFile.FileName = "Nazev";
            if (saveFile.ShowDialog() == true)
            {
                Type t = this.GetType();
                PropertyInfo[] properties = t.GetProperties();
                var propSelection = properties.Where(p => p.PropertyType == typeof(double)).Select(p => p).ToList();

                string[] parameters = new string[propSelection.Count];
                int i = 0;
                foreach (var property in propSelection)
                {
                    parameters[i] = $"{property.Name}={property.GetValue(this)}";
                    i++;
                }
                File.WriteAllLines(saveFile.FileName, parameters);
                _eventAggregator.PublishOnUIThread(new RecentlySavedMessage());
                return true;
            }
            else return false;
            

            //send message která říká jsem uložen když dám close nic se nestane
        }
    }
} 

