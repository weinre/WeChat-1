﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeChat.Business.APP;
using WeChat.Business.Base;
using WeChat.Business.Model;
using WeChat.Business.Net;
using WeChat.Business.Utils;

namespace WeChat.Business.BLL
{
    /***
    * ===========================================================
    * 创建人：袁建廷
    * 创建时间：2017/06/28 16:22:46
    * 说明：
    * ==========================================================
    * */
    public class RContactManager : BaseManager
    {

        public RContactManager(HttpTools http)
            : base(http)
        {

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public void Webwxinit(SendOrPostCallback d)
        {
            UserResponse response = null;
            try
            {
                string url = Context.base_uri + "/webwxinit?lang=zh_CN&pass_ticket=" + Context.pass_ticket;

                string json = http.PostData(url, Context.BaseRequest);

                response = JsonConvert.DeserializeObject<UserResponse>(json);
                if (response != null)
                {
                    Context.user = response.User;
                    Context.SyncKeys = response.SyncKey;
                    string[] array = new string[response.SyncKey.Count];
                    int i = 0;
                    foreach (Model.Keys item in response.SyncKey.List)
                    {
                        array[i] = item.Key + "_" + item.Val;
                        i++;
                    }
                    Context.synckey = System.Web.HttpUtility.UrlEncode(string.Join("|", array));/////接收不到消息


                    //foreach (RContact item in response.ContactList)
                    //{
                    //    try
                    //    {
                    //        string key = item.UserName;
                    //        if (item.VerifyFlag == 8 || item.VerifyFlag == 24)
                    //        {
                    //            if (Context.PublicUsersList.ContainsKey(key) == false)
                    //            {
                    //                Context.PublicUsersList.Add(key, item);
                    //            }
                    //        }
                    //        else if (key.StartsWith("@@")) //群聊
                    //        {
                    //            if (!Context.GroupList.ContainsKey(key))
                    //            {
                    //                Context.GroupList.Add(key, item);
                    //            }
                    //        }
                    //        else if (Context.SpecialUsers.Contains(key))//特殊账号
                    //        {
                    //            if (!Context.SpecialUsersList.ContainsKey(key))
                    //            {
                    //                Context.SpecialUsersList.Add(key, item);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (!Context.ContactList.ContainsKey(key))
                    //            {
                    //                Context.ContactList.Add(key, item);
                    //            }
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {

                    //    }
                    //}
                }

            }
            catch (Exception e)
            {
                LogHandler.e(e);
            }
            if (m_SyncContext != null)
                m_SyncContext.Post(d, response);
        }


        /// <summary>
        /// 获取好友信息
        /// </summary>
        /// <param name="d"></param>
        public void GetRContact(SendOrPostCallback d)
        {
            ContactResponse response = null;
            try
            {
                string url = Context.base_uri + "/webwxgetcontact?lang=zh_CN&pass_ticket=" + Context.pass_ticket + "&r=" + Tools.GetTimeStamp() + "&seq=0&skey=" + Context.skey;
                string json = http.GetPage(url);
                response = JsonConvert.DeserializeObject<ContactResponse>(json);
                if (response != null)
                {
                    //response.MemberCount = response.MemberCount - 1;//把自己减去
                    foreach (RContact item in response.MemberList)
                    {
                        if (!Context.ContactList.ContainsKey(item.UserName))
                        {
                            Context.ContactList.Add(item.UserName, item);
                        }else
                        {
                            Context.ContactList[item.UserName] = item;
                        }
                        //try
                        //{
                        //    string key = item.UserName;
                        //    if (item.VerifyFlag == 8 || item.VerifyFlag == 24)
                        //    {
                        //        if (Context.PublicUsersList.ContainsKey(key) == false)
                        //        {
                        //            Context.PublicUsersList.Add(key, item);
                        //        }
                        //        else
                        //        {
                        //            Context.PublicUsersList[key] = item;
                        //        }
                        //    }
                        //    else if (key.StartsWith("@@")) //群聊
                        //    {
                        //        if (!Context.GroupList.ContainsKey(key))
                        //        {
                        //            Context.GroupList.Add(key, item);
                        //        }
                        //        else
                        //        {
                        //            Context.GroupList[key] = item;
                        //        }
                        //    }
                        //    else if (Context.SpecialUsers.Contains(key))//特殊账号
                        //    {
                        //        if (!Context.SpecialUsersList.ContainsKey(key))
                        //        {
                        //            Context.SpecialUsersList.Add(key, item);
                        //        }
                        //        else
                        //        {
                        //            Context.SpecialUsersList[key] = item;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (!Context.ContactList.ContainsKey(key))
                        //        {
                        //            Context.ContactList.Add(key, item);
                        //        }
                        //        else
                        //        {
                        //            Context.ContactList[key] = item;
                        //        }
                        //    }
                        //}
                        //catch (Exception e)
                        //{
                        //    LogHandler.e(e);
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                LogHandler.e(e);
            }

            if (m_SyncContext != null)
                m_SyncContext.Post(d, response);
        }


        /// <summary>
        /// 查询指定群 好友信息
        /// 获取群聊天会话列表信息（post）：
        /// </summary>
        public BatchgetContactResponse webwxbatchgetcontact(string UserName)
        {

            try
            {
                string url = Context.base_uri + "/webwxbatchgetcontact?lang=zh_CN&type=ex&r=" + Tools.GetTimeStamp() + "&pass_ticket=" + Context.pass_ticket;
                ParamBean baseRuquest = JsonConvert.DeserializeObject<ParamBean>(Context.BaseRequest);

                List<EncryChatRoom> list = new List<EncryChatRoom>();

                EncryChatRoom room = new EncryChatRoom();
                room.EncryChatRoomId = "";
                room.UserName = UserName;
                list.Add(room);


                var param = new
                {
                    BaseRequest = baseRuquest.BaseRequest,
                    Count = list.Count,
                    List = list
                };
                string poatData = JsonConvert.SerializeObject(param);
                string json = http.PostData(url, poatData);

                BatchgetContactResponse response = JsonConvert.DeserializeObject<BatchgetContactResponse>(json);
                return response;
            }
            catch (Exception ex)
            {
                LogHandler.e(ex);
            }
            return null;
        }

    }


    class ParamBean
    {
        public BaseRequest BaseRequest { get; set; }
    }

    class EncryChatRoom
    {
        public string EncryChatRoomId { get; set; }
        public string UserName { get; set; }
    }
}
