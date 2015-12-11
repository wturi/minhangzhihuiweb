using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinHangWisdomParkWeb
{
    public class ApplyHelp
    {

        Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();

        /// <summary>
        /// 插入申请单表 --要审核的
        /// </summary>
        /// <param name="ObjectID">对象ID</param>
        /// <param name="ApplyType">申请的类别</param>
        /// <param name="DateTimeNew">时间</param>
        /// <param name="NeedConfirmLevel">需要达到的审核人等级</param>
        /// <param name="ConfirmerAutoID">审核流ID</param>
        public void InsertApplyBill(string useid, string ObjectID, string ApplyType, string DateTimeNew, int NeedConfirmLevel, int ConfirmerAutoID)
        {
            try
            {

                NeedConfirmLevel = dal.mtConfirmFlow.Where(item => item.ConfirmerAutoID == ConfirmerAutoID).Count();

                Models.tbApplyBill applybills = new Models.tbApplyBill
                {
                    ApplyType = ApplyType,
                    ObjectID = ObjectID,
                    Updater = useid,
                    ApplyDate = DateTime.Parse(DateTimeNew),
                    StateType = 1
                };
                dal.tbApplyBill.Add(applybills);
                dal.SaveChanges();
                InsertConfirmStart(applybills.ApplyID, NeedConfirmLevel, ConfirmerAutoID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 插入申请表单 --直接通过的
        /// </summary>
        /// <param name="ObjectID"></param>
        /// <param name="ApplyType"></param>
        public void InsertApplyBill(string ObjectID, string ApplyType, string useid)
        {
            try
            {
                Models.tbApplyBill applybills = new Models.tbApplyBill
                {
                    ApplyType = ApplyType,
                    ObjectID = ObjectID,
                    Updater = useid,
                    ApplyDate = DateTime.Now,
                    StateType = 2
                };
                dal.tbApplyBill.Add(applybills);
                dal.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 插入ConfirmSrart表
        /// </summary>
        /// <param name="ApplyID">申请ID</param>
        /// <param name="NeedConfirmLevel">需要达到的审核人员等级</param>
        /// <param name="ConfirmerAutoID">审核流ID</param>
        public void InsertConfirmStart(int ApplyID, int NeedConfirmLevel, int ConfirmerAutoID)
        {
            var tem = dal.mtConfirmFlow.Where(m => m.ConfirmerAutoID == ConfirmerAutoID && m.ConfirmerLevelID == 1).FirstOrDefault();

            MinHangWisdomParkWeb.Models.tbConfirmState confirmstates = new Models.tbConfirmState
            {
                ApplyID = ApplyID,
                CurrConfirmLevel = 1,
                NeedConfirmLevel = NeedConfirmLevel,
                ConfirmerID = tem.UserId,
                ConfirmerAutoID = ConfirmerAutoID,
            };
            dal.tbConfirmState.Add(confirmstates);
            dal.SaveChanges();
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <param name="bools"></param>
        /// <param name="content"></param>
        public void UpdateComfirmStart(int ApplyID, int bools, string content)
        {
            Models.tbConfirmState ConfirmState = dal.tbConfirmState.FirstOrDefault(m => m.ApplyID == ApplyID);
            Models.tbApplyBill ApplyBill = dal.tbApplyBill.FirstOrDefault(m => m.ApplyID == ApplyID);
            if (bools == 0)
            {
                ConfirmState.ConfirmerID = dal.mtConfirmFlow.Where(m => m.ConfirmerAutoID == ConfirmState.ConfirmerAutoID && m.ConfirmerLevelID == ConfirmState.CurrConfirmLevel).FirstOrDefault().UserId;
                ConfirmState.CurrConfirmLevel = 0;
                ConfirmState.ConfirmeMemo = content;
                ApplyBill.StateType = 3;
                dal.SaveChanges();
            }
            else if (bools == 1)
            {
                if (ConfirmState.CurrConfirmLevel >= ConfirmState.NeedConfirmLevel)
                {
                    ConfirmState.ConfirmerID = dal.mtConfirmFlow.Where(m => m.ConfirmerAutoID == ConfirmState.ConfirmerAutoID && m.ConfirmerLevelID == ConfirmState.CurrConfirmLevel).FirstOrDefault().UserId;
                    ApplyBill.StateType = 2;
                    ConfirmState.ConfirmeMemo = content;
                }
                else
                {
                    ConfirmState.CurrConfirmLevel += 1;
                    ConfirmState.ConfirmerID = dal.mtConfirmFlow.Where(m => m.ConfirmerAutoID == ConfirmState.ConfirmerAutoID && m.ConfirmerLevelID == ConfirmState.CurrConfirmLevel).FirstOrDefault().UserId;
                    ConfirmState.ConfirmeMemo = content;
                }
                dal.SaveChanges();
            }

        }



    }
}