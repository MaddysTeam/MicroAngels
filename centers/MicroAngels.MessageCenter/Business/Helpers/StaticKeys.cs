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

        public static string MessageStatusId_Consumed= "ff93d68f-8557-468d-bff3-832ee9cb0b49";
        public static string MessageStatusId_Deleted = "4d2eac28-e35c-4a2b-914e-a9da79bd547f";

        public static string UserMessageStatusId_Waiting = "4df53597-59af-460f-b106-8ef3abdce1d9";
        public static string UserMessageStatusId_Received = "f98edcbe-3895-4e30-946a-4a7a45eb5a89";
        public static string UserMessageStatusId_Deleted = "9a128248-2f2e-43b8-91df-82d950a4e302";
    }
}
