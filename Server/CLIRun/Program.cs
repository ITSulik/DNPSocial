using CLIRun.UserInterface;
using FileStorage;
using RepositoryContracts;


Console.WriteLine("Starting Application...");
IUserRepository userRepository = new UserFileStorage();
IPostRepository postRepository = new PostFileStorage();
ICommentRepository commentRepository = new CommentFileStorage();

CliAppUi cliAppUi = new CliAppUi(userRepository, postRepository, commentRepository);
await cliAppUi.StartAsync();