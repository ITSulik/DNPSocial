using EfcRepositories;
using FileStorage;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//
// builder.Services.AddScoped<IPostRepository, PostFileStorage>();
// builder.Services.AddScoped<ICommentRepository, CommentFileStorage>();
// builder.Services.AddScoped<IUserRepository, UserFileStorage>();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IPostRepository, EfcPostRepo>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepo>();
builder.Services.AddScoped<IUserRepository, EfcUserRepo>();



var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();