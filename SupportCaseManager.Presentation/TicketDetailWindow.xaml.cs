using Microsoft.UI.Xaml;
using SupportCaseManager.Domain;
using SupportCaseManager.Application.Services;


namespace SupportCaseManager.Presentation
{

    public sealed partial class TicketDetailWindow : Window
    {
        private readonly SupportTicket _ticket;
        private readonly CustomerService _customerService;

        public TicketDetailWindow(SupportTicket ticket,
            CustomerService customerService)
        {
            _ticket = ticket;
            _customerService = customerService;

            InitializeComponent();

            LoadTicket();
        }

        private void LoadTicket()
        {
            TitleTextBlock.Text = _ticket.Title;
            DescriptionTextBlock.Text = _ticket.Description;
            PriorityTextBlock.Text = $"Prioritet: {_ticket.Priority}";
            StatusTextBlock.Text = $"Status: {_ticket.Status}";
            CreatedAtTextBlock.Text = $"Skapad: {_ticket.CreatedAt}";

            var customer = _customerService.GetCustomer(_ticket.CustomerId);

            if (customer != null)
            {
                CustomerTextBlock.Text = $"Kund: {customer.Name.Value}";
            }
        }

    }
}
