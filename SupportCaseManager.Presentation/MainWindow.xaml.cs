using Microsoft.UI.Xaml;
using SupportCaseManager.Application.Services;

namespace SupportCaseManager.Presentation
{
    public sealed partial class MainWindow : Window
    {
        private readonly TicketQueryService _ticketQueryService;

        public MainWindow(TicketQueryService ticketQueryService)
        {
            InitializeComponent();

            _ticketQueryService = ticketQueryService;
        }
    }
}