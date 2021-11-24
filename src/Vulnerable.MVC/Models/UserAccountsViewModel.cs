using Vulnerable.MVC.Areas.Identity.Data;

namespace Vulnerable.MVC.Models
{
    public class UserAccountsViewModel : GenericAccountsViewModel<User>
    {
        public UserAccountsViewModel() : base(new List<User>(), new List<User>())
        {

        }

        public UserAccountsViewModel(IList<User> admins, IList<User> users) : base(admins, users)
        {
        }
    }
}
