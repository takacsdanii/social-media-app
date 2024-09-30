namespace CatchUp_server.ViewModels.AuthViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}
