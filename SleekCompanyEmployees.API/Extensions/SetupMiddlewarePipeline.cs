﻿using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace SleekCompanyEmployees.API.Extensions;

public static class SetupMiddlewarePipeline
{
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
        // Configure the pipeline
        app.UseSwagger();
        app.UseSwaggerUI(s =>
        {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "Sleek DevOps Master Series API v1");
            s.SwaggerEndpoint("/swagger/v2/swagger.json", "Sleek DevOps Master Series API v2");
        });
        using (var serviceScope = app.Services.CreateScope())
        {
            using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<RepositoryContext>())
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                    dbContext.Database.Migrate();
            }
        }


        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseRateLimiter();
        app.UseCors("CorsPolicy");

        app.UseResponseCaching();
        app.UseOutputCache();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        return app;
    }
}
