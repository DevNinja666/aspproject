using Microsoft.AspNetCore.Builder;

namespace Invoicer.Extensions;

public static class ApplicationExtensions
{
    public static void UseProjectApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}
Т
