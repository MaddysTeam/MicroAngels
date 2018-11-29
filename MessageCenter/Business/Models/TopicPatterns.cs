using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public static class TopicPatterns
    {
       public static string[] FollowPatterns =
       {
            "follow",
            "follow.{serviceId}",
            "unfollow"
        };
    }

}
