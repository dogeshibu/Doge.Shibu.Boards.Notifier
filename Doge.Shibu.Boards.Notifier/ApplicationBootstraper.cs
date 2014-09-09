using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Caliburn.Micro;
using Doge.Shibu.Boards.Notifier.Handlers;
using Doge.Shibu.Boards.Notifier.Models.Implementations;
using Doge.Shibu.Boards.Notifier.Models.Implementations.Finders;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;
using Doge.Shibu.Boards.Notifier.ViewModels;

namespace Doge.Shibu.Boards.Notifier
{
    public class ApplicationBootstrapper : BootstrapperBase
    {
        public ApplicationBootstrapper()
        {
            Initialize();
        }

        private IContainer _container;

        protected override void Configure()
        {
            base.Configure();

            var builder = new ContainerBuilder();

            builder.RegisterType<MetroWindowManager>().As<IWindowManager>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<HkThreadFinder>().As<IThreadFinder>().SingleInstance();
            builder.Register(c => new ReusableThreadFinderNotifier(c.Resolve<IThreadFinder>(), c.Resolve<IEventAggregator>().PublishOnUIThread))
                .As<INotifier>()
                .SingleInstance();
            builder.RegisterType<ShellViewModel>().AsSelf();
            builder.RegisterType<ThreadNotificationHandler>().AsSelf().SingleInstance();

            _container = builder.Build();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
           
            _container.Resolve<IEventAggregator>().Subscribe(_container.Resolve<ThreadNotificationHandler>());
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.ResolveOptional(service) ?? base.GetInstance(service, key);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _container.Resolve<IWindowManager>().ShowWindow(new ErrorNotificationViewModel(e.Exception));
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
            _container.Dispose();
        }
    }
}
