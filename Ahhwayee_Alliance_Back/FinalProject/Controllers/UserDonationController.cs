using FinalProject.Models.Db;
using FinalProject.Models.Dtos.RequestPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static FinalProject.Utils.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDonationController : ControllerBase
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
        public UserDonationController(
            DonationContext donationContext,
            IConfiguration configuration)
        {
            _donationContext = donationContext;
            _configuration = configuration;
        }

        /// <summary>
        /// 會員排行榜資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Rank")]
        [AllowAnonymous]
        public ActionResult Rank()
        {
            //月排行
            var monthRank = _donationContext.Donations
                .Where(donation => donation.DonationTime.Year == DateTime.Now.Year && donation.DonationTime.Month == DateTime.Now.Month)
                .GroupBy(group => group.UserId)
                .Select(group => new
                {
                    group.Key,
                    MonthDonation = group.Sum(points => points.DonationPoints)
                })
                .OrderByDescending(group => group.MonthDonation)
                .Join(_donationContext.UserInformations, donation => donation.Key, user => user.UserId, (donation, user) => new
                {
                    Account = user.Account,
                    MonthDonation = donation.MonthDonation
                });

            //年排行
            var yearRank = _donationContext.Donations
                .Where(donation => donation.DonationTime.Year == DateTime.Now.Year)
                .GroupBy(group => group.UserId)
                .Select(group => new
                {
                    group.Key,
                    YearDonation = group.Sum(points => points.DonationPoints)
                })
                .OrderByDescending(group => group.YearDonation)
                .Join(_donationContext.UserInformations, donation => donation.Key, user => user.UserId, (donation, user) => new
                {
                    Account = user.Account,
                    YearDonation = donation.YearDonation
                })
                .ToList();

            //歷史排行
            var soFarRank = _donationContext.Donations
                .GroupBy(group => group.UserId)
                .Select(group => new
                {
                    group.Key,
                    SoFarDonation = group.Sum(points => points.DonationPoints)
                })
                .OrderByDescending(group => group.SoFarDonation)
                .Join(_donationContext.UserInformations, donation => donation.Key, user => user.UserId, (donation, user) => new
                {
                    Account = user.Account,
                    SoFarDonation = donation.SoFarDonation
                })
                .ToList();
            return Ok(new
            {
                MonthDonation = monthRank,
                YearDonation = yearRank,
                SoFarDonation = soFarRank
            });
        }

        /// <summary>
        /// 近一周捐款資料API
        /// 放在網頁跑馬燈的地方
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Marquee")]
        [AllowAnonymous]
        public ActionResult Marquee()
        {
            var result = _donationContext.Donations
                .Where(donation => DateTime.Compare(donation.DonationTime, DateTime.Now) >= -7)
                .Join(_donationContext.UserInformations, donation => donation.UserId, user => user.UserId, (donation, user) => new
                {
                    Account = user.Account,
                    Date = donation.DonationTime,
                    DonationPoints = donation.PurposeId,
                    ShelterId = donation.ShelterId,
                    PurposeId = donation.PurposeId,
                })
                .ToList();
            return Ok(result);
        }

        /// <summary>
        /// 個人資料API
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="bearerToken">有效token</param>
        /// <returns></returns>
        [HttpGet]
        [Route("MyData")]
        public ActionResult MyData([FromQuery] string account, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            //TODO:每次給一部份資料
            //DonationData所有每一筆捐款
            var userAllDonation = _donationContext.UserInformations
                .Where(user => user.Account == account)
                .Join(_donationContext.Donations
                , user => user.UserId
                , donation => donation.UserId
                , (user, donation) => new
                {
                    DonationId = donation.DonationId,
                    UserId = donation.UserId,
                    DonationPoints = donation.DonationPoints,
                    DonationTime = donation.DonationTime,
                    ShelterId = donation.ShelterId,
                    PurposeId = donation.PurposeId
                }).ToList();

            var monthDonation = userAllDonation
                .Where(month => month.DonationTime.Year == DateTime.Now.Year && month.DonationTime.Month == DateTime.Now.Month)
                .Sum(group => group.DonationPoints);

            var yearDonation = userAllDonation
                .Where(month => month.DonationTime.Year == DateTime.Now.Year)
                .Sum(group => group.DonationPoints);

            var soForDonation = userAllDonation
                .Sum(group => group.DonationPoints);

            return Ok(new
            {
                UserData = user,
                AllDonation = userAllDonation,
                MonthDonation = monthDonation,
                YearDonation = yearDonation,
                SoFarDonation = soForDonation
            });

        }

        /// <summary>
        /// 捐點API
        /// </summary>
        /// <param name="donationSubmitPost">捐點需填入資料</param>
        /// <param name="bearerToken">Token</param>
        /// <returns></returns>
        [HttpPut]
        [Route("DonatePoints")]
        public ActionResult DonatePoints([FromBody] DonationSubmitPost donationSubmitPost, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, donationSubmitPost.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }

            if (user.RemainingPoints - donationSubmitPost.DonationPoints >= 0)
            {
                Donation newDonation = new Donation()
                {
                    DonationId = _donationContext.Donations.Count() + 1,
                    UserId = user.UserId,
                    DonationTime = DateTime.Now,
                    DonationPoints = donationSubmitPost.DonationPoints,
                    ShelterId = donationSubmitPost.ShelterId,
                    PurposeId = donationSubmitPost.PurposeId
                };
                try
                {
                    _donationContext.Donations.Add(newDonation);
                    user.RemainingPoints -= donationSubmitPost.DonationPoints;
                    _donationContext.SaveChanges();
                    return Ok(new { msg = "捐點成功"});
                }
                catch
                {
                    return Ok(new { msg = "捐點失敗" });
                }
            }
            else
            {
                return Ok(new { msg = "點數不足" });
            }
        }

        /// <summary>
        /// 付款金額成功轉為點數，並更新DB
        /// </summary>
        /// <param name="payInfo">付款資訊</param>
        /// <returns></returns>
        [HttpPut]
        [Route("MoneyToPoint")]
        public ActionResult MoneyToPoint([FromBody] PayInfoPost payInfo, [FromHeader(Name = "Authorization")] string bearerToken)
        {
            if (!CheckValidToken(_donationContext, payInfo.Account, bearerToken, out UserInformation user))
            {
                return Ok(new { msg = "帳號錯誤或Token失效，請重新登入" });
            }
            int point = payInfo.Money * EXCHANGE_RATE;

            try
            {
                user.RemainingPoints = user.RemainingPoints + point;
                _donationContext.SaveChanges();
                return Ok(new { msg = "儲值成功" });
            }
            catch
            {
                return Ok(new { msg = "儲值失敗" });
            }
        }
    }
}
