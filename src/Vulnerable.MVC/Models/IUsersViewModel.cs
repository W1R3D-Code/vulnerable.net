
namespace Vulnerable.MVC.Models
{
    public interface IUsersViewModel<TUser> where TUser : class
    {
        IList<TUser> Admins { get; set; }
        IList<TUser> Users { get; set; }
    }
}