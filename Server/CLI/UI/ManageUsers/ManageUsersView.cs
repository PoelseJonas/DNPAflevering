using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository  userRepository;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository =  userRepository;
    }
}