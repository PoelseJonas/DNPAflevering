using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;

    public ManagePostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
}