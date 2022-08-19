using Microsoft.AspNetCore.Identity;

namespace TVGuide.Models;

// Add profile data for application users by adding properties to the TVGuideUser class
public class TVGuideUser : IdentityUser
{
    [PersonalData] public string Keywords { get; set; }
    [PersonalData] public List<FavoriteChannel> favoriteChannels { get; set; }
}

