using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private IPostRepository postRepository;
    private IUserRepository userRepository;
    private ICommentRepository commentRepository;
    public CliApp(IUserRepository  userRepository, ICommentRepository  commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        // Placeholder idk
        Console.WriteLine("CLI App started!");
        await Task.CompletedTask;
    }
}