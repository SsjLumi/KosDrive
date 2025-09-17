namespace KosDrive.AdminDto
{
    public class AdminOverviewDto
    {
        public int TotalRides { get; set; }
        public int ActiveRides { get; set; }

        public int TotalDrivers { get; set; }
        public int OnlineDrivers { get; set; }

        public int TotalRiders { get; set; }

        public int RegisteredCompanies { get; set; }

        public decimal MonthlyRevenue { get; set; }

        public int PendingDriverApprovals { get; set; }
        public int PendingCompanyApprovals { get; set; }
    }
}
