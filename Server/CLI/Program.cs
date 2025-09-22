namespace CLI;
using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting CLI app...");
    IUserRepository userRepository = new UserInMemoryRepository();
    ICommentRepository commentRepository = new CommentInMemoryRepository();
    IPostRepository postRepository = new PostInMemoryRepository();
    
    CliApp cliApp = new CliApp(postRepository, userRepository, commentRepository);
    await cliApp.StartAsync();
    
    }
}