using FinalProject.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FinalProject.Utils
{
    public static class Global
    {
        /// <summary>
        /// 空字串用此取代，避免誤刪
        /// </summary>
        public const char SPACE = ' ';

        /// <summary>
        /// 現金更換成點數的倍率
        /// </summary>
        public const int EXCHANGE_RATE = 30;

        /// <summary>
        /// 獲得隨機驗證碼的方式
        /// </summary>
        /// <param name="length">驗證碼長度</param>
        /// <returns></returns>
        public static string GetGuid(int length)
        {
            return Guid.NewGuid().ToString().Substring(0, length);
        }

        /// <summary>
        /// 確認token是否有效
        /// 為了讓最新的Token才有效，所以做這個Method來判斷
        /// 不過喪失了Token不需要重新找資料庫來判斷是否登入的特性，可以改成RefreshToken或是用基本的Session驗證
        /// </summary>
        /// <param name="donationContext"></param>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckValidToken(DonationContext donationContext, string account, string token, out UserInformation user)
        {
            user = donationContext.UserInformations
                .Where(user => user.Account == account && user.ValidToken == token)
                .SingleOrDefault();
            return user != null;
        }
    }
}
