using FinalProject.Models.Db;
using FinalProject.Models.Dtos.RequestPost;
using static FinalProject.Utils.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 注入dbContext
        /// </summary>
        /// <returns></returns>
        private readonly DonationContext _donationContext;

        /// <summary>
        /// 注入Config設定
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 建構子注入
        /// </summary>
        /// <param name="donationContext"></param>
        /// <param name="configuration"></param>
        public AuthController(
            DonationContext donationContext,
            IConfiguration configuration)
        {
            _donationContext = donationContext;
            _configuration = configuration;
        }

        /// <summary>
        /// 寄送驗證信至信箱
        /// 前端註冊或更換信箱時需要判斷驗證碼是否正確
        /// </summary>
        /// <param name="emailAddress">Email地址</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendEmail")]
        [AllowAnonymous]
        public ActionResult SendEmail([FromBody] SendEmailPost sendEmailPost)
        {
            string account = sendEmailPost.Account;
            string emailAddress = sendEmailPost.EmailAddress;

            //已註冊就不動作
            //if (CheckAccountExist(account))
            //{
            //    return Ok(new { msg = "帳號已存在" });
            //}

            try
            {
                //寄信並儲存驗證碼
                SendMailByGmail(emailAddress , out string verifyCode);

                //若有認證階段的account就先刪掉
                var tmpVerifyForm = _donationContext.VerifyForms.Where(verifyForm => verifyForm.Account == account).SingleOrDefault();
                if(tmpVerifyForm != null)
                {
                    _donationContext.VerifyForms.Remove(tmpVerifyForm);
                }
                //儲存新的待認證資訊
                _donationContext.VerifyForms.Add(new VerifyForm()
                {
                    Account = account,
                    VerifyCode = verifyCode
                });
                _donationContext.SaveChanges();

                //寄信
                return Ok(new { msg = "驗證碼已寄出" });
            }
            catch (Exception exception)
            {
                return Ok(new { msg = "寄信失敗" + exception.Message });
            }
        }

        /// <summary>
        /// 註冊會員資訊，寫入DB
        /// </summary>
        /// <param name="registerPost">註冊需填入資料</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public ActionResult Register([FromBody] RegisterPost registerPost)
        {
            //已註冊就不動作
            if (CheckAccountExist(registerPost.Account))
            {
                return Ok(new { msg = "帳號已存在" });
            }
            
            //確認驗證碼是否正確
            if (!CheckVerifyCode(registerPost.Account, registerPost.VerifyCode, out VerifyForm verifyForm))
            {
                return Ok(new{msg = "驗證碼錯誤"});
            }



            try
            {
                UserInformation userInformation = new UserInformation
                {
                    UserId = _donationContext.UserInformations.Count() + 1,
                    Account = registerPost.Account,
                    Password = registerPost.Password,
                    UserName = registerPost.UserName,
                    EmailAddress = registerPost.EmailAddress,
                    RemainingPoints = 0
                };
                //寫入userDB
                _donationContext.UserInformations.Add(userInformation);
                //刪除待認證帳號
                _donationContext.VerifyForms.Remove(verifyForm);
                _donationContext.SaveChanges();

                return Ok(new { msg = "註冊成功" });
            }
            catch(Exception exception)
            {
                return Ok(new { msg = _donationContext.UserInformations.Count() +"失敗，註冊會員資訊格式不符或userId碰撞，或是刪除verifyForm失敗\n" + exception.Message });
            }

        }

        /// <summary>
        /// 登入，更新token
        /// </summary>
        /// <param name="loginPost">登入需填入資料</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginPost loginPost)
        {
            var user = _donationContext.UserInformations
                .Where(dbUser => dbUser.Account == loginPost.Account && dbUser.Password == loginPost.Password)
                .SingleOrDefault();
            if (user == null)
            {
                return Ok(new { msg = "帳號密碼錯誤" });
            }
            else
            {
                //Token訊息包含Email, UserName
                //Token驗證
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
                };

                //TODO:可以設定角色
                //var role = from a in _donationContext;

                //用appsetting的key加密
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSettings:Key"]));

                var jwt = new JwtSecurityToken
                (
                    //Header
                    issuer: _configuration["TokenSettings:Issuer"],
                    audience: _configuration["TokenSettings:Audience"],

                    //Payload
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),

                    //SignatureKey
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                //TODO:又將token存回DB其實就失去token的意義
                //將new Token存入DB
                try
                {
                    user.ValidToken = "Bearer" + " " + token;
                    _donationContext.SaveChanges();
                    return Ok(new { 
                        msg = "登入成功",
                        Token = token });
                }
                catch
                {
                    return Ok(new { msg = "登入失敗"});
                }
            }
        }

        /// <summary>
        /// 將有效的token刪除
        /// </summary>
        /// <param name="account">要登出的帳號</param>
        /// <param name="bearerToken">有效Token</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearToken")]
        public ActionResult ClearToken([FromBody] string account, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            try
            {
                user.ValidToken = null;
                _donationContext.SaveChanges();
                return Ok(new { msg = "Token已清除"});
            }
            catch 
            {
                return Ok(new { msg = "Token清除失敗??"});
            }
        }

        /// <summary>
        /// 更新account資料
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="updateAccountPost">更新帳號需填入資訊</param>
        /// <param name="bearerToken">有效Token</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/Account")]
        public ActionResult UpdateAccount([FromBody] UpdateAccountPost updateAccountPost, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, updateAccountPost.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            string originAccount = updateAccountPost.Account;
            string newAccount = updateAccountPost.NewAccount;
            var newUser = _donationContext.UserInformations.Where(user => user.Account == newAccount).SingleOrDefault();

            if(newUser != null)
            {
                return Ok(new { msg = "欲修改之帳號已有人使用"});
            }
            user.Account = newAccount;
            try
            {
                _donationContext.SaveChanges();
                return Ok(new { msg = "更新帳號成功" });
            }
            catch
            {
                return Ok(new { msg = "更新帳號失敗" });
            }
        }

        /// <summary>
        /// 更新使用者名稱
        /// </summary>
        /// <param name="updateUserNamePost">更新使用者名稱需填入資訊</param>
        /// <param name="bearerToken">有效Token</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/UserName")]
        public ActionResult UpdateUserName([FromBody] UpdateUserNamePost updateUserNamePost, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, updateUserNamePost.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            try
            {
                user.UserName = updateUserNamePost.NewUserName;
                _donationContext.SaveChanges();
                return Ok(new { msg = "更新使用者名稱成功" });
            }
            catch
            {
                return Ok(new { msg = "更新使用者名稱失敗" });
            }
        }

        /// <summary>
        /// 更新使用者密碼
        /// </summary>
        /// <param name="updatePassword">更新使用者密碼需填入資訊</param>
        /// <param name="bearerToken">有效Token</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/Password")]
        public ActionResult UpdatePassword([FromBody] UpdatePassword updatePassword, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, updatePassword.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            try
            {
                user.Password = updatePassword.NewPassword;
                _donationContext.SaveChanges();
                return Ok(new { msg = "更新密碼成功" });
            }
            catch
            {
                return Ok(new { msg = "更新密碼失敗" });
            }
        }

        /// <summary>
        /// 更新帳號
        /// </summary>
        /// <param name="updateEmailPost">更新帳號需填入資訊</param>
        /// <param name="bearerToken">有效Token</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/Email")]
        public ActionResult UpdateEmail([FromBody] UpdateEmailPost updateEmailPost, [FromHeader(Name = "Authorization")] string bearerToken)
        {

            if (!CheckValidToken(_donationContext, updateEmailPost.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            if(!CheckVerifyCode(updateEmailPost.Account, updateEmailPost.VerifyCode, out VerifyForm verifyForm))
            {
                return Ok(new { msg = "驗證碼錯誤" });
            }

            try
            {
                //更改新的Email
                user.EmailAddress = updateEmailPost.NewEmailAddress;
                //刪除待驗證資訊
                _donationContext.VerifyForms.Remove(verifyForm);
                _donationContext.SaveChanges();
                return Ok(new { msg = "更新使用者信箱成功" });
            }
            catch
            {
                return Ok(new { msg = "更新使用者信箱失敗" });
            }
        }

        /// <summary>
        /// 更新個人頭貼
        /// </summary>
        /// <param name="updateImageUrl">更新個人頭貼需填入資訊</param>
        /// <param name="bearerToken">有效Token</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/ImageUrl")]
        public ActionResult UpdateImageUrl([FromBody] UpdateImageUrl updateImageUrl, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, updateImageUrl.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            try
            {
                user.UserImageUrl = updateImageUrl.NewImageUrl;
                _donationContext.SaveChanges();
                return Ok(new { msg = "更新大頭貼成功" });
            }
            catch
            {
                return Ok(new { msg = "更新大頭貼失敗" });
            }
        }

        /// <summary>
        /// 前端可確認帳號是否存在
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsAccountExist")]
        [AllowAnonymous]
        public ActionResult IsAccountExist([FromQuery] string account)
        {
            if (CheckAccountExist(account))
            {
                return Ok(new { msg = "帳號存在"});
            }
            else
            {
                return BadRequest(new { msg = "帳號不存在"});
            }
        }

        /// <summary>
        /// 確認帳號Method
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns>是否存在:bool</returns>
        private bool CheckAccountExist(string account)
        {
            var user = _donationContext.UserInformations
                .Where(user => user.Account == account)
                .SingleOrDefault();
            return user != null;
        }

        /// <summary>
        /// 確認db中的驗證碼是否正確
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="verifyCode">驗證碼</param>
        /// <returns></returns>
        private bool CheckVerifyCode(string account, string verifyCode, out VerifyForm verifyForm)
        {
            verifyForm = _donationContext.VerifyForms
                .Where(user => user.Account == account && user.VerifyCode == verifyCode)
                .SingleOrDefault();
            return verifyForm != null;
        }

        /// <summary>
        /// 寄驗證信回傳驗證碼
        /// </summary>
        /// <param name="targetEmail">目標信箱</param>
        /// <param name="verifyCode">回傳自動生成驗證碼</param>
        private void SendMailByGmail(string targetEmail,out string verifyCode)
        {
            using (MailMessage msg = new MailMessage())
            {
                verifyCode = GetGuid(6);
                //寄送驗證信
                string Subject = "驗證信通知";
                string Body = $@"親愛的使用者您好：

您於 {DateTime.Now} 申請註冊 阿偉呷罷未 帳號。

請您輸入驗證碼：{verifyCode}
該驗證碼將於10分鐘後或註冊成功後失效。

如果您並沒有要求註冊帳號，請您忽略此封信件。

謝謝您！

此為系統自動通知信，請勿直接回信！

若您於垃圾信匣中收到此通知信，請將此封郵件勾選為不是垃圾信（移除垃圾信分類並移回至收件匣），以利於日後任何系統郵件通知。" ;
                //收件者，以逗號分隔不同收件者 ex "test@gmail.com,test2@gmail.com"
                msg.To.Add(targetEmail);
                msg.From = new MailAddress("ahhwayee666@gmail.com", "阿偉呷罷未", System.Text.Encoding.UTF8);
                //郵件標題 
                msg.Subject = Subject;
                //郵件標題編碼  
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                //郵件內容
                msg.Body = Body;
                msg.IsBodyHtml = true;
                //郵件內容編碼 
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.Priority = MailPriority.Normal;//郵件優先級 

                //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                #region 其它 Host
                /*
                 *  outlook.com smtp.live.com port:25
                 *  yahoo smtp.mail.yahoo.com.tw port:465
                */
                #endregion
                try
                {
                    using (SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        //設定你的帳號密碼
                        MySmtp.Credentials = new System.Net.NetworkCredential("ahhwayee666@gmail.com", "cmoney666");
                        //Gmial 的 smtp 使用 SSL
                        MySmtp.EnableSsl = true;
                        MySmtp.Send(msg);
                    }
                }
                catch (Exception e)
                {
                    //無法寄信
                    throw e;
                }
            }
        }
    }
}
