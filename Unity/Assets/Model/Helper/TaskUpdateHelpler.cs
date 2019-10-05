using System;

namespace ETModel
{
    public static class TaskUpdateHelper
    {
        public static async ETVoid OnTaskUpdateAsync(Session session /* TODO 新获取任务列表，完成列表，新的位置 */)
        {
            try
            {
                TaskUpdateRsp rsp = (TaskUpdateRsp) await session.Call(new TaskUpdateReq() {PositionX = 100, PositionY = 200});
                if (rsp.Error == (int)TaskUpdateRsp.Types.ErrorCode.Succeed)
                {
                    Log.Info("task update succeed");
                } else 
                {
                    Log.Warning("unkown error");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        } 
    }
}