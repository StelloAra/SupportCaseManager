using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SupportCaseManager.Application.Contracts;
using SupportCaseManager.Application.Services;
using SupportCaseManager.Infrastructure.Repositories;



namespace SupportCaseManager.Presentation
{
    public partial class App : Microsoft.UI.Xaml.Application
    {
        private Window? _window;

        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            InitializeComponent();

            var services = new ServiceCollection();

            services.AddSingleton<ICustomerRepository, JsonCustomerRepository>();
            services.AddSingleton<ITicketRepository, JsonTicketRepository>();

            services.AddSingleton<CustomerService>();
            services.AddSingleton<TicketService>();
            services.AddSingleton<TicketQueryService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = _serviceProvider.GetRequiredService<MainWindow>();
            _window.Activate();
        }
    }
}
