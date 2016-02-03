using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Taoqi
{
    public class Msg
    {
        public static void ClientApply(Guid clientID, bool modeEdit = false)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", clientID);
            DataRow row = DAL.GetRow("vwTQClient_List", ht);
            if (row != null)
            {
                string companyName = row["C_ClientName"].ToString();
                ht.Clear();
                ht.Add("IS_ADMIN", 1);
                DataTable dt = DAL.GetTable("vwUSERS", ht);
                foreach (DataRow rowUsers in dt.Rows)
                {
                    Guid gTo = Sql.ToGuid(rowUsers["ID"]);
                    string strContent;
                    if (modeEdit)
                        strContent = string.Format("用户{0}资料已修改，请审核！", companyName);
                    else
                        strContent = string.Format("用户{0}资料已提交，请审核！", companyName);
                    SqlProcs.spTQMessageNew(gTo, strContent, "/Member/Client/");
                }
            }
        }

        public static void ClientAudit(Guid newBossID, Guid clientID, bool modePass = true)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_UserID", newBossID);
            ht.Add("C_ClientID", clientID);
            DataRow row = DAL.GetRow("vwTQAccount_List", ht);
            if (row != null)
            {
                string strTitle;
                string dateModify = row["DATE_MODIFIED"].ToString();
                string bossPhone = row["BossPhone"].ToString();
                string companyName = row["C_ClientName"].ToString();
                SMS sms = new SMS();
                if (modePass)
                {
                    sms.SendTemplate(bossPhone, "56925", new string[] { dateModify, companyName });
                    strTitle = string.Format("您{0}提交的{1}信息已经通过平台管理员审核！", dateModify, companyName);
                }
                else
                {
                    sms.SendTemplate(bossPhone, "56926", new string[] { dateModify, companyName });
                    strTitle = string.Format("您{0}提交的{1}信息审核未通过，请联系平台客服！", dateModify, companyName);
                }
                SqlProcs.spTQMessageNew(newBossID, strTitle, null);
            }
        }

        public static void UserApply(Guid accountID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", accountID);
            DataRow row = DAL.GetRow("vwTQAccount_List", ht);
            if (row != null)
            {
                Guid gTo = Sql.ToGuid(row["C_BossID"]);
                string to = row["BossPhone"].ToString();
                string phone = row["PHONE_MOBILE"].ToString();
                string realName = row["LAST_NAME"].ToString();
                string strTitle = string.Format("用户：{0}，手机号：{1}申请加入您的公司，请审核！", phone, realName);

                SMS sms = new SMS();
                sms.SendTemplate(to, "57407", new string[] { phone, realName });
                SqlProcs.spTQMessageNew(gTo, strTitle, "/Member/Account/");
            }
        }

        public static void BossAudit(Guid gID, bool mode = false)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", gID);
            DataRow row = DAL.GetRow("vwTQAccount_List", ht);
            if (row != null)
            {
                string strTitle;
                string phone = row["PHONE_MOBILE"].ToString();
                string company = row["C_ClientName"].ToString();
                string bossName = row["BossName"].ToString();
                string bossPhone = row["BossPhone"].ToString();
                SMS sms = new SMS();
                if (mode)
                {
                    sms.SendTemplate(phone, "56929", new string[] { company, bossName, bossPhone });
                    strTitle = string.Format("您提交的{0}的账户审核已通过，如有疑问请联系{1}，手机号：{2}，或平台客服", company, bossName, bossPhone);
                }
                else
                {
                    sms.SendTemplate(phone, "56930", new string[] { company, bossName, bossPhone });
                    strTitle = string.Format("您提交的{0}的账户审核未通过审核，如有疑问请联系{1}，手机号：{2}", company, bossName, bossPhone);
                }
                SqlProcs.spTQMessageNew(gID, strTitle, null);
            }
        }

        public static void Order(Guid orderID, ref string SN)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderID);
            DataRow row = DAL.GetRow("vwTQOrder_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                string sellerPhone = row["SellerPhone"].ToString();
                SN = row["SN"].ToString();
                string strTitle = string.Format("您发布的气源有买家下单，请您及时响应！单号{0}", SN);
                SMS sms = new SMS();
                sms.SendTemplate(sellerPhone, "56931", new string[] { SN });
                SqlProcs.spTQMessageNew(sellerID, strTitle, "/member/OrderSell/");
            }
        }

        public static void OrderCancel(Guid orderID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderID);
            DataRow row = DAL.GetRow("vwTQOrder_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                string sellerPhone = row["SellerPhone"].ToString();
                string SN = row["SN"].ToString();
                string strTitle = string.Format("买家取消订单！单号{0}", SN);
                SqlProcs.spTQMessageNew(sellerID, strTitle, null);
            }
        }

        public static void ModifyPrice(Guid orderDetailID, float newPrice)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderDetailID);
            DataRow row = DAL.GetRow("vwTQOrderDetail_List", ht);
            if (row != null)
            {
                Guid buyerID = Sql.ToGuid(row["BuyerID"]);
                string seller = row["Seller"].ToString();
                string SN = row["SN"].ToString();
                string price = row["C_Price"].ToString();
                string strTitle = string.Format("您与{0}的订单（编号{1}）卖方价格已经由{2}元/吨调整至{3}元/吨，请您及时查看！", seller, SN, price, newPrice);
                SqlProcs.spTQMessageNew(buyerID, strTitle, "/member/Order/");
            }
        }

        public static void AcceptPrice(Guid orderDetailID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderDetailID);
            DataRow row = DAL.GetRow("vwTQOrderDetail_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                string SN = row["SN"].ToString();
                string buyer = row["Buyer"].ToString();
                string strTitle = string.Format("您的订单（编号{0}）{1}已经确定价格，请您及时筹备发货。", SN, buyer);
                SqlProcs.spTQMessageNew(sellerID, strTitle, "/member/OrderSell/");
            }
        }

        public static void DispatchCar(Guid orderDetailID, ref string msgContent)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderDetailID);
            DataRow row = DAL.GetRow("vwTQOrderDetail_List", ht);
            if (row != null)
            {
                Guid driverID;
                Guid.TryParse(row["DriverID"].ToString(), out driverID);
                string driverTel = row["C_Tel"].ToString();
                string seller = row["Seller"].ToString();
                string driverName = row["C_Driver"].ToString();
                string arriveTime = row["C_ArriveTime"].ToString();
                string stationAddress = row["FullAddress"].ToString();
                string userTypeName = row["UserTypeName"].ToString();
                string buyer = row["Buyer"].ToString();
                string contactName = row["C_ContactName"].ToString();
                string contactTel = row["ContactTel"].ToString();
                string sellerName = row["SellerName"].ToString();
                msgContent = string.Format("您好：{0}的{1}：请您于{2}之前，将一车LNG送到{3}的{4}，该站隶属于{5}，站内联系人：{6}，联系方式{7}；请您及时与{8}确认信息。",
                    seller, driverName, arriveTime, stationAddress, userTypeName, buyer, contactName, contactTel, sellerName);
                SMS sms = new SMS();
                sms.SendTemplate(driverTel, "52016", new[] { seller, driverName, arriveTime, stationAddress, userTypeName, buyer, contactName, contactTel, sellerName });
                if (driverID != Guid.Empty) SqlProcs.spTQMessageNew(driverID, msgContent, null);
            }
        }

        /// <summary>
        /// 车辆出发
        /// </summary>
        /// <param name="orderDetailID"></param>
        /// <param name="msgContent"></param>
        /// <param name="mode">true：发送给买家, false：发送给卖家</param>
        public static void DepartCar(Guid orderDetailID, bool mode = false)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderDetailID);
            DataRow row = DAL.GetRow("vwTQOrderDetail_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                Guid buyerID = Sql.ToGuid(row["BuyerID"]);
                string sellerPhone = row["SellerPhone"].ToString();
                string buyerPhone = row["BuyerPhone"].ToString();
                string seller = row["Seller"].ToString();
                string buyer = row["Buyer"].ToString();
                string driverName = row["C_Driver"].ToString();
                string strContent;
                SMS sms = new SMS();
                if (mode)   // 买方
	            {
                    sms.SendTemplate(buyerPhone, "56933", new[] { seller, driverName });
                    strContent = string.Format("您公司订购{0}的LNG已经由驾驶员{1}上传《随货通讯单》，请及时登录平台或手机客户端查看车辆实时位置。",
                                seller, driverName);
                    SqlProcs.spTQMessageNew(buyerID, strContent, "/member/Order/");
	            }
                else
                {
                    sms.SendTemplate(sellerPhone, "56932", new[] { buyer, driverName });
                    strContent = string.Format("您公司销售给{0}的LNG已经由驾驶员{1}上传《随货通讯单》，请及时登录平台或手机客户端查看车辆实时位置。",
                        buyer, driverName);
                    SqlProcs.spTQMessageNew(sellerID, strContent, "/member/OrderSell/");
                }
            }
        }

        public static void Receipt(Guid orderDetailID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", orderDetailID);
            DataRow row = DAL.GetRow("vwTQOrderDetail_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                string sellerPhone = row["SellerPhone"].ToString();
                string buyer = row["Buyer"].ToString();
                string SN = row["SN"].ToString();
                string strContent = string.Format("您公司销售给{0}的LNG，买方已经收货，请您及时查看！单号：{1}", buyer, SN);
                SMS sms = new SMS();
                sms.SendTemplate(sellerPhone, "56934", new[] { buyer, SN });
                SqlProcs.spTQMessageNew(sellerID, strContent, "/member/OrderSell/");
            }
        }

        public static void QuoteDetailPrice(Guid quoteDetailID, bool modeEdit = false)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", quoteDetailID);
            DataRow row = DAL.GetRow("vwTQQuoteDetail_List", ht);
            if (row != null)
            {
                Guid buyerID = Sql.ToGuid(row["BuyerID"]);
                string buyerPhone = row["BuyerPhone"].ToString();
                string strContent;
                SMS sms = new SMS();
                if (modeEdit)   // 编辑
                {
                    strContent = string.Format("您的求购需求中有卖家调整报价，请您及时登录查看报价！");
                    sms.SendTemplate(buyerPhone, "56936", new[] { "" });
                    SqlProcs.spTQMessageNew(buyerID, strContent, "/member/Quote/");
                }
                else // 新报价
                {
                    strContent = string.Format("您的求购需求有新的卖家报价，请您及时查看报价！");
                    sms.SendTemplate(buyerPhone, "56938", new[] { "" });
                    SqlProcs.spTQMessageNew(buyerID, strContent, "/member/Quote/");
                }
            }
        }

        public static void AcceptQuotePrice(Guid quotePriceID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", quotePriceID);
            DataRow row = DAL.GetRow("vwTQQuotePrice_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                string sellerPhone = row["SellerPhone"].ToString();
                string buyer = row["Buyer"].ToString();
                string strContent = string.Format("您给{0}气源报价，该公司已确认！请及时查看", buyer);
                SMS sms = new SMS();
                sms.SendTemplate(sellerPhone, "56939", new[] { buyer });
                SqlProcs.spTQMessageNew(sellerID, strContent, "/member/OrderSell/");
            }
        }

        public static void QuoteApply(Guid quoteID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", quoteID);
            DataRow row = DAL.GetRow("vwTQQuote_List", ht);
            if (row != null)
            {
                string dateQuote = row["DATE_ENTERED"].ToString();
                string strContent = string.Format("有买家发布求购审核（{0}）", dateQuote);
                ht.Clear();
                ht.Add("IS_ADMIN", 1);
                DataTable dt = DAL.GetTable("vwUSERS", ht);
                foreach (DataRow rowUsers in dt.Rows)
                {
                    Guid gTo = Sql.ToGuid(rowUsers["ID"]);
                    SqlProcs.spTQMessageNew(gTo, strContent, "/member/QuoteCheck/");
                }
            }
        }

        public static void QuoteAudit(Guid quoteID, bool modePass = true)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", quoteID);
            DataRow row = DAL.GetRow("vwTQQuote_List", ht);
            if (row != null)
            {
                Guid buyerID = Sql.ToGuid(row["BuyerID"]);
                string buyerPhone = row["BuyerPhone"].ToString();
                SMS sms = new SMS();
                string strContent;
                if (modePass)
                {
                    strContent = string.Format("您发布的求购需求平台管理员已经通过审核，请等待卖家报价！");
                    sms.SendTemplate(buyerPhone, "56942", new[] { "" });
                    SqlProcs.spTQMessageNew(buyerID, strContent, "/member/Quote/");
                }
                else
                {
                    strContent = string.Format("您发布的求购需求平台管理员未通过审核，请联系在线客服");
                    SqlProcs.spTQMessageNew(buyerID, strContent, null);
                }
                
            }
        }

        public static void TransitApply(Guid transitID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", transitID);
            DataRow row = DAL.GetRow("vwTQTransit_List", ht);
            if (row != null)
            {
                string seller = row["Seller"].ToString();
                string dateQuote = row["DATE_ENTERED"].ToString();
                string strContent = string.Format("有{0}发布在途气审核（{1}）", seller, dateQuote);
                ht.Clear();
                ht.Add("IS_ADMIN", 1);
                DataTable dt = DAL.GetTable("vwUSERS", ht);
                foreach (DataRow rowUsers in dt.Rows)
                {
                    Guid gTo = Sql.ToGuid(rowUsers["ID"]);
                    SqlProcs.spTQMessageNew(gTo, strContent, "/member/TransitCheck/");
                }
            }
        }

        public static void TransitAudit(Guid transitID, bool modePass = true)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", transitID);
            DataRow row = DAL.GetRow("vwTQTransit_List", ht);
            if (row != null)
            {
                Guid sellerID = Sql.ToGuid(row["SellerID"]);
                string sellerPhone = row["SellerPhone"].ToString();
                SMS sms = new SMS();
                string strContent;
                if (modePass)
                {
                    strContent = string.Format("您发布的在途气平台管理员已经通过审核！");
                    sms.SendTemplate(sellerPhone, "56943", new[] { "" });
                    SqlProcs.spTQMessageNew(sellerID, strContent, "/member/TransitSell/");
                }
                else
                {
                    strContent = string.Format("您发布的在途气平台管理员未通过审核，请联系在线客服！");
                    SqlProcs.spTQMessageNew(sellerID, strContent, null);
                }
            }
        }

        public static void AcceptTransit(Guid myTransitID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", myTransitID);
            DataRow row = DAL.GetRow("vwTQTransitMy_List", ht);
            if (row != null)
            {
                Guid buyerID = Sql.ToGuid(row["BuyerID"]);
                string strContent = string.Format("您抢购的在途气卖家已确认！请及时查看");
                SqlProcs.spTQMessageNew(buyerID, strContent, "/member/Transit/");
            }
        }
    }
}