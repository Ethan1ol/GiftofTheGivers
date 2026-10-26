using GiftOfTheGiversPrototype.Data;
using GiftOfTheGiversPrototype.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGiversPrototype.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("GiftOfTheGiversDb"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<PrototypeStore>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var role in new[] { "Employee", "Donor" })
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    var employee = await userManager.FindByEmailAsync("employee@givers.org");
    if (employee == null)
    {
        employee = new ApplicationUser { UserName = "employee@givers.org", Email = "employee@givers.org", EmailConfirmed = true };
        await userManager.CreateAsync(employee, "Password1");
        await userManager.AddToRoleAsync(employee, "Employee");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapGet("/certificate/{id:int}", (int id, PrototypeStore store) =>
{
    var donation = store.Donations.FirstOrDefault(x => x.Id == id);
    if (donation == null) return Results.NotFound();

    var html = $@"<!doctype html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <title>Tax Certificate</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"" rel=""stylesheet"" />
    <style>
        body {{
            background: #f5f5f5;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 2rem 1rem;
        }}
        .certificate-card {{
            max-width: 760px;
            width: 100%;
            border: 1px solid #dfe3e8;
            border-radius: 1rem;
            box-shadow: 0 0.5rem 1.25rem rgba(0,0,0,0.08);
            background: #fff;
            padding: 2.5rem;
        }}
        .brand {{
            color: #green;
            font-weight: 800;
            letter-spacing: 0.02em;
        }}
        @media print {{
            body {{ background: white; }}
            .no-print {{ display: none !important; }}
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""certificate-card"">
            <div class=""text-center mb-4"">
                <h1 class=""brand mb-2"">G Gift of the Givers</h1>
                <h2 class=""fw-bold mb-3"">Tax Certificate</h2>
            </div>

            <div class=""alert alert-light border rounded-4 mb-4"">
                This prototype certificate confirms a symbolic donation record.
            </div>

            <dl class=""row fs-5"">
                <dt class=""col-sm-4"">Certificate</dt>
                <dd class=""col-sm-8 fw-semibold"">GOTG-{donation.Id:0000}</dd>

                <dt class=""col-sm-4"">Amount</dt>
                <dd class=""col-sm-8"">{donation.Amount:0.00} {donation.Currency}</dd>

                <dt class=""col-sm-4"">Type</dt>
                <dd class=""col-sm-8"">{donation.Type}</dd>

                <dt class=""col-sm-4"">Date</dt>
                <dd class=""col-sm-8"">{donation.CreatedAt:dd MMMM yyyy}</dd>
            </dl>

            <div class=""d-flex justify-content-center gap-3 mt-4 no-print"">
                <button class=""btn btn-primary btn-lg"" onclick=""window.print()"">Print / Save as PDF</button>
                <a class=""btn btn-outline-secondary btn-lg"" href=""/"">Back home</a>
            </div>
        </div>
    </div>
</body>
</html>";
    return Results.Content(html, "text/html");
});

app.Run();
