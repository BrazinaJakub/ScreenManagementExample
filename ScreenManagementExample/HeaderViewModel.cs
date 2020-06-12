using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenManagementExample
{
    public class HeaderViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public HeaderViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void New()
        {
            _eventAggregator.PublishOnUIThread(new New());
        }
    }
}
