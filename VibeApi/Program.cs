using VibeApi.Services;
using VibeApi.Services.External;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IExternalPostService, ExternalPostService>();
builder.Services.AddScoped<IExternalUserService, ExternalUserService>();
builder.Services.AddScoped<IExternalAlbumService, ExternalAlbumService>();
builder.Services.AddScoped<IExternalCommentService, ExternalCommentService>();
builder.Services.AddScoped<IExternalPhotoService, ExternalPhotoService>();
builder.Services.AddScoped<IExternalTodoService, ExternalTodoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
