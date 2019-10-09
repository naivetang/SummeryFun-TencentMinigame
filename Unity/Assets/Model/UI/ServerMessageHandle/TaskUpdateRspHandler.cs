using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class TaskUpdateRspHandler: AMHandler<TaskUpdateRsp>
    {
        protected override async ETTask Run(Session session, TaskUpdateRsp message)
        {
            
            if (message.Error == (int)TaskUpdateRsp.Types.ErrorCode.Succeed)
            {
                Log.Info("更新进度成功");
            }
            else
            {
                Log.Info("更新进度失败：" + message.Error);
            }

            await ETTask.CompletedTask;
        }
    }
   
}
