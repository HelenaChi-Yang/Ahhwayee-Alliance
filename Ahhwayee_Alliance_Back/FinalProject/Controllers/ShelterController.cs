using FinalProject.Models.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterController : ControllerBase
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
        public ShelterController(
            DonationContext donationContext,
            IConfiguration configuration)
        {
            _donationContext = donationContext;
            _configuration = configuration;
        }

        /// <summary>
        /// 所有收容所資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<ShelterInformation> Get()
        {
            List<ShelterInformation> allData = _donationContext.ShelterInformations.Select(allData => allData).ToList();
            return allData;
        }

        /// <summary>
        /// 收容所依據捐款目的之收入排名，倒敘
        /// </summary>
        /// <param name="purposeId">捐款目的id</param>
        /// <returns></returns>
        [HttpGet("ShelterRank/{purposeId}")]
        [AllowAnonymous]
        public ActionResult GetShelterRank(int purposeId)
        {
            //當月目前收到(某目的)捐贈點數
            var donationMonthRank = _donationContext.Donations
            .Where(donation => donation.DonationTime.Year == DateTime.Now.Year && donation.DonationTime.Month == DateTime.Now.Month)
            .Where(TotalOrPurpose => purposeId != 0 ? TotalOrPurpose.PurposeId == purposeId : true)
            .GroupBy(group => new { group.ShelterId })
            .Select(donation => new
            {
                shelterId = donation.Key.ShelterId,
                purposePoint = donation.Sum(PurposePoint => PurposePoint.DonationPoints)
            })
            .ToList();

            //收容所當月所需(某目的)點數
            var spendOnPurpose = _donationContext.EvaluateCosts
                .Where(TotalOrPurpose => purposeId != 0 ? TotalOrPurpose.PurposeId == purposeId : true)
                .Select(spend => new
                {
                    shelterId = spend.ShelterId,
                    howMuch = spend.AverageCostOnOnePetPerMonth
                })
                .ToList();

            //收容所所需(某目的)點數、收到(某目的)捐贈點數(左交集spendOnPurpose不漏掉沒被捐贈過的)
            var shelterNeedPoints = spendOnPurpose.GroupJoin(
                donationMonthRank,
                spendOnPurpose => spendOnPurpose.shelterId,
                donationMonthRank => donationMonthRank.shelterId,
                (spendOnPurpose, donationMonthRank) => new
                {
                    shelterId = spendOnPurpose.shelterId,
                    getPoints = donationMonthRank.Select(getPoint => getPoint.purposePoint).DefaultIfEmpty().Max(),
                    needPoints = spendOnPurpose.howMuch
                })
                .ToList();

            //收容所名稱與ID
            var shelterName = _donationContext.ShelterInformations
                .Select(allData => new
                {
                    shelterId = allData.ShelterId,
                    shelterName = allData.ShelterName,
                    howManyPetInShelter = allData.RealNumber,
                    Address = allData.Address,
                    ShelterPhoneNumber = allData.ShelterPhoneNumber,
                    ContainMaxNumber = allData.ContainMaxNumber,
                    RealNumber = allData.RealNumber,
                    ShelterImgUrl = allData.ShelterImgUrl,
                    ShelterImgName = allData.ShelterImgName,
                    UpdateTime = allData.UpdateTime
                })
                .ToList();

            ///收容所名稱與所需、收到點數、當purposeId == 0 時，得groupBy
            var shelterRankForOnePurpose = shelterNeedPoints.Join(
                shelterName,
                shelterNeedPoints => shelterNeedPoints.shelterId,
                shelterName => shelterName.shelterId,
                (shelterNeedPoints, shelterName) => new
                {
                    shelterId = shelterName.shelterId,
                    shelterName = shelterName.shelterName,
                    shelterNeedPoints = shelterNeedPoints.needPoints * shelterName.howManyPetInShelter,
                    shelterGetPoints = shelterNeedPoints.getPoints,
                    Address = shelterName.Address,
                    ShelterPhoneNumber = shelterName.ShelterPhoneNumber,
                    ContainMaxNumber = shelterName.ContainMaxNumber,
                    RealNumber = shelterName.RealNumber,
                    ShelterImgUrl = shelterName.ShelterImgUrl,
                    ShelterImgName = shelterName.ShelterImgName,
                    UpdateTime = shelterName.UpdateTime
                })
                .GroupBy(TotalOrPurpose => TotalOrPurpose.shelterId)
                .Select(result => new
                {
                    shelterId = result.Max(shelter => shelter.shelterId),
                    shelterName = result.Max(shelter => shelter.shelterName),
                    shelterNeedPoints = result.Sum(totalNeedPoint => totalNeedPoint.shelterNeedPoints),
                    shelterGetPoints = result.Max(getPointFromCustomer => getPointFromCustomer.shelterGetPoints),
                    Address = result.Max(shelter => shelter.Address),
                    ShelterPhoneNumber = result.Max(shelter => shelter.ShelterPhoneNumber),
                    ContainMaxNumber = result.Max(shelter => shelter.ContainMaxNumber),
                    RealNumber = result.Max(shelter => shelter.RealNumber),
                    ShelterImgUrl = result.Max(shelter => shelter.ShelterImgUrl),
                    ShelterImgName = result.Max(shelter => shelter.ShelterImgName),
                    UpdateTime = result.Max(shelter => shelter.UpdateTime)
                })
                .OrderBy(shelterRank => 10000 * shelterRank.shelterGetPoints / shelterRank.shelterNeedPoints)
                .ToList();

            return Ok(shelterRankForOnePurpose);
        } 
    }
}