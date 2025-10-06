using RepositoryContracts;
using FileRepositories;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container. Hvis der er andre som LikeRepository, skal de også på her.
        builder.Services.AddScoped<IPostRepository, PostFileRepository>();
        builder.Services.AddScoped<IUserRepository, UserFileRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
        
        
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}