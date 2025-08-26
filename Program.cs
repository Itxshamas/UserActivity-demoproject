using DemoProje.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//  Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⚡ Authentication & Authorization abhi custom implement hoga (JWT ya manual login logic ke zariye)
// app.UseAuthentication();   <-- ye tab chahiye jab tum JWT/Auth system lagao
// app.UseAuthorization();

app.MapControllers();

app.Run();
