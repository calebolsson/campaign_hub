using campaign_hub.Components;
using campaign_hub.Services;
using campaign_hub.Services.AccountService;
using campaign_hub.Services.NewCharacterService;
using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ICharacterData, CharacterData>();
builder.Services.AddTransient<ICampaignData, CampaignData>();
builder.Services.AddTransient<ISessionData, SessionData>();
builder.Services.AddTransient<IAccountData, AccountData>();
builder.Services.AddTransient<IGuildData, GuildData>();
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<NewCharacterService>();
builder.Services.AddScoped<campaign_hub.Services.UIServices.ConfirmService.ConfirmService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
