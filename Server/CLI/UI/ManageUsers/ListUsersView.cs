using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
}