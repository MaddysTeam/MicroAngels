using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public  class StaticKeys
    {
        public static string MessageTypeId_Subscribe = "1bc24af4-6c23-4655-adfd-d338f816fe26";
        public static string MessageTypeId_Notify = "495407af-6e94-48dd-94f1-e6789bd1fd93";
        public static string MessageTypeId_Announce = "f80d9be1-1435-4fc3-9561-6bd4cf3ad8e0";

        public static string MessageStatusId_Consumed= "";
        public static string MessageStatusId_Deleted = "";

        public static string UserMessageStatusId_Waiting = "";
        public static string UserMessageStatusId_Received = "";
        public static string UserMessageStatusId_Deleted = "";
    }
}
