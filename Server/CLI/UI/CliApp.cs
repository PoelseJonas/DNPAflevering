using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp(
    IPostRepository postRepository,
    IUserRepository userRepository,
    ICommentRepository commentRepository)
{
    private readonly IPostRepository postRepository = postRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly ICommentRepository commentRepository = commentRepository;

    ManageUsersView manageUsersView = new ManageUsersView(userRepository);

    ManageCommentsView manageCommentsView =
        new ManageCommentsView(commentRepository);

    ManagePostsView managePostsView = new ManagePostsView(postRepository);

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine("\n Choose an option:");
            Console.WriteLine("1. Add user");
            Console.WriteLine("2. Edit user");
            Console.WriteLine("3. Delete user");
            Console.WriteLine("4. Exit");
            Console.Write("Enter choice: ");
            var input = Console.ReadLine();

            if (input == "4")
            {
                break;
            }
            
            switch (input)
            {
                case "1":
                    Console.WriteLine("Enter username: ");
                    var username = Console.ReadLine();
                    Console.WriteLine("Enter password: ");
                    var password = Console.ReadLine();

                    await manageUsersView.AddUser(new User(username, password));
                    Console.WriteLine($"Added user: {username}");
                    break;

                case "2":
                    
                    break;

                case "3":
                    
                    break;
                case "4":
                    
                    break;
                
            }
        }

        //Console.WriteLine(); denne med prints og bagefter cases
        //1. new createuserview(userrepository)
        //det skal stå i en public void Start()
        await Task.CompletedTask;
    }
}