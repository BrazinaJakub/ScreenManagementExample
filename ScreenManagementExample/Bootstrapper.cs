﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Windows;
using System.CodeDom;

namespace ScreenManagementExample
{
    public class Bootstrapper : BootstrapperBase
    {      
        private readonly SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container
                        .Singleton<IWindowManager, WindowManager>()
                        .Singleton<IEventAggregator, EventAggregator>()
                        .Singleton<ShellViewModel>()
                        .Singleton<HeaderViewModel>()
                        //.Singleton<NewFileViewModel>()
                        .Singleton<FileConductorViewModel>()
                        .RegisterPerRequest(typeof(FileViewModel), null, typeof(FileViewModel));
            
                        //.Singleton<FileViewModel>();

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
