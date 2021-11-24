namespace Vulnerable.MVC.Models
{
    public abstract class GenericAccountsViewModel<TUser> : IUsersViewModel<TUser> where TUser : class
    {
        public virtual IList<TUser> Admins { get; set; }

        public virtual IList<TUser> Users { get; set; }

        public GenericAccountsViewModel(IList<TUser> admins, IList<TUser> users)
        {
            Admins = admins ?? throw new ArgumentNullException(nameof(admins));
            Users = users ?? throw new ArgumentNullException(nameof(users));
        }
    }
}
