using Entities;

namespace CLI;
using CLI.UI;
using FileRepositories;
using RepositoryContracts;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting CLI app...");
    IUserRepository userRepository = new UserFileRepository();
    ICommentRepository commentRepository = new CommentFileRepository(); //Her var den gamle CommentInMemoryRepository
    IPostRepository postRepository = new PostFileRepository();
    
    CliApp cliApp = new CliApp(postRepository, userRepository, commentRepository);
    await cliApp.StartAsync();
    
    }
}