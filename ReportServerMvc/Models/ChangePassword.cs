namespace fyiReporting.ReportServerMvc.Models
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
