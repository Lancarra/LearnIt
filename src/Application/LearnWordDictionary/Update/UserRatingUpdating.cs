using Domain;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.UpdatePermission;

public static class UserRatingUpdating
{
    public async static Task UpdateAchievement(User user, LearnContext context, CancellationToken cancellationToken)
    {
        var achievements = await context.Achievements.ToListAsync(cancellationToken);

        switch (user.Rating)
        {
           case 5:
               var achievement = achievements.FirstOrDefault(a => a.Name == "Beginner");
               if (achievement == null)
               {
                   var beginer = new Achievement()
                   {
                       Name = "Beginner",
                       Users = new List<User>() {user}
                   };
                   var achievementBeginner = await context.Achievements.AddAsync(beginer);
                   context.SaveChanges();
                   user.AchievementId = achievementBeginner.Entity.Id;
                   user.Achievement = beginer;
               }
               else
               {
                   user.Achievement = achievement;
                   user.AchievementId = achievement.Id;
               }
               try
               {
                   context.Users.Update(user);
                   context.SaveChanges();
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
                   throw;
               }
               break;
           
           case 10:
               var achievementAdvanced = achievements.FirstOrDefault(a => a.Name == "Advanced");
               if (achievementAdvanced == null)
               {
                   var advanced = new Achievement()
                   {
                       Name = "Advanced",
                       Users = new List<User>() {user}
                   };
                   var achievementAdvancedEntity = await context.Achievements.AddAsync(advanced);
                   context.SaveChanges();
                   user.AchievementId = achievementAdvancedEntity.Entity.Id;
                   user.Achievement = advanced;
               }
               else
               {
                   user.Achievement = achievementAdvanced;
                   user.AchievementId = achievementAdvanced.Id;
               }
               try
               {
                   context.Users.Update(user);
                   context.SaveChanges();
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
                   throw;
               }
               break;
           
           case 15:
              
               var achievementProfessional = achievements.FirstOrDefault(a => a.Name == "Professional");
               if (achievementProfessional == null)
               {
                   var professional = new Achievement()
                   {
                       Name = "Professional",
                       Users = new List<User>() {user}
                   };
                   var achievementProfessionalEntity = await context.Achievements.AddAsync(professional);
                   context.SaveChanges();
                   user.AchievementId = achievementProfessionalEntity.Entity.Id;
                   user.Achievement = professional;
               }
               else
               {
                   user.Achievement = achievementProfessional;
                   user.AchievementId = achievementProfessional.Id;
               }
               try
               {
                   context.Users.Update(user);
                   context.SaveChanges();
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
                   throw;
               }
               break;
           
           default:break;
        }
    }
}