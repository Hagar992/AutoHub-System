public class ManageRolesViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public IList<string> UserRoles { get; set; }
    public List<string> AvailableRoles { get; set; }
}
