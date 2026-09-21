using CLIRun.UserInterface;
using InMemoryRepositories;
using RepositoryContracts;


Console.WriteLine("Starting Application...");
IUserRepository userRepository = new UserInMemoryRepo();
IPostRepository postRepository = new PostInMemoryRepo();
ICommentRepository commentRepository = new CommentInMemoryRepo();

CliAppUi cliAppUi = new CliAppUi(userRepository, postRepository, commentRepository);
await cliAppUi.StartAsync();