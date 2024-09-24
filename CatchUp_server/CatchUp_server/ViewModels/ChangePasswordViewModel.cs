namespace CatchUp_server.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}
