using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenManagementExample
{
    public class NewFileViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        public NewFileViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public void New()
        {
            _eventAggregator.PublishOnUIThread(new New());
        }
    }
}
