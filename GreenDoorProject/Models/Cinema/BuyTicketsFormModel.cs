namespace GreenDoorProject.Models.Cinema
{
    public class BuyTicketsFormModel
    {
        public string Id { get; set; }

        public string MovieTitle { get; set; }

        public string ImagePath { get; set; }

        public decimal PricePerTicket { get; set; }

        public int NumberOfTickets { get; set; }

        public string ProjectionId { get; set; }
    }
}
