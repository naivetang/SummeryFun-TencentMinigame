using System;

namespace ETModel
{
    public static class RegisterHelper
    {
        public static async ETVoid OnRegisterAsync(Session session, string account, string password)
        {
            try
            {
                RegisterRsp rsp = (RegisterRsp) await session.Call(new RegisterReq() { Account = account, Password = password });
                if (rsp.Error == (int)RegisterRsp.Types.ErrorCode.Succeed)
                {
                    Log.Info("register succeed");
                } else if (rsp.Error == (int) RegisterRsp.Types.ErrorCode.AccountAlreadyExist)
                {
                    Log.Info("account already exist");
                }
                // TODO 可选性地记录下 gid(不过当前协议没有使用 gid)
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        } 
    }
}