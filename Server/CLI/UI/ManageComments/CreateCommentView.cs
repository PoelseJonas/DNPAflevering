using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task AddComment(Comment comment)
    {
        if (comment == null)
        {
            throw new ArgumentNullException("comment cannot be null");
        }
        
    }
}