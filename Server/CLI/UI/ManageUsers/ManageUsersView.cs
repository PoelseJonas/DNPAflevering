using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository  userRepository;
    private CreateUserView createUserView;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository =  userRepository;
        createUserView = new CreateUserView(userRepository);
    }

    public async Task AddUser(User user)
    {
        await createUserView.AddUser(user.Username, user.Password);
    }

    public async Task DeleteUser(User user)
    {
        //samme måde som over
    }

    public async Task EditUser(User user)
    {
        
    }
}