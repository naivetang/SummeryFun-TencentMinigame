using System;

namespace ETModel
{
    public static class LoginHelper
    {
        public static async ETVoid OnLoginAsync(Session session, string account, string password)
        {
            try
            {
                LoginRsp rsp = (LoginRsp) await session.Call(new LoginReq() { Account = account, Password = password });
                if (rsp.Error == (int)LoginRsp.Types.ErrorCode.Succeed)
                {
                    Log.Info("login succeed");
                }
                else if (rsp.Error == (int)LoginRsp.Types.ErrorCode.LoginNotRegistered)
                {
                    Log.Warning("account not exist, please register new account");
                }
                else if (rsp.Error == (int)LoginRsp.Types.ErrorCode.LoginPasswordWrong)
                {
                    Log.Warning("password wrong");
                }
                {
                    
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